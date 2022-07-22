using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSteer2D : MonoBehaviour
{
	[Header( "Provide or we add.")]
	public Rigidbody2D rb2d;

	[Header( "Provide or we guess.")]
	public float RateOfTurning;
	public float Acceleration;
	public float Damping = 0.5f;
	public float MaxSpeed;

	public bool LinkSteeringToSpeed = true;

	float Speed;

	void Start ()
	{
		if (!rb2d)
		{
			rb2d = GetComponent<Rigidbody2D>();
			if (!rb2d)
			{
				rb2d = gameObject.AddComponent<Rigidbody2D>();

				rb2d.gravityScale = 0.0f;
			}
		}

		if (RateOfTurning <= 0)
		{
			RateOfTurning = 200;
		}

		if (Acceleration <= 0)
		{
			Acceleration = 10;
		}

		if (MaxSpeed <= 0)
		{
			MaxSpeed = 10;
		}
	}

	void FixedUpdate ()
	{
		float steer = -Input.GetAxisRaw( "Horizontal");
		float gas = Input.GetAxisRaw( "Vertical");

		Speed += Acceleration * gas * Time.deltaTime;

		Speed -= Damping * Speed * Time.deltaTime;

		if (Speed > +MaxSpeed)
		{
			Speed = +MaxSpeed;
		}
		if (Speed < -MaxSpeed)
		{
			Speed = -MaxSpeed;
		}

		if (LinkSteeringToSpeed)
		{
			steer *= Speed / MaxSpeed;
		}

		float angle = rb2d.rotation;

		angle += steer * RateOfTurning * Time.deltaTime;

		rb2d.MoveRotation( angle);

		Vector2 position = rb2d.position;

		position += (Vector2)rb2d.transform.up * Speed * Time.deltaTime;

		rb2d.MovePosition( position);		
	}
}
