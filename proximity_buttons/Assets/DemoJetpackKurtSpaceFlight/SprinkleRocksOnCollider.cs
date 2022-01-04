using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprinkleRocksOnCollider : MonoBehaviour
{
	public GameObjectCollection Rocks;

	public int MinRocks = 200;
	public int Maxrocks = 400;

	void Start ()
	{
		var col = GetComponent<Collider>();

		int numRocks = Random.Range( MinRocks, Maxrocks);

		for (int i = 0; i < numRocks; i++)
		{
			var p = col.bounds.center
				+
				new Vector3(
					Random.Range( -col.bounds.extents.x, +col.bounds.extents.x),
					0,
					Random.Range( -col.bounds.extents.z, +col.bounds.extents.z))
					+
					Vector3.up * 200.0f
				;

			var ray = new Ray( origin: p, direction: Vector3.down);

			RaycastHit hit;
			if (col.Raycast( ray, out hit, maxDistance: 1000))
			{
				var rock = Instantiate<GameObject>( Rocks.PickNextShuffled(), transform);

				rock.transform.position = hit.point;
				rock.transform.rotation = Quaternion.Euler (0, Random.Range( 0.0f, 360.0f), 0);
			}
		}
	}
}
