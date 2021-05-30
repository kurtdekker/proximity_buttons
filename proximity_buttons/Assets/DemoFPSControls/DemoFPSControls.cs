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

public class DemoFPSControls : MonoBehaviour
{
	[Header( "Warning: works only on flat for simplicity.")]

	[Tooltip( "Meters / second")]
	public float WalkSpeed = 10;
	public float StrafeSpeed = 10;

	[Tooltip( "Degrees / second")]
	public float LookHeadingRate = 100;

	[Tooltip( "Degrees / second")]
	public float LookUpDownRate = 50;

	[Tooltip( "Degrees")]
	public float MinLookUpDown = -60.0f;
	public float MaxLookUpDown = 70.0f;

	[Tooltip( "We'll grab this camera and parent it at our eye height.")]
	public Camera CameraToGrab;

	public bool InvertXLook;
	public bool InvertYLook;

	public float EyeHeight = 1.8f;

	VAButton vabMove;
	VAButton vabFire;

	void CreateVABs()
	{
		if (vabMove) Destroy(vabMove);
		if (vabFire) Destroy(vabFire);

		float sz = MR.MINAXIS * 0.5f;

		vabMove = gameObject.AddComponent<VAButton>();
		vabMove.r_downable = new Rect(0, Screen.height - sz, sz, sz);
		vabMove.label = "move";
		vabMove.doClamp = true;
		vabMove.doNormalize = false;

		vabFire = gameObject.AddComponent<VAButton>();
		vabFire.r_downable = new Rect(Screen.width - sz, Screen.height - sz, sz, sz);
		vabFire.label = "look";
		vabFire.doClamp = true;
		vabFire.doNormalize = false;

		// TODO: if you want left-handed, swap the rectangles above right now...
	}

	float heading;
	float upDown;

	void Start ()
	{
		CreateVABs();
		OrientationChangeSensor.Create(transform, CreateVABs);

		upDown = 0;
		heading = transform.eulerAngles.y;

		if (!CameraToGrab)
		{
			Debug.LogWarning( "Grabbing main camera; set one if you want another camera!");
			CameraToGrab = Camera.main;
		}

		// grab the camera, bolt it up to our eye height
		CameraToGrab.transform.SetParent( transform);
		CameraToGrab.transform.localRotation = Quaternion.identity;
		CameraToGrab.transform.localPosition = Vector3.up * EyeHeight;
	}

	// accepted input
	Vector3 MoveInput;
	Vector3 LookInput;

	void UpdateGatherInput ()
	{
		MoveInput = Vector3.zero;
		LookInput = Vector3.zero;

		if (vabMove.fingerDown)
		{
			MoveInput = vabMove.output;
		}

		if (vabFire.fingerDown)
		{
			LookInput = vabFire.output;
		}

		if (InvertXLook)
		{
			LookInput.x = -LookInput.x;
		}
		if (InvertYLook)
		{
			LookInput.y = -LookInput.y;
		}
	}

	private void UpdateProcessLook()
	{
		heading += LookInput.x * LookHeadingRate * Time.deltaTime;

		upDown += LookInput.y * LookUpDownRate * Time.deltaTime;

		upDown = Mathf.Clamp( upDown, MinLookUpDown, MaxLookUpDown);

		transform.rotation = Quaternion.Euler( 0, heading, 0);

		CameraToGrab.transform.localRotation = Quaternion.Euler( -upDown, 0, 0);
	}

	private void UpdateProcessMove()
	{
		Vector3 move = transform.forward * MoveInput.y * WalkSpeed +
			transform.right * MoveInput.x * StrafeSpeed;

		transform.position += move * Time.deltaTime;
	}

	private void Update()
	{
		UpdateGatherInput();

		UpdateProcessLook();
		UpdateProcessMove();
	}
}
