using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( CylindricalPosition))]
public class CylindricalEnemyShip : MonoBehaviour
{
	public CylindricalPosition ThePlayer;

	CylindricalPosition cp;

	float age;
	float phase;		// drives the sine wave

	// how quick around the tube he goes
	const float EnemyAngularRotation = 20;
	// how quickly in and out it goes on the sine wave pattern
	const float EnemyPhaseInOutRate = 1.0f;

	const float EnemyShotSpeed = 100;

	public GameObject TemplateEnemyShot;

	void Start ()
	{
		// turn it off; we'll turn on each copy we make
		TemplateEnemyShot.SetActive( false);

		cp = GetComponent<CylindricalPosition>();

		cp.Angle = 0;
		cp.Depth = 0;
	}

	void UpdateMovement()
	{
		age += Time.deltaTime;

		cp.Angle = age * EnemyAngularRotation;

		phase += Time.deltaTime * EnemyPhaseInOutRate;

		cp.Depth = (0.3f + Mathf.Sin( phase) * 0.7f) * CylindricalPosition.FullDepth;
	}

	float timeToShootYet;
	const float ShotInterval = 1.0f;

	void UpdateShooting()
	{
		timeToShootYet += Time.deltaTime;

		if (timeToShootYet >= ShotInterval)
		{
			timeToShootYet -= ShotInterval;

			var copy = Instantiate<GameObject>(TemplateEnemyShot);
			copy.SetActive( true);

			// MAGIC SAUCE: the aiming is just 2D delta!
			float deltaAngle = Mathf.DeltaAngle( cp.Angle, ThePlayer.Angle);
			float deltaDepth = ThePlayer.Depth - cp.Depth;

			// make vector to normalize it
			Vector2 aimDirection = new Vector2( deltaAngle, deltaDepth).normalized;

			// scale up by speed
			aimDirection *= EnemyShotSpeed;

			var ballistic = CylindricalBallisticItem.Attach( copy, cp,
				angleVelocity: aimDirection.x,
				depthVelocity: aimDirection.y);
		}
	}

	void Update()
	{
		UpdateMovement();

		UpdateShooting();

		// project us onto the cylinder
		CylindricalMapper.Instance.ProjectCylindricalToWorld3D( cp, transform);
	}
}
