using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @kurtdekker - simple 2D ladder attach and climb code

public class PlayerLadderable : MonoBehaviour
{
	// the current ladder we are on, if any
	Ladder2D ladder;
	Rigidbody2D rb;

	// just some random constants to control the physics
	const float LadderClimbSpeed = 2.0f;
	const float LadderDescentSpeed = 3.0f;
	const float LateralSpeed = 3.0f;

	const float Gravity = -6.0f;

	const float JumpVelocity = 4.0f;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		rb.gravityScale = 0.0f;		// we'll take care of gravity
	}

	// ladders call here to tell us "you're riding on us now"
	// best never to have ladders close enough that a player
	// can't at least exist one frame OUTSIDE all ladders.
	// no idea how that logic might get tangled up!
	public void SetLadder( Ladder2D currentLadder)
	{
		ladder = currentLadder;
	}

	bool grounded;
	float yVelocity;

	void OnCollisionStay2D()
	{
		if (yVelocity < 0)
		{
			yVelocity = -0.5f;
		}
		grounded = true;
	}

	bool jumpIntent;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) ||
			Input.GetKeyDown(KeyCode.LeftControl))
		{
			jumpIntent = true;
		}
	}

	void FixedUpdate ()
	{
		// what the user intends
		float xintent = Input.GetAxisRaw( "Horizontal");
		float yintent = Input.GetAxisRaw( "Vertical");

		// clamp near zero
		if (Mathf.Abs( xintent) < 0.1f) xintent = 0;
		if (Mathf.Abs( yintent) < 0.1f) yintent = 0;

		var oldPosition = rb.position;

		var newPosition = oldPosition;

		bool freeFalling = true;
		bool mayMoveLaterally = true;

		// are we on a ladder?
		if (ladder)
		{
			// we can't fall off a ladder, we are an OSHA dream worker
			freeFalling = false;
			// and we stop all vertical ballistic motion
			yVelocity = 0.0f;

			// when on ladder, up / down takes priority and quelches lateral motion
			if (yintent != 0)
			{
				// decide how fast we can go up or down
				var ySpeed = LadderClimbSpeed;
				if (yintent < 0) ySpeed = LadderDescentSpeed;

				// scale for time (well, technically fixedTime since we're in FixedUpdate)
				ySpeed *= Time.deltaTime;

				// move us vertically
				newPosition.y += yintent * ySpeed;

				// when going vertically, move towards center of ladder
				newPosition.x = Mathf.MoveTowards(
					newPosition.x,
					ladder.transform.position.x,
					Mathf.Abs(yintent * ySpeed));

				// can only move laterally on a ladder when you are not
				// actively trying to go up/down
				mayMoveLaterally = false;
			}
		}

		if (mayMoveLaterally)
		{
			// not on ladder, we can only go left/right
			newPosition.x += xintent * LateralSpeed * Time.deltaTime;
		}

		// process jumping
		if (jumpIntent)
		{
			jumpIntent = false;			// consume
			if (grounded)
			{
				yVelocity = JumpVelocity;
			}
		}

		if (freeFalling)
		{
			// and gravity rules at the end of the day
			yVelocity += Gravity * Time.deltaTime;

			newPosition.y += yVelocity * Time.deltaTime;
		}

		// update position
		rb.MovePosition( newPosition);

		// clear groundedness
		grounded = false;
	}
}
