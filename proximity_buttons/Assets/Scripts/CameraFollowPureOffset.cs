/*
	The following license supersedes all notices in the source code.

	Copyright (c) 2022 Kurt Dekker/PLBM Games All rights reserved.

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
// Put this on the camera, give it something to follow
// Does NOT affect the gaze angle of the camera. You set that.
// go get Cinemachine if you want more features!

public class CameraFollowPureOffset : MonoBehaviour
{
	public Transform Target;

	public Vector3 Offset;

	public float Snappiness;

	void Reset()
	{
		Offset = new Vector3( 0, 0, -10);
		Snappiness = 8.0f;
	}

	Vector3 positionAccumulator;

	void UpdateCameraPosition( bool instantaneous = false)
	{
		if (Target)
		{
			var proposedPosition = Target.position + Offset;

			positionAccumulator = Vector3.Lerp( positionAccumulator, proposedPosition, Snappiness * Time.deltaTime);

			if (instantaneous)
			{
				positionAccumulator = proposedPosition;
			}

			transform.position = positionAccumulator;
		}
	}

	void Start()
	{
		UpdateCameraPosition(true);
	}

	void LateUpdate ()
	{
		UpdateCameraPosition();
	}
}
