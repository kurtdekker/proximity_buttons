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

// Put this script directly on the player ship.

public class DemoButtonsForLeftRight : MonoBehaviour
{
	[Header("World units per second:")]
	public float MovementSpeed;

	[Header("Optional boundaries:")]
	public Transform LeftMovementLimit;
	public Transform RightMovementLimit;

	void Reset()
	{
		MovementSpeed = 5.0f;
	}

	ProximityButtonSet pbsMove;

	ProximityButtonSet.ProximityButton pbLeft, pbRight;

	void CreateControls()
	{
		if (pbsMove) Destroy(pbsMove);

		float sz = Mathf.Min(Screen.width, Screen.height) * 0.35f;
		pbsMove = ProximityButtonSet.Create(sz);
		pbLeft = pbsMove.AddButton("left", MR.SR(0.1f, 0.9f, 0, 0).center);
		pbRight = pbsMove.AddButton("right", MR.SR(0.3f, 0.9f, 0, 0).center);
	}

	void Start ()
	{
		CreateControls();

		OrientationChangeSensor.Create( transform, CreateControls);
	}

	void Update ()
	{
		// assume you're not moving
		Vector3 movement = Vector3.zero;

		// read the proximity buttons, left and right
		if (pbLeft.fingerDown)
		{
			movement.x = -1;
		}
		if (pbRight.fingerDown)
		{
			movement.x = 1;
		}

		// scale any commanded movement up appropriately
		movement *= MovementSpeed;

		// where do we go next this frame?
		var position = transform.position + movement * Time.deltaTime;

		// did you ask for left/right limiting?
		if (LeftMovementLimit)
		{
			if (position.x < LeftMovementLimit.position.x)
			{
				position.x = LeftMovementLimit.position.x;
			}
		}
		if (RightMovementLimit)
		{
			if (position.x > RightMovementLimit.position.x)
			{
				position.x = RightMovementLimit.position.x;
			}
		}

		// make it so Captain!
		transform.position = position;
	}
}
