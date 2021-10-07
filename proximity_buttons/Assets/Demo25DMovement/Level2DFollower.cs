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

// @kurtdekker
// Simple curving 2.5D world line follower
// If you want, tack on jump stuff in a logic layer above this one

public class Level2DFollower : MonoBehaviour
{
	// the only part of this that looks left / right
	public Transform PivotingVisuals;

	float LeftRightMovementCommanded;

	const float NominalSpeed = 5.0f;

	// how quickly we turn to follow the track, which knocks onto
	// how quickly the camera tracks us from our side
	const float TrackTurnSnappiness = 5.0f;

	// how quickly we follow the terrain vertically
	const float VerticalSnappiness = 10.0f;

	// where we are linearly along the world (sort of your "X")
	float along;

	ILevel25DPath path;

	// which way left/right is our visuals facing?
	float VisualsFacing;
	// which way do they WANT to be facing?
	float desiredVisualsFacing = AngleToCam;
	// how quickly we whip our visual geometry left/right
	const float VisualsTurnSnappiness = 10.0f;

	// player stands just slightly facing cam
	const float AngleToCam = 10;

	void Start()
	{
		path = InterfaceHelpers.FindInterfaceOfType<ILevel25DPath>();

		along = path.GetDistanceAlongPath( transform.position);
	}

	bool HaveRun;

	void Update()
	{
		UpdateGatherInput();
		UpdateVisualsFacing();
		UpdateFollowCurve();
		HaveRun = true;
	}

	void UpdateGatherInput()
	{
		LeftRightMovementCommanded = Input.GetAxisRaw( "Horizontal") * NominalSpeed;
	}

	void UpdateVisualsFacing()
	{
		if (LeftRightMovementCommanded < -0.1f)
		{
			desiredVisualsFacing = 180 - AngleToCam;
		}
		if (LeftRightMovementCommanded > 0.1f)
		{
			desiredVisualsFacing = AngleToCam;
		}

		VisualsFacing = Mathf.LerpAngle( VisualsFacing, desiredVisualsFacing, VisualsTurnSnappiness * Time.deltaTime);

		PivotingVisuals.localRotation = Quaternion.Euler( 0, VisualsFacing, 0);
	}

	void UpdateFollowCurve()
	{
		float movement = LeftRightMovementCommanded * Time.deltaTime;

		along += movement;

		if (along < 0)
		{
			along = 0;
		}
		if (along > path.GetTotalLength())
		{
			along = path.GetTotalLength();
		}

		Vector3 worldPosition = Vector3.zero;
		Vector3 alongVector = Vector3.zero;

		path.GetCurveInfo( along, out worldPosition, out alongVector);

		// ultra cheese: relies on no collider on player we might accidentally hit
		// TODO: use layers like a real program might
		RaycastHit rch;
		Ray ray = new Ray(origin: worldPosition + Vector3.up * 2, direction: Vector3.down);
		if (Physics.Raycast( ray, out rch, 4))
		{
			Vector3 currPosition = transform.position;

			// copy X/Z straight through
			currPosition.x = worldPosition.x;
			currPosition.z = worldPosition.z;

			float desiredY = rch.point.y;

			// smoothly move Y to follow ground contour
			currPosition.y = Mathf.Lerp( currPosition.y, desiredY, VerticalSnappiness * Time.deltaTime);

			if (!HaveRun)
			{
				currPosition.y = desiredY;
			}

			transform.position = currPosition;
		}
		else
		{
			transform.position = worldPosition;
		}

		transform.forward = Vector3.Lerp( transform.forward, alongVector, TrackTurnSnappiness * Time.deltaTime);

		if (!HaveRun)
		{
			transform.forward = alongVector;
		}
	}
}
