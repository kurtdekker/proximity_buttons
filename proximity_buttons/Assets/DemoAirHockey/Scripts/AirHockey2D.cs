namespace AirHockey
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	// @kurtdekker - air hockey controller: runs most of everything.
	//
	// Setup: see scene for everything, but essentially
	//	- put this script on an empty GameObject - it runs BOTH players
	//	- make 2 paddle sprites with collider - script will add Rigidbody
	//		- hard-wired to 2 players now
	//		- drag players into script
	//	- make ball sprite with collider AND with Rigidbody
	//		- add gravityscale (or not) to the ball
	//		- drag ball Rigidbody into script
	//	- drag various PhysicMaterials into script
	//	- set minimum velocity
	//	- make a single goal wall - we'll clone it
	//		- drag it into this script

	// Operation:
	//	- touchable area is something less than half of the left/right sides
	//	- touching sets desired screen position
	//	- paddle always moves towards desired position at max velocity
	//	- move the paddle always with MovePosition()
	//	- Rigibody collision must be continuous
	//

	public partial class AirHockey2D : MonoBehaviour
	{
		[Header("Fill this with your Collider2Ds (NO Rigidbody2D)")]
		public Collider2D[] Paddles;

		[Header("Make yourself a ball (add Rigidbody2D!)")]
		public Rigidbody2D TheBall;

		[Header("The slowest a ball can go in an axis.")]
		public float MinimumAxisMotion = 5.0f;
		public float MaximumTotalMotion = 50.0f;

		[Header("Provide!")]
		public PhysicsMaterial2D BallMat;
		public PhysicsMaterial2D PaddleMat;
		public PhysicsMaterial2D WallMat;

		[Header("Make one, we'll clone it and move it.")]
		public GoalAndSidewallDriver GoalWallExample;

		// just to keep it all together...
		public class Player
		{
			public Rigidbody2D PaddleRB;

			public Rect TouchArea;

			public Vector2 CurrentScreenPosition;
			public Vector2 DesiredScreenPosition;

			public int Score;
		}

		Camera cam;

		Player[] Players;

		bool leftRightService;

		const int NUMPLAYERS = 2;

		// percent of screen width for each player's finger
		const float FingerFraction = 0.45f;

		// arbitrarily chosen maximum speed; should be in screen pixel units
		float MaxPlayerSpeed { get { return Screen.width * 4; } }

		// never returns zero
		float MySign( float f)
		{
			if (f < 0) return -1;
			return +1;
		}

		void Start()
		{
			if (Paddles.Length != NUMPLAYERS)
			{
				Debug.LogError("Script hard-wired to exactly 2 paddles - you must provide colliders!");
				Debug.Break();
				return;
			}

			cam = Camera.main;
			cam.orthographic = true;	// mandatory for the cheesy way we're projecting!!

			Players = new Player[NUMPLAYERS];

			for (int i = 0; i < NUMPLAYERS; i++)
			{
				Collider2D paddleCollider = Paddles[i];

				paddleCollider.sharedMaterial = PaddleMat;

				Player player = new Player();

				Rect r = new Rect();

				switch (i)
				{
				default:
				case 0:
					// left side
					r.x = 0;
					r.y = 0;
					r.width = Screen.width * FingerFraction;
					r.height = Screen.height;
					break;
				case 1:
					// right side
					r.x = 0;
					r.y = 0;
					r.width = Screen.width * FingerFraction;
					r.height = Screen.height;
					r.x = Screen.width - r.width;
					break;
				}

				player.TouchArea = r;

				Rigidbody2D rb = paddleCollider.gameObject.AddComponent<Rigidbody2D>();
				rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
				rb.gravityScale = 0.0f;
				rb.freezeRotation = true;

				rb.sharedMaterial = PaddleMat;

				// presume to start you at the center
				Vector2 startPosition = r.center;
				player.CurrentScreenPosition = startPosition;
				player.DesiredScreenPosition = startPosition;

				player.PaddleRB = rb;

				Players[i] = player;
			}

			UpdateMovePlayers( teleport: true);

			{
				leftRightService = Random.value < 0.5f;

				TheBall.gameObject.AddComponent<IdentifyBall>();

				ServeTheBall();
			}

			FabricateWorldBoundaryColliders();
			OrientationChangeSensor.Create(transform, FabricateWorldBoundaryColliders);

			UI_DrivePlayerScores();
		}

		void ServeTheBall()
		{
			TheBall.position = Vector3.zero + Vector3.up * Random.Range( -4.0f, +4.0f);

			leftRightService = !leftRightService;

			TheBall.angularVelocity = 0;

			Vector2 ballDirection = Random.insideUnitCircle.normalized;

			ballDirection.x *= 5;

			ballDirection.x = Mathf.Abs( ballDirection.x);

			if (leftRightService)
			{
				ballDirection.x *= -1;
			}

			TheBall.velocity = ballDirection;

			TheBall.sharedMaterial = BallMat;
		}

		GameObject boundaryColliders;
		void FabricateWorldBoundaryColliders()
		{
			if (boundaryColliders) Destroy(boundaryColliders);

			boundaryColliders = new GameObject("BoundaryColliders");

			Vector2 lowerLeft = cam.ScreenToWorldPoint(new Vector2(0, 0));
			Vector2 upperRight = cam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

			Vector2 center = (lowerLeft + upperRight) / 2;

			float width = upperRight.x - lowerLeft.x;
			float height = upperRight.y - lowerLeft.y;

			// world units
			float thickness = 5.0f;

			var top = boundaryColliders.AddComponent<BoxCollider2D>();
			top.offset = new Vector2(0, +(height + thickness) / 2);
			top.size = new Vector2(width + thickness * 2, thickness);
			top.sharedMaterial = WallMat;

			var bottom = boundaryColliders.AddComponent<BoxCollider2D>();
			bottom.offset = new Vector2(0, -(height + thickness) / 2);
			bottom.size = new Vector2(width + thickness * 2, thickness);
			bottom.sharedMaterial = WallMat;

			// TODO: make these side colliders be two colliders with a gap for the goal!!

			var left = boundaryColliders.AddComponent<BoxCollider2D>();
			left.offset = new Vector2(-(width + thickness) / 2, 0);
			left.size = new Vector2(thickness, height + thickness * 2);
			left.sharedMaterial = WallMat;

			var right = boundaryColliders.AddComponent<BoxCollider2D>();
			right.offset = new Vector2(+(width + thickness) / 2, 0);
			right.size = new Vector2(thickness, height + thickness * 2);
			right.sharedMaterial = WallMat;

			GoalWallExample.gameObject.SetActive(false);

			var leftWall = Instantiate<GoalAndSidewallDriver>(GoalWallExample, boundaryColliders.transform);
			var rightWall = Instantiate<GoalAndSidewallDriver>(GoalWallExample, boundaryColliders.transform);

			leftWall.transform.position = new Vector2(lowerLeft.x + GoalWallExample.NotionalWallThickness / 2, center.y);
			rightWall.transform.position = new Vector2(upperRight.x - GoalWallExample.NotionalWallThickness / 2, center.y);

			leftWall.gameObject.SetActive(true);
			rightWall.gameObject.SetActive(true);

			// attach conditional sensors to thegoals
			Trigger2DForwarder.Attach(
				leftWall.GoalBox.gameObject,
				TestIfBall,
				() => { PlayerWins( 1); });
			Trigger2DForwarder.Attach(
				rightWall.GoalBox.gameObject,
				TestIfBall,
				() => { PlayerWins( 0); });
		}

		bool TestIfBall( GameObject go)
		{
			if (go.GetComponent<IdentifyBall>()) return true;
			return false;
		}

		void PlayerWins( int who)
		{
			Debug.Log( "Winner was player index " + who);

			int score = Players[who].Score;

			score++;

			Players[who].Score = score;

			UI_DrivePlayerScores();

			Time.timeScale = 0.0f;

			StartCoroutine( StartNextRound());
		}

		IEnumerator StartNextRound()
		{
			yield return new WaitForSecondsRealtime( 2.0f);

			ServeTheBall();

			Time.timeScale = 1.0f;

			yield return new WaitForFixedUpdate();

			Time.timeScale = 0.0f;

			yield return new WaitForSecondsRealtime( 0.2f);

			Time.timeScale = 1.0f;
		}

		void UpdateGatherInputTouches()
		{
			var mts = MicroTouch.GatherMicroTouches();

			// check each touch to see whose rect it belongs to
			foreach (var mt in mts)
			{
				var fingerPosition = mt.position;

				for (int i = 0; i < NUMPLAYERS; i++)
				{
					Player player = Players[i];

					// positional updates all happen in screenspace
					if (player.TouchArea.Contains(fingerPosition))
					{
						player.DesiredScreenPosition = fingerPosition;
					}
				}
			}
		}

		void UpdateMovePlayers( bool teleport)
		{
			// the most a paddle can move per frame
			float stepDistance = MaxPlayerSpeed * Time.deltaTime;

			// project all players screen spaces into the world
			for (int i = 0; i < NUMPLAYERS; i++)
			{
				Player player = Players[i];

				Rigidbody2D rb2d = player.PaddleRB;

				if (teleport)
				{
					player.CurrentScreenPosition = player.DesiredScreenPosition;
				}

				player.CurrentScreenPosition = Vector2.MoveTowards(
					player.CurrentScreenPosition,
					player.DesiredScreenPosition,
					stepDistance);

				Vector2 worldPosition = cam.ScreenToWorldPoint(player.CurrentScreenPosition);

				if (teleport)
				{
					rb2d.position = worldPosition;
				}

				rb2d.MovePosition(worldPosition);
			}
		}

		void EnforceBallSpeeds()
		{
			Vector2 velocity = TheBall.velocity;

			velocity = Vector2.ClampMagnitude(velocity, MaximumTotalMotion);

			if (Mathf.Abs(velocity.x) < MinimumAxisMotion)
			{
				velocity.x = MySign(velocity.x) * MinimumAxisMotion;
			}

			// NOTE: if you enable gravity, you probably want to remove this code
			// because it will prevent the ball from falling if it goes upwards.
			if (Mathf.Abs(velocity.y) < MinimumAxisMotion)
			{
				velocity.y = MySign(velocity.y) * MinimumAxisMotion;
			}

			TheBall.velocity = velocity;
		}

		void FixedUpdate()
		{
			EnforceBallSpeeds();

			UpdateGatherInputTouches();

			UpdateMovePlayers(false);
		}
	}
}
