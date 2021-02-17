/*
	The following license supersedes all notices in the source code.

	Copyright (c) 2021 Kurt Dekker/PLBM Games All rights reserved.

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

public class RotatedControlsPlayerController : MonoBehaviour
{
	public float BaseMoveSpeed;

	public Transform TheCamera;

	// optional; put one of these on here if you want to have its benefits
	CharacterController cc;

	IPushable pushable;

	private void Reset()
	{
		BaseMoveSpeed = 10.0f;
	}

	VAButton vabMove;
	VAButton vabCamera;

	const float TurnToFaceRate = 500;       // degrees per second

	const float CameraInfluence = 10.0f;
	const float VerticalCameraAttenuation = 0.1f;

	// how much must the controls be deflected before we turn to face?
	const float MinimumInputForRotation = 0.1f;

	void CreateVABs()
	{
		if (vabMove) Destroy(vabMove);
		if (vabCamera) Destroy(vabCamera);

		vabMove = gameObject.AddComponent<VAButton>();
		vabMove.r_downable = MR.SR(0.00f, 0.20f, 0.50f, 0.80f);
		vabMove.doClamp = true;
		vabMove.doNormalize = false;
		vabMove.label = "MOVE";

		if (pushable != null)
		{
			vabCamera = gameObject.AddComponent<VAButton>();
			vabCamera.r_downable = MR.SR(0.50f, 0.20f, 0.50f, 0.80f);
			vabCamera.doClamp = true;
			vabCamera.doNormalize = false;
			vabCamera.label = "CAMERA";
		}
	}

	void Start()
	{
		// see if the camera is pushable first. If it isn't, we won't make a camera control
		pushable = TheCamera.GetComponent<IPushable>();

		CreateVABs();

		OrientationChangeSensor.Create(transform, () => { CreateVABs(); });

		// if we find a CharacterController, use it
		cc = GetComponent<CharacterController>();

		// if we have a CharacterController, adjust our child visual
		if (cc)
		{
			transform.GetChild(0).localPosition = Vector3.zero;
		}
	}

	void AddKeyboardMovementControls(ref Vector3 raw)
	{
		raw.x += Input.GetAxis("Horizontal");
		raw.z += Input.GetAxis("Vertical");
	}

	void HandlePlayerMovement()
	{
		Vector3 RawMovementInputs = Vector3.zero;

		if (vabMove.fingerDown)
		{
			RawMovementInputs = new Vector3(vabMove.outputRaw.x, 0, vabMove.outputRaw.y);
		}

		AddKeyboardMovementControls(ref RawMovementInputs);

		var cameraHeading = TheCamera.transform.eulerAngles.y;

		var controlRotation = Quaternion.Euler(0, cameraHeading, 0);

		var RotatedMoveInputs = controlRotation * RawMovementInputs;

		var motion = RotatedMoveInputs * BaseMoveSpeed * Time.deltaTime;

		if (cc)
		{
			cc.Move(motion + Physics.gravity * Time.deltaTime);
		}
		else
		{
			transform.position += motion;
		}

		// turn to face our motion if we're moving "enough"
		if (RotatedMoveInputs.magnitude >= MinimumInputForRotation)
		{
			float controlsFacing = Mathf.Atan2(RotatedMoveInputs.x, RotatedMoveInputs.z) * Mathf.Rad2Deg;

			float myCurrentHeading = transform.eulerAngles.y;

			myCurrentHeading = Mathf.MoveTowardsAngle(myCurrentHeading, controlsFacing, TurnToFaceRate * Time.deltaTime);

			transform.rotation = Quaternion.Euler(0, myCurrentHeading, 0);
		}
	}

	void AddKeyboardCameraControls( ref Vector3 raw)
	{
		// keyboard overrides
		if (Input.GetKey(KeyCode.Alpha1))
		{
			raw.x = -1;
		}
		if (Input.GetKey(KeyCode.Alpha2))
		{
			raw.x = 1;
		}
	}

	void HandleCameraMovement()
	{
		if (pushable != null)
		{
			Vector3 RawCameraInputs = Vector3.zero;

			if (vabCamera.fingerDown)
			{
				RawCameraInputs = new Vector3(vabCamera.outputRaw.x, 0, vabCamera.outputRaw.y);
			}

			// drastically reduce up/down input; we want to encourage circling around
			RawCameraInputs.y *= VerticalCameraAttenuation;

			AddKeyboardCameraControls(ref RawCameraInputs);

			var cameraHeading = TheCamera.transform.eulerAngles.y;

			var controlRotation = Quaternion.Euler(0, cameraHeading, 0);

			var RotatedMoveInputs = controlRotation * RawCameraInputs;

			var motion = RotatedMoveInputs * CameraInfluence * Time.deltaTime;

			pushable.Push(motion);
		}
	}

	void Update()
	{
		HandlePlayerMovement();

		HandleCameraMovement();
	}
}
