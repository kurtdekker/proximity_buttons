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

public class TwinStickPlayerController : MonoBehaviour
{
	public float BaseMoveSpeed;
	public float WaveMoveSpeed;
	public float ShootSpeed;
	public float ShootRate;

	public GameObject BulletPrefab;

	float coolDown;

	void Reset()
	{
		BaseMoveSpeed = 5.0f;
		WaveMoveSpeed = 0.5f;
		ShootSpeed = 40.0f;
		ShootRate = 4;
	}

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

		if (BulletPrefab.activeInHierarchy)
		{
			BulletPrefab.SetActive( false);
		}
	}

	Vector3 LastPlayerMotion = Vector3.zero;

	void UpdateMoving()
	{
		if (bMove)
		{
			float currentSpeed = BaseMoveSpeed + DSM.Wave.iValue;

			LastPlayerMotion = new Vector3(
				v3Move.x,
				0,
				v3Move.y) * currentSpeed;

			transform.position += LastPlayerMotion * Time.deltaTime;

			float angle = Mathf.Rad2Deg * Mathf.Atan2( LastPlayerMotion.x, LastPlayerMotion.z);
			transform.rotation = Quaternion.Euler( 0, angle, 0);
		}

		TwinStickGameManager.I.PlayerPosition = transform.position;
	}

	Vector3 LastFireDirection = Vector3.forward;

	// fraction of player motion to add to bullets
	const float PlayerMotionFraction = 0.5f;

	void UpdateShooting()
	{
		if (coolDown > 0)
		{
			coolDown -= Time.deltaTime;
		}

		if (bShoot)
		{
			if (v3Shoot.magnitude > 0.2f)
			{
				LastFireDirection = new Vector3(
					v3Shoot.x, 0, v3Shoot.y).normalized;
			}

			if (coolDown <= 0)
			{
				coolDown = 1.0f / ShootRate;

				var Bullet = Instantiate<GameObject>( BulletPrefab);
				Bullet.SetActive( false);
				Bullet.transform.position = transform.position;
				Vector3 velocity = LastFireDirection * ShootSpeed;
				if (bMove)
				{
					velocity += LastPlayerMotion * PlayerMotionFraction;
				}
				float angle = Mathf.Rad2Deg * Mathf.Atan2( velocity.x, velocity.z);
				Bullet.transform.rotation = Quaternion.Euler( 0, angle, 0);
				Ballistic.Attach( Bullet, velocity);
				TTL.Attach( Bullet, 1.0f);
				Bullet.SetActive( true);

				TwinStickGameManager.I.AddBullet( Bullet);

				DSM.AudioShoot.Poke();
			}
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
		if (!DSM.GameRunning.bValue)
		{
			Destroy( gameObject);
			return;
		}

		// this demo gathers input from all sources. You would normally
		// only gather input for the sources on your platform / target.

		UpdateGatherTouchInput();

		UpdateGatherKeyboardInput();

		UpdateMoving();

		UpdateShooting();
	}
}
