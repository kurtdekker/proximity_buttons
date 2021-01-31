using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( CylindricalPosition))]
public class CylindricalEnemyShip : MonoBehaviour
{
	CylindricalPosition cp;

	float age;
	float phase;		// drives the sine wave

	// how quickly in and out it goes
	const float PhaseRate = 3.0f;

	void Start ()
	{
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

	void Update()
	{
		UpdateMovement();

		// project us onto the cylinder
		CylindricalMapper.Instance.ProjectCylindricalToWorld3D( cp, transform);
	}
}
