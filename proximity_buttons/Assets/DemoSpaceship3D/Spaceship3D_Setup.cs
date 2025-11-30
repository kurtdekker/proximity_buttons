using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Spaceship3D : MonoBehaviour
{
	void SetupPhysics()
	{
		if (physicMaterial == null)
		{
			physicMaterial = new PhysicMaterial();
			physicMaterial.bounciness = 0.4f;
			physicMaterial.staticFriction = 0.0f;
			physicMaterial.dynamicFriction = 0.0f;
		}

		Physics.gravity = Vector3.zero;

		SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
		sphereCollider.radius = 0.5f;
		sphereCollider.sharedMaterial = physicMaterial;

		rb = gameObject.GetComponent<Rigidbody>();
		if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
		rb.drag = 0;
		rb.angularDrag = 0;
		rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

		gameObject.layer = PlayerLayer;
	}

	void CreateLevel()
	{
		// axi-centric volume of our "world"
		Vector3 volume = new Vector3(250, 100, 250);

		Vector3 center = new Vector3(0, 0, 0);

		int boxCount = 1000;

		if (boxCount > 0)
		{
			for (int i = 0; i < boxCount; i++)
			{
				float fraction = (float)i / (boxCount - 1);
				float x = Mathf.Lerp(-volume.x, +volume.x, fraction);
				float y = Random.Range(-volume.y, +volume.y);
				float z = Random.Range(-volume.z, +volume.z);

				Vector3 position = center + new Vector3(x, y, z);

				GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

				float scale = Random.Range( 1.0f, 10.0f);
				cube.transform.localScale = Vector3.one * scale;

				cube.transform.position = position;
				cube.AddComponent<Rigidbody>();
				Collider collider = cube.GetComponent<Collider>();
				collider.sharedMaterial = physicMaterial;

				AsteroidHealth.Attach(cube);
			}
		}
	}
}
