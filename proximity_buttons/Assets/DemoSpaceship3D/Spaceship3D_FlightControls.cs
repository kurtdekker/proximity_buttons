using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Spaceship3D : MonoBehaviour
{
	void ProcessOrientation()
	{
		// find out our current slew in local axes
		var iq = Quaternion.Inverse(rb.rotation);
		Vector3 LocalAngularVelocity = iq * rb.angularVelocity;


//		LocalAngularVelocity = rb.angularVelocity;

//		Debug.Log(LocalAngularVelocity);

		// scale time-filtered inputs down to angularVelocity vectors
		// pay attention to the relevant axes swaps!
		// pitch rotates around the +X
		float inputX = pitchInput;
		// yaw rotates around the +Y
		float inputY = yawInput;
		// roll rotates around the +Z
		float inputZ = rollInput;

		float dt = Time.deltaTime;

		// apply control laws
		float errorX = rollFilter.Compute(LocalAngularVelocity.x, inputX, dt);
		float errorY = yawFilter.Compute(LocalAngularVelocity.y, inputY, dt);
		float errorZ = pitchFilter.Compute(LocalAngularVelocity.z, inputZ, dt);

		// scale error terms up by reaction control system thruster power
		errorX *= 0.5f;
		errorY *= 0.5f;
		errorZ *= 0.5f;

		Vector3 finalTorque = new Vector3(errorX, errorY, errorZ);

		// wrap it back around our rigidbody's local frame
		Vector3 localTorque = rb.rotation * finalTorque;

		// apply the final torque
		rb.AddTorque(localTorque);
	}

	void ProcessPower()
	{
		float powerBase = 25;

		rb.AddForce(transform.forward * (powerInput * powerBase));
	}

	void ProcessBrake()
	{
		float dragBase = 1;

		rb.drag = brakeInput * dragBase;
	}
}
