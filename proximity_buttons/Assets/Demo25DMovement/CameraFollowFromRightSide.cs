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
// follows an arbitrary object at 90 degrees to its right
// always regards the object's look point
// has some lerping smoothness to its positioning
// replace with Cinemachine if you want more features already!

public class CameraFollowFromRightSide : MonoBehaviour
{
	public Transform PositionTarget;
	public Transform LookTarget;

	public float Distance;
	public float Above;

	// how snappy the camera's location moves
	public float MoveSnappiness;

	// how snappy the camera's gaze moves
	public float LookSnappiness;

	Vector3 LookPosition;

	void Reset()
	{
		Distance = 10.0f;
		Above = 4.0f;
		MoveSnappiness = 3.0f;
		LookSnappiness = 2.0f;
	}

	bool HaveRun;

	void LateUpdate ()
	{
		if (PositionTarget)
		{
			Vector3 right = PositionTarget.right;

			Vector3 position = PositionTarget.position + right * Distance + Vector3.up * Above;

			transform.position = Vector3.Lerp( transform.position, position, MoveSnappiness * Time.deltaTime);

			LookPosition = Vector3.Lerp( LookPosition, LookTarget.position, LookSnappiness * Time.deltaTime);

			if (!HaveRun)
			{
				transform.position = position;
				LookPosition = LookTarget.position;
			}

			transform.LookAt( LookPosition);
		}

		HaveRun = true;
	}
}
