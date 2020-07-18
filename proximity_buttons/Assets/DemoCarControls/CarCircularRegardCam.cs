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
using System.Collections;

// This variant of camera control does not try to be behind the
// target until the target tries to go away, to allow you to do
// fun local movement without a lot of camera shifting.
//
// It does this by maintaining a radius from the car between
// the min and max values, and regarding the car always.
//
public class CarCircularRegardCam : MonoBehaviour, IMyUpdateable
{
	public Transform target;

	public float MinimumRadius;
	public float MaximumRadius;
	public float DistanceAbove;

	Vector3 lookpoint;
	Vector3 bepoint;

	const float snapBe = 5.0f;
	const float snapLook = 10.0f;

	void Reset()
	{
		MinimumRadius = 3.0f;
		MaximumRadius = 7.0f;
		DistanceAbove = 5.0f;
	}

	public void MyUpdate ()
	{
		// presume camera does not have to move
		Vector3 newBe = transform.position;
		newBe.y = target.position.y + DistanceAbove;

		Vector3 flatDelta = target.position - transform.position;
		flatDelta.y = 0;

		float distance = flatDelta.magnitude;

		float snapBeToUse = snapBe;

		// cam is too far, try to be behind the car
		if (distance > MaximumRadius)
		{
			float overage = distance - MaximumRadius;
			snapBeToUse += 10 * overage;

			newBe = target.position + target.forward * -MaximumRadius + Vector3.up * DistanceAbove;
		}

		// cam is too close, try to get away from the car
		if (distance < MinimumRadius)
		{
			newBe = target.position - flatDelta.normalized * MinimumRadius + Vector3.up * DistanceAbove;
		}

		// where the camera should BE
		newBe = Vector3.Lerp( transform.position, newBe, snapBeToUse * Time.deltaTime);

		// where the camera should look
		Vector3 newLook = target.position;

		bepoint = Vector3.Lerp( bepoint, newBe, snapBe * Time.deltaTime);
		lookpoint = Vector3.Lerp( lookpoint, newLook, snapLook * Time.deltaTime);

		transform.position = bepoint;
		transform.LookAt (lookpoint);
	}
}
