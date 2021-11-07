using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdraftRigidbody2D : MonoBehaviour
{
	[Tooltip("How quick this wants to make you rise.")]
	public float VerticalSpeed = 5.0f;

	[Tooltip( "How quickly it moves to the above speed.")]
	public float VerticalAcceleration = 20.0f;

	void OnTriggerStay2D( Collider2D collider)
	{
		var rb = collider.GetComponentInParent<Rigidbody2D>();
		if (rb)
		{
			var vel = rb.velocity;

			vel.y = Mathf.MoveTowards( vel.y, VerticalSpeed, VerticalAcceleration * Time.deltaTime);

			rb.velocity = vel;
		}
	}
}
