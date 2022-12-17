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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// put this on the camera

public class CamFollowRelaxed : MonoBehaviour, IPushable
{
	public Transform Target;

	public float GazeHeight;

	public float DesiredDistance;
	public float DesiredHeight;

	const float SlopDistance = 1.0f;
	const float MoveSnappiness = 2.0f;

	private void Reset()
	{
		GazeHeight = 2.0f;
		DesiredDistance = 10.0f;
		DesiredHeight = 10.0f;
	}

	Vector3 AccumulatedPosition;

	void Start ()
	{
		AccumulatedPosition = Target.position +
				new Vector3(0, DesiredHeight, DesiredDistance);

		RegardTarget();
	}

	void RegardTarget()
	{
		transform.LookAt(Target.position + Vector3.up * GazeHeight);
	}

	void LateUpdate ()
	{
		Vector3 delta = Target.position - transform.position;

		delta.y = 0;			// flatten

		float distance = delta.magnitude;

		// by taking our current position instead of the AccumulatedPosition,
		// it lets external forces push us around, such as the camera control does.
		Vector3 desiredPosition = AccumulatedPosition;

		// are we too far?
		if (distance > DesiredDistance + SlopDistance)
		{
			// how badly are we too far?
			float amount = distance - (DesiredDistance + SlopDistance);

			desiredPosition += delta.normalized * amount;
		}
		// are we too close?
		if (distance < DesiredDistance - SlopDistance)
		{
			// how badly are we too close?
			float amount = (DesiredDistance - SlopDistance) - distance;

			desiredPosition -= delta.normalized * amount;
		}

		AccumulatedPosition = Vector3.Lerp(AccumulatedPosition, desiredPosition, MoveSnappiness * Time.deltaTime);

		transform.position = AccumulatedPosition;

		RegardTarget();
	}

	public void Push(Vector3 direction)
	{
		AccumulatedPosition += direction;
	}
}
