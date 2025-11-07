using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SpaceshipComplex2D
{
	void FixedUpdate_ApplyPhysics()
	{
		float mass = 0;
		for (int i = 0; i < Bodies.Count; i++)
		{
			var body = Bodies[i];
			mass += body.mass;
		}

		mass *= Physics2D.gravity.magnitude;

		float leftForce = thrustLeftAccumulator * mass * 0.5f;
		float rightForce = thrustRightAccumulator * mass * 0.5f;

		leftEngine.AddForce(leftEngine.transform.up * leftForce);
		rightEngine.AddForce(rightEngine.transform.up * rightForce);
	}
}
