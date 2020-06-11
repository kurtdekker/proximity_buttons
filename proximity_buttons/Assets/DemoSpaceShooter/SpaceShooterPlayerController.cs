/*
	The following license supersedes all notices in the source code.

	Copyright (c) 2020 Kurt Dekker/PLBM Games All rights reserved.

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

public class SpaceShooterPlayerController : MonoBehaviour
{
	public float BaseMoveSpeed;
	public float ShootSpeed;
	public float ShootRate;

	public GameObject Bullet1Prefab;
	public GameObject Bullet2Prefab;

	float coolDown;

	void Reset()
	{
		BaseMoveSpeed = 5.0f;
		ShootSpeed = 40.0f;
		ShootRate = 4;
	}

	VAButton vabMove;
	ProximityButtonSet pbsWeapons;
	ProximityButtonSet.ProximityButton pbFire1;
	ProximityButtonSet.ProximityButton pbFire2;

	void CreateVABs()
	{
		if (vabMove) Destroy( vabMove);
		if (pbsWeapons) Destroy(pbsWeapons);

		vabMove = gameObject.AddComponent<VAButton>();
		vabMove.r_downable = MR.SR( 0.00f, 0.20f, 0.50f, 0.80f);
		vabMove.doClamp = true;
		vabMove.doNormalize = false;
		vabMove.label = "MOVE";

		float sz = Mathf.Min(Screen.width, Screen.height) * 0.35f;
		pbsWeapons = ProximityButtonSet.Create(sz);
		pbFire1 = pbsWeapons.AddButton("FIRE1", MR.SR(0.7f, 0.9f, 0, 0).center);
		pbFire2 = pbsWeapons.AddButton("FIRE2", MR.SR(0.9f, 0.8f, 0, 0).center);

	}

	void Start ()
	{
		CreateVABs();

		OrientationChangeSensor.Create( transform, () => { CreateVABs(); });

		if (Bullet1Prefab.activeInHierarchy)
		{
			Bullet1Prefab.SetActive(false);
		}
		if (Bullet2Prefab.activeInHierarchy)
		{
			Bullet2Prefab.SetActive(false);
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
		}

		SpaceShooterGameManager.I.PlayerPosition = transform.position;
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

		if (bShoot1)
		{
			if (coolDown <= 0)
			{
				coolDown = 1.0f / ShootRate;

				var Bullet = Instantiate<GameObject>( Bullet1Prefab);
				Bullet.SetActive( false);
				Bullet.transform.position = transform.position;
				Vector3 velocity = Vector3.forward * ShootSpeed;
				float angle = Mathf.Rad2Deg * Mathf.Atan2( velocity.x, velocity.z);
				Bullet.transform.rotation = Quaternion.Euler( 0, angle, 0);
				Ballistic.Attach( Bullet, velocity);
				TTL.Attach( Bullet, 1.0f);
				Bullet.SetActive( true);

				SpaceShooterGameManager.I.AddBullet( Bullet);

				DSM.AudioShoot.Poke();
			}
		}

		if (bShoot2)
		{
			if (coolDown <= 0)
			{
				coolDown = 1.0f / ShootRate;

				var Bullet = Instantiate<GameObject>(Bullet2Prefab);
				Bullet.SetActive(false);
				Bullet.transform.position = transform.position;
				Vector3 velocity = Vector3.forward * ShootSpeed;
				float angle = Mathf.Rad2Deg * Mathf.Atan2(velocity.x, velocity.z);
				Bullet.transform.rotation = Quaternion.Euler(0, angle, 0);
				Ballistic.Attach(Bullet, velocity);
				TTL.Attach(Bullet, 1.0f);
				Bullet.SetActive(true);

				SpaceShooterGameManager.I.AddBullet(Bullet);

				DSM.AudioShoot.Poke();
			}
		}
	}

	bool bMove;
	Vector3 v3Move;
	bool bShoot1;
	bool bShoot2;

	void UpdateGatherTouchInput()
	{
		bMove = vabMove.fingerDown;
		v3Move = vabMove.outputRaw;

		bShoot1 = pbFire1.fingerDown;
		bShoot2 = pbFire2.fingerDown;
	}

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
		if (Input.GetKey(KeyCode.Alpha1))
		{
			bShoot1 = true;
		}
		if (Input.GetKey(KeyCode.Alpha2))
		{
			bShoot2 = true;
		}
	}

	void Update()
	{
		if (!DSM.GameRunning.bValue)
		{
			Destroy( gameObject);
			return;
		}

		UpdateGatherTouchInput();

		UpdateGatherKeyboardInput();

		UpdateMoving();

		UpdateShooting();
	}
}
