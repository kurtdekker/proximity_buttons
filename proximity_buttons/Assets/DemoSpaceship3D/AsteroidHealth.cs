using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidHealth : MonoBehaviour, IDamageable
{
	public static AsteroidHealth Attach(GameObject go)
	{
		var ah = go.AddComponent<AsteroidHealth>();
		return ah;
	}

	public void TakeDamage(int damage)
	{
		Destroy(gameObject);

		// TODO: throw off other chunks here!
	}
}
