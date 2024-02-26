using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @kurtdekker - ultra simple 3D spacecraft controller
// to use, just drop it on a blank GameObject,
// and make sure you have a Main Camera!
//
// a discussion:
//	- raw digital inputs are time-filtered so they ramp up / down
//	- orientation:
//		- time-filtered inputs are scaled by desired rotational rates
//		- existing spacecraft rotational rates (in each axes) are gathered
//		- PD filters are used to compute desired torques to achieve rotations
//		- resultant torques are scaled by thruster powers and applied as torques
//	- fore / aft:
//		- drag driven by braking
//		- power driven by power
// and we're done!
//
// TODO: pull out flight parameters and put them in a ScriptableObject:
//
//	- snappiness
//	- rotational rates
//	- maximum torques
//	- maximum power
//	- baseline drag
//
// TODO: put spacecraft rigidbody parameters into a ScriptableObject:
//
//	- mass
//	- inertial tensor

public partial class Spaceship3D : MonoBehaviour
{
	[Header( "Provide or we find/make!")]
	public PhysicMaterial physicMaterial;

	[Header("Adjust to suit...")]
	public KeyCode keyRollLeft = KeyCode.LeftArrow;
	public KeyCode keyRollRight = KeyCode.RightArrow;
	public KeyCode keyPitchUp = KeyCode.UpArrow;
	public KeyCode keyPitchDown = KeyCode.DownArrow;
	public KeyCode keyYawLeft = KeyCode.Q;
	public KeyCode keyYawRight = KeyCode.E;

	public KeyCode keyPower = KeyCode.Space;
	public KeyCode keyBrake = KeyCode.Tab;

	public bool invertPitch;

	Camera cam;
	Rigidbody rb;

	void Start ()
	{
		cam = Camera.main;

		SetupPhysics();

		CreateLevel();

		SetupControlLawFilters();
	}

	void FixedUpdate()
	{
		GatherTimeFilteredInputs();

		ProcessOrientation();
		ProcessPower();
		ProcessBrake();
	}

	private void LateUpdate()
	{
		cam.transform.position = transform.position;
		cam.transform.rotation = transform.rotation;
	}
}
