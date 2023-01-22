/*
	The following license supersedes all notices in the source code.

	Copyright (c) 2023 Kurt Dekker/PLBM Games All rights reserved.

	http://www.twitter.com/kurtdekker

	Redistribution and use in source and binary forms, with or without
	modification, are permitted provided that the following conditions are
	met:

	Redistributions of source code must retain the above copyright notice,
	this list of conditions and the following disclaimer.

	Redistributions in binary form must reproduce the above copyright
	notice, this list of conditions and the following disclaimer in the
	documentation and/or other materials provided with the distribution.

	Neither the name of the Kurt Dekker/PLBM Games nor the names of its
	contributors may be used to endorse or promote products derived from
	this software without specific prior written permission.

	THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS
	IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED
	TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
	PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
	HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
	SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
	TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
	PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
	LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
	NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
	SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinStickTankController : MonoBehaviour
{
	public Transform TankBody;
	public Transform TankTurret;
	public Transform TankMuzzle;		// where to send the bullet out from

	public float TankDrivingSpeed = 1;

	[Header( "What your gun shoots.")]
	public GameObject BulletPrefab;

	[Header( "Shots per second.")]
	public float FireRate = 4;
	public float ShotSpeed = 10;

	[Header( "Maintain heading when not aiming?")]
	public bool MaintainTurretHeading = true;

	[Header( "Can you back up if that's most-direct?")]
	public bool CanReverseTank = true;

	[Header( "Turning values.")]
	public float TurnRate = 100;
	public float AimRate = 250;

	float currentBodyHeading;
	float currentTurretHeading;

	VAButton vabMove;
	VAButton vabShoot;

	void CreateVABs()
	{
		if (vabMove) Destroy( vabMove);
		if (vabShoot) Destroy( vabShoot);

		vabMove = gameObject.AddComponent<VAButton>();
		vabMove.r_downable = MR.SR( 0.00f, 0.20f, 0.50f, 0.80f);
		vabMove.doClamp = true;
		vabMove.doNormalize = false;
		vabMove.label = "MOVE";

		vabShoot = gameObject.AddComponent<VAButton>();
		vabShoot.r_downable = MR.SR( 0.50f, 0.20f, 0.50f, 0.80f);
		vabShoot.doClamp = true;
		vabShoot.doNormalize = false;
		vabShoot.label = "SHOOT";
	}

	void Start ()
	{
		CreateVABs();
		OrientationChangeSensor.Create( transform, () => { CreateVABs(); });

		// so it works as a prefab or just lying in-scene
		if (BulletPrefab.activeInHierarchy) BulletPrefab.SetActive( false);
	}

	void UpdateMoving()
	{
		if (bMove)
		{
			Vector3 playerMotion = new Vector3(
				v3Move.x,
				0,
				v3Move.y);

			float speed = playerMotion.magnitude * TankDrivingSpeed;

			// only turn if there's a signal
			if (playerMotion.magnitude >= 0.1f)
			{
				float heading = Mathf.Rad2Deg * Mathf.Atan2( playerMotion.x, playerMotion.z);

				if (CanReverseTank)
				{
					float heading2 = heading + 180;

					// is it shorter to back up? If so use that instead
					float delta = Mathf.DeltaAngle( currentBodyHeading, heading);
					float delta2 = Mathf.DeltaAngle( currentBodyHeading, heading2);

					if (Mathf.Abs( delta2) < Mathf.Abs( delta))
					{
						heading = heading2;
						speed = -speed;
					}
				}

				currentBodyHeading = Mathf.MoveTowardsAngle( currentBodyHeading, heading, TurnRate * Time.deltaTime);

				TankBody.rotation = Quaternion.Euler( 0, currentBodyHeading, 0);
			}

			TankBody.position += TankBody.forward * speed * Time.deltaTime;
		}
	}

	float gunHeat;

	void UpdateShooting()
	{
		if (gunHeat > 0)
		{
			gunHeat -= Time.deltaTime;
		}

		bool driveRotation = MaintainTurretHeading;

		if (bShoot)
		{
			Vector3 playerShooting = new Vector3(
				v3Shoot.x,
				0,
				v3Shoot.y);

			// only aim if there's a signal
			if (playerShooting.magnitude >= 0.1f)
			{
				float heading = Mathf.Rad2Deg * Mathf.Atan2( playerShooting.x, playerShooting.z);
				currentTurretHeading = Mathf.MoveTowardsAngle( currentTurretHeading, heading, AimRate * Time.deltaTime);

				driveRotation = true;
			}

			// fire weapon here! start it at the muzzle and fire
			if (gunHeat <= 0)
			{
				var bullet = Instantiate<GameObject>( BulletPrefab, TankMuzzle.position, TankMuzzle.rotation);
				bullet.SetActive( true);

				Vector3 shotVelocity = TankMuzzle.forward * ShotSpeed;

				// move
				Ballistic.Attach( bullet, shotVelocity);

				// death sentence
				TTL.Attach( bullet, 2.0f);

				// cooldown
				gunHeat += 1.0f / FireRate;
			}
		}

		if (driveRotation)
		{
			TankTurret.rotation = Quaternion.Euler( 0, currentTurretHeading, 0);
		}
	}

	// we have movement input
	bool bMove;
	// this is the movement input vector
	Vector3 v3Move;
	// we have shooting input
	bool bShoot;
	// this is the shooting input vector
	Vector3 v3Shoot;

	// add touch inputs
	void UpdateGatherTouchInput()
	{
		bMove = vabMove.fingerDown;
		v3Move = vabMove.outputRaw;

		bShoot = vabShoot.fingerDown;
		v3Shoot = vabShoot.outputRaw;
	}

	// add keyboard inputs
	void UpdateGatherKeyboardInput()
	{
		// move
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			bMove = true;
			v3Move.x = -1;
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			bMove = true;
			v3Move.x =  1;
		}
		if (Input.GetKey(KeyCode.UpArrow))
		{
			bMove = true;
			v3Move.y = 1;
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			bMove = true;
			v3Move.y = -1;
		}

		// shoot
		if (Input.GetKey(KeyCode.A))
		{
			bShoot = true;
			v3Shoot.x = -1;
		}
		if (Input.GetKey(KeyCode.D))
		{
			bShoot = true;
			v3Shoot.x =  1;
		}
		if (Input.GetKey(KeyCode.W))
		{
			bShoot = true;
			v3Shoot.y = 1;
		}
		if (Input.GetKey(KeyCode.S))
		{
			bShoot = true;
			v3Shoot.y = -1;
		}
	}

	void Update()
	{
		// this demo gathers input from all sources. You would normally
		// only gather input for the sources on your platform / target.

		UpdateGatherTouchInput();

		UpdateGatherKeyboardInput();

		UpdateMoving();

		UpdateShooting();
	}
}
