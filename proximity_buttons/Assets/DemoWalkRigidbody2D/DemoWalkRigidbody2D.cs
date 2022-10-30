using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @kurtdekker
//
// walks in 2D with WASD and correctly handles
// collisions and blocking against 2d colliders.
//
// put this on the 2D player, where the Rigidbody2D must be

public class DemoWalkRigidbody2D : MonoBehaviour
{
	[Header( "Max walk speed.")]
	public float WalkSpeed;
	[Header( "How quick velocity can change.")]
	public float Acceleration;
	[Header( "How quick rotation can change.")]
	public float RotationalSnappiness;

	Rigidbody2D rb2d;

	void Reset()
	{
		WalkSpeed = 3.0f;
		Acceleration = 10.0f;
		RotationalSnappiness = 300.0f;
	}

	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate ()
	{
		// gather input, presuming nothing
		Vector2 movement = Vector2.zero;

		// old input system
		movement.x += Input.GetAxis( "Horizontal");
		movement.y += Input.GetAxis( "Vertical");

		// TODO: if you use new input, put it here!
		// movement.x += ... new input crud
		// movement.y += ... new input crud

		// END of collecting input

		// scale input up by speed
		movement *= WalkSpeed;

		// what were we rotated to? This tends to prevent
		// us from rotating if we just push on stuff
		float rotation = rb2d.rotation;

		// now we have the velocity... compute the new
		// direction, but only if velocity is "big enough."
		if (movement.magnitude > 0.1f)
		{
			// compute in radians
			float angle = Mathf.Atan2( -movement.x, movement.y);

			// convert to degrees
			angle *= Mathf.Rad2Deg;

			// move towards that angle
			rotation = Mathf.MoveTowardsAngle( rotation, angle, RotationalSnappiness * Time.deltaTime);
		}

		// set the rotation (possibly changed, but it might be the same too)
		rb2d.MoveRotation( rotation);

		// drive the velocity, including acceleration
		Vector2 workVelocity = rb2d.velocity;
		workVelocity = Vector2.MoveTowards( workVelocity, movement, Acceleration);
		rb2d.velocity = workVelocity;
	}
}
