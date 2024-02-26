using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Spaceship3D : MonoBehaviour
{
	float rollInput;
	float pitchInput;
	float yawInput;
	float powerInput;
	float brakeInput;

	const float rollSnappiness = 5.0f;
	const float pitchSnappiness = 5.0f;
	const float yawSnappiness = 5.0f;
	const float powerSnappiness = 5.0f;
	const float brakeSnappiness = 5.0f;

	void GatherTimeFilteredInputs()
	{
		float rawRoll = 0;
		float rawPitch = 0;
		float rawYaw = 0;
		float rawPower = 0;
		float rawBrake = 0;

		// get the raw inputs
		if (Input.GetKey(keyRollLeft)) rawRoll = +1;
		if (Input.GetKey(keyRollRight)) rawRoll = -1;
		if (Input.GetKey(keyPitchUp)) rawPitch = -1;
		if (Input.GetKey(keyPitchDown)) rawPitch = +1;
		if (Input.GetKey(keyYawLeft)) rawYaw = -1;
		if (Input.GetKey(keyYawRight)) rawYaw = +1;
		if (Input.GetKey(keyPower)) rawPower = +1;
		if (Input.GetKey(keyBrake)) rawBrake = +1;

		if (invertPitch) rawPitch = -rawPitch;

		float dt = Time.deltaTime;

		// time-filter the inputs
		rollInput = Mathf.Lerp(rollInput, rawRoll, rollSnappiness * dt);
		pitchInput = Mathf.Lerp(pitchInput, rawPitch, pitchSnappiness * dt);
		yawInput = Mathf.Lerp(yawInput, rawYaw, yawSnappiness * dt);
		powerInput = Mathf.Lerp(powerInput, rawPower, powerSnappiness * dt);
		brakeInput = Mathf.Lerp(brakeInput, rawBrake, brakeSnappiness * dt);
	}
}
