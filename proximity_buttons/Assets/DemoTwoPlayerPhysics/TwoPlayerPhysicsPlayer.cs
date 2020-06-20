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

public class TwoPlayerPhysicsPlayer : MonoBehaviour
{
	public float BaseMoveSpeed;

	[Tooltip( "Just 0 or 1 for now...")]
	public int PlayerIndex;

	public	GrippingPlayerControlSet	controls;

	void Reset()
	{
		BaseMoveSpeed = 5.0f;
	}

	Rigidbody rb;
	VAButton vabMove;

	void CreateVABs()
	{
		if (vabMove) Destroy( vabMove);

		float sz = MR.MINAXIS * 0.67f;

		vabMove = gameObject.AddComponent<VAButton>();
		// default left side
		vabMove.r_downable = new Rect( 0, Screen.height - sz, sz, sz);
		if (PlayerIndex == 1)
		{
			// move to right side
			Rect r = vabMove.r_downable;
			r.x = Screen.width - r.width;
			vabMove.r_downable = r;
		}
		vabMove.doClamp = true;
		vabMove.doNormalize = false;
		vabMove.label = "MOVE " + PlayerIndex.ToString();
	}

	void Start ()
	{
		rb = GetComponent<Rigidbody>();

		// enforce only 0 or 1 player index
		PlayerIndex &= 1;

		CreateVABs();

		OrientationChangeSensor.Create( transform, () => { CreateVABs(); });

		controls.SetActionButtonActive(false);
		controls.SetCallback( OnButtonAction);
	}

	Vector3 LastPlayerMotion = Vector3.zero;

	void UpdateMoving()
	{
		if (bMove)
		{
			float currentSpeed = BaseMoveSpeed + DSM.Wave.iValue;

			LastPlayerMotion = new Vector3(
				v3Move.x,
				0,
				v3Move.y) * currentSpeed;

			rb.MovePosition( transform.position + LastPlayerMotion * Time.deltaTime);

			float angle = Mathf.Rad2Deg * Mathf.Atan2( LastPlayerMotion.x, LastPlayerMotion.z);
			transform.rotation = Quaternion.Euler( 0, angle, 0);
		}
	}

	// fraction of player motion to add to bullets
	const float PlayerMotionFraction = 0.5f;

	bool bMove;
	Vector3 v3Move;

	void UpdateGatherTouchInput()
	{
		bMove = vabMove.fingerDown;
		v3Move = vabMove.outputRaw;
	}

	void UpdateGatherKeyboardInput()
	{
		switch( PlayerIndex)
		{
		default :
			Debug.LogWarning( GetType() + "UpdateGatherKeyboardInput(): PlayerIndex must be 0 or 1!");
			break;

		case 0 :
			// move
			if (Input.GetKey(KeyCode.A))
			{
				bMove = true;
				v3Move.x = -1;
			}
			if (Input.GetKey(KeyCode.D))
			{
				bMove = true;
				v3Move.x =  1;
			}
			if (Input.GetKey(KeyCode.W))
			{
				bMove = true;
				v3Move.y = 1;
			}
			if (Input.GetKey(KeyCode.S))
			{
				bMove = true;
				v3Move.y = -1;
			}
			break;

		case 1 :
			// move
			if (Input.GetKey(KeyCode.LeftArrow))
			{
				bMove = true;
				v3Move.x = -1;
			}
			if (Input.GetKey(KeyCode.RightArrow))
			{
				bMove = true;
				v3Move.x =  1;
			}
			if (Input.GetKey(KeyCode.UpArrow))
			{
				bMove = true;
				v3Move.y = 1;
			}
			if (Input.GetKey(KeyCode.DownArrow))
			{
				bMove = true;
				v3Move.y = -1;
			}
			break;
		}
	}

	IGrippable GrippableObject;		// what you might be able to grab
	IGrippable GrippedObject;		// what you have grabbed onto

	void OnButtonAction()
	{
		// this must be a "drop"
		if (GrippedObject != null)
		{
			var hj = gameObject.AddComponent<HingeJoint>();
			if (hj)
			{
				Destroy(hj);
			}

			GrippedObject = null;

			return;
		}

		// this must be a "lift"
		if (GrippedObject == null)
		{
			if (GrippableObject != null)
			{
				GrippedObject = GrippableObject;

				var hj = gameObject.AddComponent<HingeJoint>();
				hj.connectedBody = GrippedObject.GetRigidbody();
			}

			return;
		}
	}

	void UpdateGripping()
	{
		if (GrippedObject == null)
		{
			GrippableObject = GrippableManager.Instance.GetNearestGrippable(transform);

			if (GrippableObject != null)
			{
				controls.SetActionButtonActive(true);
				controls.SetText( "LIFT");
			}
			else
			{
				controls.SetActionButtonActive(false);
			}

			return;
		}

		if (GrippedObject != null)
		{
			controls.SetActionButtonActive(true);
			controls.SetText( "DROP");

			return;
		}
	}

	void Update()
	{
		UpdateGatherTouchInput();

		UpdateGatherKeyboardInput();

		UpdateMoving();

		UpdateGripping();
	}
}
