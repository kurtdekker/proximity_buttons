using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdraftRigidbody2D : MonoBehaviour
{
	[Tooltip("How quick this wants to make you rise.")]
	public float VerticalSpeed = 5.0f;

	[Tooltip( "How quickly it moves to the above speed.")]
	public float VerticalAcceleration = 20.0f;

	[Tooltip( "Audio Sources:")]
	public AudioSource azz_Updraft;

	float originalVolume;

	// 0.0 to 1.0
	float currentVolume;
	float desiredVolume;
	const float VolumeSnappiness = 5.0f;

	void Start()
	{
		originalVolume = azz_Updraft.volume;
	}

	void Update()
	{
		currentVolume = Mathf.Lerp( currentVolume, desiredVolume, VolumeSnappiness * Time.deltaTime);

		azz_Updraft.volume = originalVolume * currentVolume;
	}

	void OnTriggerExit2D()
	{
		desiredVolume = 0.0f;
	}

	void OnTriggerStay2D( Collider2D collider)
	{
		var rb = collider.GetComponentInParent<Rigidbody2D>();
		if (rb)
		{
			var vel = rb.velocity;

			vel.y = Mathf.MoveTowards( vel.y, VerticalSpeed, VerticalAcceleration * Time.deltaTime);

			rb.velocity = vel;

			desiredVolume = 1.0f;
		}
	}
}
