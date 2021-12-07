using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// @kurtdekker
//
// demos simple touch-to-jump, with longer holds producing higher jumps
//
// can handle jump time lookup or jump frame count lookup.

public class DemoHoldForHigherJump : MonoBehaviour
{
	[Header( "FixedUpdate() frame count (first is not used)")]
	public float[] JumpFrameCountHeightTable;

	[Header( "TimeCount: time looked up to jump height:")]
	public AnimationCurve JumpTimeHeldHeightTable;

	// super-ultra-cheese hack to grab time of last frame (is there better way I am not aware of perhaps?)
	float FinalJumpHeldTime
	{
		get
		{
			var keys = JumpTimeHeldHeightTable.keys;
			if (keys.Length > 0)
			{
				return keys[ keys.Length - 1].time;
			}
			return 0.0f;		// failed to define any AnimationCurve keys!
		}
	}

	[Header( "Choose method above (frame count or hold time).")]
	public bool UseTimeHeldTable;

	[Header( "Lateral walk speeds / snappiness.")]
	public float LateralSpeed;
	public float LateralAcceleration;

	[Header( "Onscreen debugging:")]
	public GameObject MarkerGroundedActual;
	public GameObject MarkerJumpHeldDown;
	public Text MarkerLastJumpFrames;

	void Reset()
	{
		JumpFrameCountHeightTable = new float[] {
			0,			// not used!
			2.0f,
			3.0f,
			4.0f,
			5.0f,
			6.0f,
			7.0f,
			8.0f,
			9.0f,
			10.0f,
		};

		JumpTimeHeldHeightTable = new AnimationCurve(
			new Keyframe[] {
				new Keyframe( 0.00f, 2.00f),
				new Keyframe( 0.20f, 10.00f),
			}
		);

		LateralSpeed = 10.0f;
		LateralAcceleration = 100.0f;

		// auto-hookup some debugging
		// remember the first rule of GameObject.Find(): DO NOT USE GameObject.Find()!!!
		MarkerGroundedActual = GameObject.Find("TextMarkerGroundedActual");
		MarkerJumpHeldDown = GameObject.Find( "TextMarkerJumpButton");
		var go = GameObject.Find( "TextMarkerLastJumpFrameCount");
		MarkerLastJumpFrames = go.GetComponent<Text>();
	}

	Rigidbody2D rb2d;
	float VerticalGravity { get { return Mathf.Abs( Physics2D.gravity.y); } }

	void Start ()
	{
		rb2d = GetComponent<Rigidbody2D>();	
	}

	bool isGrounded;

	// user intent
	bool jumpButtonIsDown;

	void GatherJumpIntent()
	{
		jumpButtonIsDown = Input.GetButton( "Jump");

		if (jumpButtonIsDown)
		{
			jumpButtonHeldTimer += Time.deltaTime;
		}

		MarkerJumpHeldDown.SetActive( jumpButtonIsDown);
	}

	void UpdateDebugIndicators()
	{
		MarkerGroundedActual.SetActive( isGrounded);
	}

	void Update()
	{
		GatherJumpIntent();

		UpdateDebugIndicators();
	}

	// counted in FixedUpdate() to be consistently spaced
	int jumpFrameCounter;
	// counted in Update() to be precise time
	float jumpButtonHeldTimer;

	// actually does the jump, calculating height based on how we asked to
	void ExecuteJump()
	{
		float jumpHeight = 0.0f;

		if (UseTimeHeldTable)
		{
			jumpHeight = JumpTimeHeldHeightTable.Evaluate( jumpButtonHeldTimer);

			MarkerLastJumpFrames.text = System.String.Format(
				"Last Jump:\n{0:0.000}s\nheight: {1:0.0}m",
				jumpButtonHeldTimer,
				jumpHeight);
		}
		else
		{
			if (jumpFrameCounter < 0) jumpFrameCounter = 0;
			if (jumpFrameCounter > JumpFrameCountHeightTable.Length - 1)
			{
				jumpFrameCounter = JumpFrameCountHeightTable.Length - 1;
			}
			// look up frames held to find desired jump height
			jumpHeight = JumpFrameCountHeightTable[jumpFrameCounter];

			MarkerLastJumpFrames.text = System.String.Format(
				"Last Jump:\n{0} Frames\nheight: {1:0.0}m",
				jumpFrameCounter,
				jumpHeight);
		}

		// compute launch velocity for desired height
		float upwardSpeed = Mathf.Sqrt( VerticalGravity * jumpHeight * 2);

		// up we go
		rb2d.velocity = new Vector2( rb2d.velocity.x, upwardSpeed);

		isGrounded = false;

		// consume the jump
		jumpFrameCounter = 0;
		jumpButtonHeldTimer = 0;
	}

	void FixedUpdate ()
	{
		// lateral movement always, in air or on ground
		{
			float xm = 0;

			xm = Input.GetAxis( "Horizontal") * LateralSpeed;

			var velocity = rb2d.velocity;

			velocity.x = Mathf.MoveTowards( velocity.x, xm, LateralAcceleration * Time.deltaTime);

			rb2d.velocity = velocity;
		}

		// disregard all jump input except when grounded
		if (isGrounded)
		{
			if (jumpButtonIsDown)
			{
				jumpFrameCounter++;

				if (UseTimeHeldTable)
				{
					// did you hold it enough time?
					if (jumpButtonHeldTimer >= FinalJumpHeldTime)
					{
						ExecuteJump();
					}
				}
				else
				{
					// did you hold it enough frames?
					if (jumpFrameCounter >= JumpFrameCountHeightTable.Length - 1)
					{
						ExecuteJump();
					}
				}
			}
			if (!jumpButtonIsDown)
			{
				if (jumpFrameCounter > 0)
				{
					ExecuteJump();
				}
			}
		}
		else
		{
			jumpFrameCounter = 0;
			jumpButtonHeldTimer = 0;
		}
	}

	void OnCollisionEnter2D()
	{
		isGrounded = true;
	}

	void OnCollisionStay2D()
	{
		isGrounded = true;
	}

	void OnCollisionExit2D()
	{
		isGrounded = false;
	}
}
