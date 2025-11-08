using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SpaceshipComplex2D
{
	void FabricatePlayerShip()
	{
		Bodies = new List<Rigidbody2D>();

		// how fragile is this thing?
		const float breakForce = 200;
		const float breakTorque = 200;

		// center around (0,0)
		Vector2 position = Vector2.left * (ChunkPrefabs.Length - 1) * 0.5f;

		FlamePrefab.transform.position = Vector3.zero;

		for (int i = 0; i < ChunkPrefabs.Length; i++)
		{
			var prefab = ChunkPrefabs[i];

			prefab.transform.position = Vector3.zero;

			var currChunk = Instantiate<ChunkAdaptor>(prefab, position, Quaternion.identity);

			Rigidbody2D currBody = currChunk.gameObject.AddComponent<Rigidbody2D>();

			currBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
			currBody.drag = 0.0f;
			currBody.angularDrag = 0.0f;

			// connect them with fixed joints
			if (i > 0)
			{
				Rigidbody2D prevBody = Bodies[i - 1];
				var fj1 = prevBody.gameObject.AddComponent<FixedJoint2D>();
				var fj2 = currBody.gameObject.AddComponent<FixedJoint2D>();

				// crosslink: forward
				fj1.connectedBody = currBody;
				fj1.breakForce = breakForce;
				fj1.breakTorque = breakTorque;

				// crosslink: backward
				fj2.connectedBody = prevBody;
				fj2.breakForce = breakForce;
				fj2.breakTorque = breakTorque;
			}

			if (i == LeftEngineIndex)
			{
				leftEngine = currBody;

				leftFlame = Instantiate<FlameAdaptor>(
					FlamePrefab,
					currChunk.EngineExhaust);
			}
			if (i == RightEngineIndex)
			{
				rightEngine = currBody;

				rightFlame = Instantiate<FlameAdaptor>(
					FlamePrefab,
					currChunk.EngineExhaust);
			}

			Bodies.Add(currBody);

			// TODO: consider the size of the chunk for spacing...

			position += Vector2.right;
		}

		// for cheesy in-scene "prefabs"
		if (Assets && Assets.activeInHierarchy)
		{
			DestroyImmediate(Assets);
		}
	}
}
