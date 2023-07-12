using System.Collections;
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

	const float engineSnappiness = 12.0f;
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
			// lets you keep tumbling until you command input
			rb2d.angularVelocity = 0;

			float angle = rb2d.rotation;

			angle += inputSteer * RateOfTurn * Time.deltaTime;

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

			velocity += (forward * Acceleration * Time.deltaTime);

			if (velocity.magnitude > MaxSpeed)
			{
				velocity = velocity.normalized * MaxSpeed;
			}
		}

		velocity -= velocity * Damping * Time.deltaTime;

		rb2d.velocity = velocity;

		// fractional term
		currentEngineLevel = Mathf.Lerp( currentEngineLevel, desiredEngineLevel, engineSnappiness * Time.deltaTime);

		// additive term
		currentEngineLevel = Mathf.MoveTowards( currentEngineLevel, desiredEngineLevel, engineNullingTerm * Time.deltaTime);
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
