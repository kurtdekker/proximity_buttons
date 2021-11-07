using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// from this forum post, just curious about this coyote jump stuff and acme anvils.
// https://forum.unity.com/threads/coyote-time-variable-height-jump-with-touch-controls.1192519/
//
// NOTE: you need to make and set what you consider "Ground layer"
//
// NOTE: you should probably turn down Default Contact Offset in Physics2D setup
//

public partial class DemoCoyoteTimeJumping : MonoBehaviour
{
	[Tooltip( "Lateral walk speed.")]
	public float LateralSpeed = 5.0f;
	[Tooltip( "Lateral walk acceleration.")]
	public float LateralAcceleration = 20.0f;

	[Tooltip( "How much vertical speed to add for each jump.")]
	public float JumpVerticalSpeed = 6.0f;

	[Tooltip( "How long after stepping off a ledge can you still jump?")]
	public float OffLedgeStillJumpTime = 0.25f;

	[Tooltip( "How many jumps can you do in total after being grounded?")]
	public int TotalJumpCount = 2;

	[Tooltip( "How early before ground contact can you say jump?")]
	public float PreLandingJumpTime = 0.15f;

	[Tooltip( "Set this to ONLY the layers you want for ground.")]
	public LayerMask GroundMask;

	[Header( "Onscreen debugging:")]
	public GameObject MarkerGroundedActual;
	public GameObject MarkerGroundedCoyote;
	public Text TextJumpsAvailable;

	float PlayerHeight = 1.0f;

	Rigidbody2D rb2d;

	float GroundedTimer;

	bool CombinedIsGrounded
	{
		get
		{
			return GroundedTimer > 0;
		}
	}

	int jumpCounter;

	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();

		jumpCounter = TotalJumpCount;		// use them up initially

		// these are mobile touch buttons; ignore if you don't want
		CreateProximityButtons();
		OrientationChangeSensor.Create( transform, CreateProximityButtons);
	}

	bool leftIntent, rightIntent;

	bool jumpIntent;				// actual intent
	float preJumpIntent;			// timer
	bool CombinedJumpIntent
	{
		get
		{
			return jumpIntent || (preJumpIntent > 0);
		}
	}

	void UpdateAdjustTimers()
	{
		if(GroundedTimer > 0)
		{
			GroundedTimer -= Time.deltaTime;
		}
		if (preJumpIntent > 0)
		{
			preJumpIntent -= Time.deltaTime;
		}
	}

	void UpdateGatherInputs()
	{
		// continuous intents
		leftIntent = false;
		rightIntent = false;

		if (Input.GetKey( KeyCode.LeftArrow)) leftIntent = true;
		if (Input.GetKey( KeyCode.RightArrow)) rightIntent = true;

		// eventful intents
		if (Input.GetKeyDown( KeyCode.Space))
		{
			jumpIntent = true;
			preJumpIntent = PreLandingJumpTime;
		}

		UpdateAddMobileTouchButtons();
	}

	bool ActualGrounded;

	void OnCollisionStay2D( Collision2D collision)
	{
		foreach( var contact in collision.contacts)
		{
			var point = contact.point;

			float dx = Mathf.Abs( point.x - transform.position.x);
			float dy = Mathf.Abs( point.y - (transform.position.y - PlayerHeight / 2));

			// contact pretty much has to be right below you
			if (dx < 0.1f && dy < 0.2f)
			{
				ActualGrounded = true;
			}
		}
	}

	void DoGroundChecks()
	{
		if (ActualGrounded)
		{
			if (GroundedTimer <= 0)
			{
				jumpCounter = 0;
			}

			GroundedTimer = OffLedgeStillJumpTime;
		}

		MarkerGroundedActual.SetActive( ActualGrounded);

		ActualGrounded = false;
	}

	void ProcessJumping()
	{
		var jumpString = "X";
		if (CombinedIsGrounded || (jumpCounter < TotalJumpCount))
		{
			jumpString = (TotalJumpCount - jumpCounter).ToString();
		}
		if (preJumpIntent > 0) jumpString = "P";
		TextJumpsAvailable.text = jumpString;

		MarkerGroundedCoyote.SetActive( CombinedIsGrounded);

		if (CombinedJumpIntent)
		{
			if (CombinedIsGrounded || (jumpCounter < TotalJumpCount))
			{
				preJumpIntent = 0;		// consume any pending
				GroundedTimer = 0;		// you can't be grounded; you jumped!

				jumpCounter++;			// consume a jump

				var vel = rb2d.velocity;

				// zero any falling velocity
				if (vel.y < 0) vel.y = 0;

				vel.y += JumpVerticalSpeed;

				rb2d.velocity = vel;
			}
		}

		// ALWAYS consumed
		jumpIntent = false;
	}

	void ProcessLeftRightMovement()
	{
		float desiredXMovement = 0;

		if (leftIntent) desiredXMovement = -1;
		if (rightIntent) desiredXMovement = 1;

		var desiredXSpeed = desiredXMovement * LateralSpeed;

		var vel = rb2d.velocity;

		vel.x = Mathf.MoveTowards( vel.x, desiredXSpeed, LateralAcceleration * Time.deltaTime);

		rb2d.velocity = vel;
	}

	void Update ()
	{
		UpdateAdjustTimers();

		UpdateGatherInputs();
	}

	void FixedUpdate()
	{
		DoGroundChecks();

		ProcessJumping();

		ProcessLeftRightMovement();
	}
}
