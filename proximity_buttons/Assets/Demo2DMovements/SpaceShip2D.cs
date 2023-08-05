﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @kurtdekker - ultra simple 2D space flight controls with audio and particles

public class SpaceShip2D : MonoBehaviour
{
	public float RateOfTurn;

	public float MaxSpeed;

	public float Acceleration;
	public float Damping;

	[Header( "Engine audio loop.")]
	public AudioSource EngineAudioLoop;

	float originalAudioVolume;
	float originalAudioPitch;

	[Header( "Thruster particles.")]
	public ParticleSystem EngineParticleSystem;

	float originalParticleEmissionRate;

	const float engineSnappiness = 2.0f;
	const float engineNullingTerm = 1.0f;
	float currentEngineLevel;
	float desiredEngineLevel;

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

		rb2d.sleepMode = RigidbodySleepMode2D.NeverSleep;

		if (EngineAudioLoop)
		{
			originalAudioVolume = EngineAudioLoop.volume;
			originalAudioPitch = EngineAudioLoop.pitch;
		}

		if (EngineParticleSystem)
		{
			originalParticleEmissionRate = EngineParticleSystem.emission.rateOverTimeMultiplier;
		}

		currentEngineLevel = 0;
		desiredEngineLevel = 0;

		UpdateEngineAudio();
	}

	// inputs
	float inputSteer;
	bool inputThrust;
	bool inputCenter;

	void UpdateGatherInput()
	{
		inputSteer = 0;
		inputThrust = false;
		inputCenter = false;

		inputSteer += -Input.GetAxisRaw( "Horizontal");
		float inputY = Input.GetAxisRaw( "Vertical");
		if ( inputY > 0.25f)
		{
			inputThrust = true;
		}
		if ( inputY < -0.25f)
		{
			inputCenter = true;
		}

		// specific keys
		if (Input.GetKey( KeyCode.LeftControl))
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

		// first the center intent is used to generate a steer
		if (inputCenter)
		{
			inputSteer = -Mathf.Sign( rb2d.rotation);
		}
	}

	void UpdateSteering()
	{
		if (inputSteer != 0)
		{
			// lets you keep tumbling until you command input
			rb2d.angularVelocity = 0;

			float angle = rb2d.rotation;

			float prevAngle = angle;

			angle += inputSteer * RateOfTurn * Time.deltaTime;

			// second the center intent is used to stop zero-crossings
			if (inputCenter)
			{
				// crossing zero while Centering intent is on causes stop at zero
				if (Mathf.Sign( angle) != Mathf.Sign( prevAngle))
				{
					angle = 0;
				}
			}

			rb2d.MoveRotation( angle);
		}
	}

	void UpdateThrusting()
	{
		desiredEngineLevel = 0;

		Vector2 forward = rb2d.transform.up;

		Vector2 velocity = rb2d.velocity;

		if (inputThrust)
		{
			desiredEngineLevel = 1;
		}

		// fractional term
		currentEngineLevel = Mathf.Lerp( currentEngineLevel, desiredEngineLevel, engineSnappiness * Time.deltaTime);

		// additive term
		currentEngineLevel = Mathf.MoveTowards( currentEngineLevel, desiredEngineLevel, engineNullingTerm * Time.deltaTime);

		if (inputThrust)
		{
			float impulse = currentEngineLevel * Acceleration * Time.deltaTime;

			velocity += forward * impulse;
		}

		if (velocity.magnitude > MaxSpeed)
		{
			velocity = velocity.normalized * MaxSpeed;
		}

		velocity -= velocity * Damping * Time.deltaTime;

		rb2d.velocity = velocity;
	}

	void UpdateEngineAudio()
	{
		if (EngineAudioLoop)
		{
			float pitch = 1.0f + currentEngineLevel / 2.0f;

			float volume = currentEngineLevel;

			// apply the authored terms from the scene
			pitch *= originalAudioPitch;
			volume *= originalAudioVolume;

			EngineAudioLoop.pitch = pitch;
			EngineAudioLoop.volume = volume;
		}
	}

	void UpdateEngineParticles()
	{
		if (EngineParticleSystem)
		{
			float rate = currentEngineLevel * originalParticleEmissionRate;

			var em = EngineParticleSystem.emission;

			em.rateOverTimeMultiplier = rate;
		}
	}

	void FixedUpdate ()
	{
		UpdateGatherInput();

		UpdateSteering();

		UpdateThrusting();

		UpdateEngineAudio();

		UpdateEngineParticles();
	}
}
