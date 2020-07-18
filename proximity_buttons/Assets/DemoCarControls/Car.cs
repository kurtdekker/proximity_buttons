/*
	The following license supersedes all notices in the source code.

	Copyright (c) 2020 Kurt Dekker/PLBM Games All rights reserved.

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

using UnityEngine;

public partial class Car : MonoBehaviour
{
	public float maxTorque;
	public float maxBrake;

	float SteeredAngle;
	const float MaxRateOfSteerAngleChange = 120.0f;

	public Transform[] tireMeshes = new Transform[4];
	
	bool isMobile { get { return (
		Application.isEditor ||
		Application.platform == RuntimePlatform.Android ||
		Application.platform == RuntimePlatform.IPhonePlayer);
		}}

	Rigidbody rb;

	IMyUpdateable CameraUpdater;

	void Start()
	{
		rb = GetComponent<Rigidbody>();

		if (isMobile) StartMobile();

		var cam = Camera.main;
		CameraUpdater = cam.GetComponent<IMyUpdateable>();
	}
	
	void Update()
	{
		UpdateMeshesPositions();
	}

	float steer, accelerate, brake;

	float speed;
	const float acceleration = 1.0f;

	// if your controls are primarily forward/ahead,
	// make a center deadband in the steering.
	// If you are turning more, make a deadband in
	// the fore/aft control
	void ApplyMutuallyExclusiveDeadband()
	{
		const float margin = 1.5f;
		const float powerDeadband = 0.15f;
		const float steeringDeadband = 0.25f;

		if (Mathf.Abs( steer) > Mathf.Abs( accelerate) * margin)
		{
			var sign = Mathf.Sign( accelerate);
			accelerate = Mathf.Abs( accelerate) - powerDeadband;
			if (accelerate < 0) accelerate = 0;
			accelerate = sign * accelerate * (1.0f - powerDeadband);	// re-expand to 0-1
			return;
		}

		if (Mathf.Abs( accelerate) > Mathf.Abs( steer) * margin)
		{
			var sign = Mathf.Sign( steer);
			steer = Mathf.Abs( steer) - steeringDeadband;
			if (steer < 0) steer = 0;
			steer = sign * steer * (1.0f - steeringDeadband);	// re-expand to 0-1
			return;
		}
	}

	void FixedUpdate()
	{
		steer = Input.GetAxisRaw("Horizontal");
		accelerate = Input.GetAxisRaw("Vertical");
		brake = Input.GetKey (KeyCode.Space) ? 1.0f : 0.0f;

		if (isMobile) FixedUpdateMobile();

		ApplyMutuallyExclusiveDeadband();

		speed += acceleration * accelerate * Time.deltaTime;

		float finalAngle = steer * 45f;

		SteeredAngle = Mathf.MoveTowards( SteeredAngle, finalAngle, MaxRateOfSteerAngleChange * Time.deltaTime);

		// steer visuals
		tireMeshes[1].localRotation = Quaternion.Euler( 0, SteeredAngle, 0);
		tireMeshes[3].localRotation = Quaternion.Euler( 0, SteeredAngle, 0);

		// steer logical
		var rot = Quaternion.AngleAxis( SteeredAngle * speed, Vector3.up) * rb.rotation;

		rb.MoveRotation(rot);

		// move
		var pos = transform.position + transform.forward * speed;

		// always be slowing you, more slowing the harder you turn
		speed -= speed * (2.5f + 0.02f * Mathf.Abs( SteeredAngle)) * Time.deltaTime;

		rb.MovePosition( pos);

		CameraUpdater.MyUpdate();
	}
	
	void UpdateMeshesPositions()
	{
		for(int i = 0; i < 4; i++)
		{
			Quaternion quat;
//			Vector3 pos;
//			wheelColliders[i].GetWorldPose(out pos, out quat);
			
//			tireMeshes[i].position = pos;
//			tireMeshes[i].rotation = quat;
		}
	}
}
