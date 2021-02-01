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

	// how quickly in and out it goes
	const float PhaseRate = 3.0f;

	const float EnemyShotSpeed = 100;

	public GameObject TemplateEnemyShot;

	void Start ()
	{
		TemplateEnemyShot.SetActive( false);

		cp = GetComponent<CylindricalPosition>();

		cp.Angle = 0;
		cp.Depth = 0;
	}

	void UpdateMovement()
	{
		age += Time.deltaTime;

		cp.Angle = age * 100;

		phase += Time.deltaTime * PhaseRate;

		cp.Depth = (0.5f + Mathf.Sin( phase) * 0.4f) * CylindricalPosition.FullDepth;
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
			float deltaAngle = Mathf.DeltaAngle( ThePlayer.Angle, cp.Angle);
			float deltaDepth = ThePlayer.Depth - cp.Angle;

			// make vector to normalize it
			Vector2 aimDirection = new Vector2( deltaAngle, deltaDepth).normalized;

			// scale up by speed
			aimDirection *= EnemyShotSpeed;

			var ballistic = CylindricalBallisticItem.Attach( copy, cp,
				angleVelocity: aimDirection.x,
				depthVelocity: aimDirection.y);

			copy.AddComponent<TTL>().ageLimit = 3.0f;
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
