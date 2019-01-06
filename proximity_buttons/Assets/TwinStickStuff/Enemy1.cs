using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
	float speed;

	public static Enemy1 Attach( GameObject go, float speed)
	{
		var e1 = go.AddComponent<Enemy1>();
		e1.speed = speed;
		return e1;
	}

	void Update ()
	{
		Vector3 player = TwinStickGameManager.PlayerPosition;

		Vector3 delta = transform.position - player;

		float angle = Mathf.Rad2Deg * Mathf.Atan2( delta.x, delta.z);
		transform.rotation = Quaternion.Euler( 0, angle, 0);
	}
}
