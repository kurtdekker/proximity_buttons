using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @kurtdekker - variable-height jumping
//
// This observes the jump key and will not jump until
// either of these conditions have been met:
//
//	a) the jump key is released
//	b) the maximum jump time has expired
//
// NOTE: for simplicity sake this does not try to
// lock out any double jumping. Jump one press at
// a time.
//
// If you want to start the jump immediately, then
// just make separate bookkeeping to:
//
//	- set an initial upward velocity upon key down
//	- keep track of where you started Y-wise
//	- compare where you are now when the REAL jump happens
//	- adjust the jump height velocity mid-air so that
//		the final jump height matches desired height.
//
// You would need to ensure that at your immediate
// upwards velocity, the player could never exceed
// the minimum jump height before the max time expired.

public class VariableJumpHeight : MonoBehaviour
{
	public KeyCode JumpKey = KeyCode.Space;

	[Header( "This curve controls ALL jump and timing dynamics.")]
	[Header( "Put min and max jump hold time on the X axis.")]
	[Header( "Put min and max jump height on the Y axis.")]
	public AnimationCurve HoldTimeToJumpHeight;

	// will be extracted from the above curve
	float MinJumpHold;
	float MaxJumpHold;

	// this records key state in the process of jumping
	bool workIntent;
	// this is how long we were holding
	float workTimer;

	// output: we have a finished intention to jump
	bool jumpIntent;
	// this high
	float jumpHeight;

	void ComputeMinMaxTimes()
	{
		// asuming keys are in time order... I think they are??
		var keys = HoldTimeToJumpHeight.keys;
		MinJumpHold = keys[0].time;
		MaxJumpHold = keys[keys.Length - 1].time;
	}

	void RecordJumpIntent()
	{
		Debug.Log( "Record Jump Intent: workTimer = " + workTimer);

		float guardedTime = workTimer;

		// guard timespans
		if (guardedTime < MinJumpHold)
		{
			guardedTime = MinJumpHold;
		}
		if (guardedTime >= MaxJumpHold)
		{
			guardedTime = MaxJumpHold;
		}

		// lookup height
		jumpHeight = HoldTimeToJumpHeight.Evaluate( guardedTime);

		// set jump intention
		jumpIntent = true;

		// clear work in progress (regardless of how we jumped)
		workIntent = false;
	}

	void UpdateReadJumping()
	{
		// you could do this ONLY in Start but then you cannot
		// really fiddle with the curve at runtime.
		ComputeMinMaxTimes();

		bool jumpIntentDown = Input.GetKeyDown( JumpKey);
		bool jumpIntentUp = Input.GetKeyUp( JumpKey);

		if (jumpIntentDown)
		{
			workIntent = true;
			workTimer = 0.0f;
		}
		if (workIntent)
		{
			if (jumpIntentUp)
			{
				RecordJumpIntent();
			}
		}

		if (workIntent)
		{
			if (workTimer >= MaxJumpHold)
			{
				RecordJumpIntent();
			}

			workTimer += Time.deltaTime;
		}
	}

	void FixedUpdateHandleJumping()
	{
		if (jumpIntent)
		{
			jumpIntent = false;

			float yVelocity = Mathf.Sqrt( Physics2D.gravity.magnitude * 2 * jumpHeight);

			var rb = GetComponent<Rigidbody2D>();
			Vector2 velocity = rb.velocity;
			velocity.y = yVelocity;
			rb.velocity = velocity;
		}
	}

	void Start()
	{
		ComputeMinMaxTimes();
	}

	void Update()
	{
		UpdateReadJumping();
	}

	void FixedUpdate()
	{
		FixedUpdateHandleJumping();
	}
}
