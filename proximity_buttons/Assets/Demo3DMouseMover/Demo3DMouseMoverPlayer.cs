﻿using UnityEngine;

// @kurtdekker - simple 3D spaceship controller
// To use:
//	- Slap this on your main camera
//	- Press Play
//	- Wiggle mouse to steer
//	- Go where no human has gone before
//	- Enjoy
// Barf bag located in the seat pocket in front of you.

public class Demo3DMouseMoverPlayer : MonoBehaviour
{
	// increase if you feel the need
	float speed = 5;

	void Update ()
	{
		float pitch = Input.GetAxis( "Mouse Y");
		float roll = Input.GetAxis( "Mouse X");

		// increase to make things sportier
		pitch *= 20;
		roll *= 20;

		// make mouse-left roll to the left
		roll *= -1;

		// do you prefer pitch inverted? uncomment this!
		// pitch *= -1;

		transform.Rotate( Vector3.forward * roll);
		transform.Rotate( Vector3.right * pitch);

		transform.position += transform.forward * speed * Time.deltaTime;
	}
}
