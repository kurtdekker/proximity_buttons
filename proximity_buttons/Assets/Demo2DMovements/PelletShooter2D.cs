using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @kurtdekker - hang this on your favorite 2D spaceship
// or player or whatever to make it fire 2D bullets
//
// NOTE: if this object collides with your ship as
// you fire it, use layers and then set the Physics2D
// collision matrix to disable that layer interaction.

public class PelletShooter2D : MonoBehaviour
{
	[Header( "Either prefab or in-scene exemplar:")]
	[Header( "We'll add the Rigidbody2d in code.")]
	public GameObject ExemplarBullet;

	[Header( "Used for position and heading (up!)")]
	[Header( "Put it near the nose of your ship")]
	public Transform Muzzle;

	const float BulletSpeed = 8.0f;
	const float BulletInterval = 0.1f;

	void Start ()
	{
		if (ExemplarBullet.activeInHierarchy)
		{
			ExemplarBullet.SetActive( false);
		}
	}

	int shotsRequested;

	void UpdateGatherInput()
	{
		if (Input.GetKeyDown( KeyCode.Tab))
		{
			shotsRequested += 3;
		}
		if (Input.GetKeyDown( KeyCode.Space))
		{
			shotsRequested += 1;
		}
	}

	void Update ()
	{
		if (gunHeat > 0)
		{
			gunHeat -= Time.deltaTime;
		}

		UpdateGatherInput();
	}

	float gunHeat;

	void UpdateFireAnyPendingShot()
	{
		if (gunHeat <= 0)
		{
			if (shotsRequested > 0)
			{
				GameObject shot = Instantiate<GameObject>( ExemplarBullet, Muzzle.position, Muzzle.rotation);
				shot.SetActive(true);

				Rigidbody2D rb2d = shot.AddComponent<Rigidbody2D>();
				rb2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
				rb2d.drag = 0;

				Vector2 bulletDirection = shot.transform.up;

				Vector2 velocity = bulletDirection * BulletSpeed;
				rb2d.velocity = velocity;

				TTL.Attach( shot, 1.0f);

				shotsRequested--;

				gunHeat = BulletInterval;
			}
		}
	}

	void FixedUpdate()
	{
		UpdateFireAnyPendingShot();
	}
}
