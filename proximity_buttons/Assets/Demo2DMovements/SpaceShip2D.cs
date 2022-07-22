using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip2D : MonoBehaviour
{
	public float RateOfTurn;

	public float MaxSpeed;

	public float Acceleration;
	public float Damping;

	void Reset()
	{
		RateOfTurn = 180;
		MaxSpeed = 10;
		Acceleration = 10;
		Damping = 0.5f;
	}

	Rigidbody2D rb2d;

	void Start()
	{
		rb2d = gameObject.AddComponent<Rigidbody2D>();
		// set to zero if you want not gravity
		rb2d.gravityScale = 0.1f;
	}

	float inputSteer;
	bool inputThrust;

	void UpdateGatherInput()
	{
		inputSteer = 0;
		inputThrust = false;

		inputSteer += -Input.GetAxisRaw( "Horizontal");
		if (Mathf.Abs( Input.GetAxisRaw( "Vertical")) > 0.5f)
		{
			inputThrust = true;
		}

		// TODO: gather other input here

		// gate and normalize steering
		if (Mathf.Abs( inputSteer) < 0.3)
		{
			inputSteer = 0;
		}
		if (inputSteer != 0)
		{
			inputSteer = Mathf.Sign( inputSteer);
		}
	}

	void UpdateSteering()
	{
		if (inputSteer != 0)
		{
			float angle = rb2d.rotation;

			angle += inputSteer * RateOfTurn * Time.deltaTime;

			rb2d.MoveRotation( angle);
		}
	}

	void UpdateThrusting()
	{
		Vector2 forward = rb2d.transform.up;

		Vector2 velocity = rb2d.velocity;

		if (inputThrust)
		{
			velocity += (forward * Acceleration * Time.deltaTime);

			if (velocity.magnitude > MaxSpeed)
			{
				velocity = velocity.normalized * MaxSpeed;
			}
		}

		velocity -= velocity * Damping * Time.deltaTime;

		rb2d.velocity = velocity;
	}

	void FixedUpdate ()
	{
		UpdateGatherInput();

		UpdateSteering();

		UpdateThrusting();
	}
}
