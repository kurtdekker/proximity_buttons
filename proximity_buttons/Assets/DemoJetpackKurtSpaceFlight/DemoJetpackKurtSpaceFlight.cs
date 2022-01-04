using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @kurtdekker
//
// This is a simplified version of the Space Flight
// control system in Jetpack Kurt.
//
// See the game:
//	https://youtu.be/34wbtTIWdDQ
//
// Get the game:
//	Appstore: https://itunes.apple.com/us/app/jetpack-kurt/id1033348911
//	GooglePlay: https://play.google.com/store/apps/details?id=com.plbm.jetpack
//	Android TV: https://play.google.com/store/apps/details?id=com.plbm.jetpacktv
//

public class DemoJetpackKurtSpaceFlight : MonoBehaviour
{
	Rigidbody rb;

	float gravityMagnitude;

	public ParticleSystem EngineParticleSystem;

	void Start ()
	{
		rb = GetComponent<Rigidbody>();

		// makes things a little more stable!!
		rb.centerOfMass = Vector3.down * 0.5f;

		Physics.gravity = Vector3.down * 2.0f;

		gravityMagnitude = Physics.gravity.magnitude;
	}

	// 1.0 is how much it takes to hover
	const float BasicPowerMargin = 1.5f;

	float GetPowerMargin()
	{
		return gravityMagnitude * rb.mass * BasicPowerMargin;
	}

	float GetNormalizedPowerSustainLevel()
	{
		return 1.0f / BasicPowerMargin;
	}

	// technically we should consider the inertial tensor
	// to determine these, but I use .mass as a decent proxy.
	float GetPitchAuthority()
	{
		return 0.5f * rb.mass;
	}

	float GetRollAuthority()
	{
		return 0.5f * rb.mass;
	}

	float GetYawAuthority()
	{
		return 0.25f * rb.mass;
	}

	// current inputs
	float pitch;
	float yaw;
	float roll;
	float power;

	// how rapidly do the yaw buttons build up?
	float persistentYaw;
	const float YawSnappiness = 5.0f;

	// how rapidly do the engine power build up?
	float persistentPower;
	const float PowerSnappiness = 5.0f;

	void Update()
	{
		if (Input.GetKeyDown( KeyCode.I))
		{
			DSM.SpaceFlight.InvertPitch.bToggle();
		}
		if (Input.GetKeyDown( KeyCode.E))
		{
			DSM.SpaceFlight.SustainedEngine.bToggle();
		}
		if (Input.GetKeyDown( KeyCode.R))
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(
				UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
		}
	}

	void GatherInputs()
	{
		pitch = 0;
		yaw = 0;
		roll = 0;
		power = 0;

		if (DSM.SpaceFlight.SustainedEngine.bValue)
		{
			power = GetNormalizedPowerSustainLevel();
		}

		// TODO: you can add/remove other input sources here:

		// layer in the basic Unity Input system
		roll -= Input.GetAxisRaw( "Horizontal");
		pitch -= Input.GetAxisRaw( "Vertical");

		// simple filtering for yaw buttons
		{
			float desiredYaw = 0;
			if (Input.GetKey( KeyCode.Alpha1))
			{
				desiredYaw = -1.0f;
			}
			if (Input.GetKey( KeyCode.Alpha2))
			{
				desiredYaw = +1.0f;
			}

			persistentYaw = Mathf.MoveTowards( persistentYaw, desiredYaw, YawSnappiness * Time.deltaTime);

			yaw += persistentYaw;
		}

		// simple filtering for power button
		{
			float desiredPower = 0;
			if (Input.GetKey( KeyCode.Space))
			{
				desiredPower = 1.0f;
			}

			persistentPower = Mathf.MoveTowards( persistentPower, desiredPower, PowerSnappiness * Time.deltaTime);

			power += persistentPower;
		}

		if (DSM.SpaceFlight.InvertPitch.bValue)
		{
			pitch = -pitch;
		}
	}

	void ApplyInputs()
	{
		rb.AddForce( rb.transform.up * power * GetPowerMargin());

		rb.AddTorque( rb.transform.up * yaw * GetYawAuthority());
		rb.AddTorque( rb.transform.right * pitch * GetPitchAuthority());
		rb.AddTorque( rb.transform.forward * roll * GetRollAuthority());
	}

	void DriveEngineParticles()
	{
		float displayedPower = power;

		// keep it from being visible in cockpit
		if (DSM.SpaceFlight.FirstPersonCamera.bValue)
		{
			displayedPower = 0.0f;
		}

		var em = EngineParticleSystem.emission;
		em.rateOverTime = 100 * displayedPower;
	}

	void FixedUpdate ()
	{
		GatherInputs();

		ApplyInputs();

		DriveEngineParticles();
	}
}
