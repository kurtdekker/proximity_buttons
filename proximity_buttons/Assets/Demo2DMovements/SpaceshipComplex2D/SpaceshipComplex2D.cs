using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @kurtdekker - simple multi-Rigidbody2D spaceship demo
//
// To play a game based on this, get my free Impulse One Eagle2D game:
//
//	https://itunes.apple.com/us/app/impulse-one/id1001814482
//	https://play.google.com/store/apps/details?id=com.plbm.impulse1
//

public partial class SpaceshipComplex2D : MonoBehaviour
{
	[Header("Lay out what you want your ship to look like:")]
	public ChunkAdaptor[] ChunkPrefabs;

	[Header("Indexes into the above list:")]
	public int LeftEngineIndex;
	public int RightEngineIndex;

	[Header("ParticleSystem flame:")]
	public FlameAdaptor FlamePrefab;

	[Header("To destroy after we're done:")]
	public GameObject Assets;

	List<Rigidbody2D> Bodies;

	Rigidbody2D leftEngine;
	Rigidbody2D rightEngine;
	FlameAdaptor leftFlame;
	FlameAdaptor rightFlame;

	bool engineOn;
	float thrustLeftAccumulator;
	float thrustRightAccumulator;

	const float ClimbAuthority = 0.5f;
	const float DescentAuthority = 0.8f;

	// how much delta can we develop with left/right?
	const float TiltAuthority = 0.25f;

	// how quickly do the engines respond to inputs?
	const float IndividualEngineResponsiveness = 8.0f;

	private void Start()
	{
		FabricatePlayerShip();

		engineOn = true;
	}

	void Update()
	{
		Update_GatherInput();
	}

	void FixedUpdate()
	{
		FixedUpdate_ApplyPhysics();
	}

	private void LateUpdate()
	{
		LateUpdate_Camera();
	}


	Vector2 savedGravity;
	private void OnEnable()
	{
		savedGravity = Physics2D.gravity;
		Physics2D.gravity = Vector2.down * 5.0f;
	}
	private void OnDisable()
	{
		Physics2D.gravity = savedGravity;	
	}
}
