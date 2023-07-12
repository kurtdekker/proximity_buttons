using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @kurtdekker - cheesy landing sensor - put this on the base

public class LandingSensor2D : MonoBehaviour
{
	bool captured;

	float  touchingTimer;

	float captureTimer;

	void OnCollisionStay2D( Collision2D collision)
	{
		// TODO: decide if the thing that hit us is actually the player

		// decide if the player is quiescent enough to consider "landed."
		Collider2D col = collision.collider;
		if (col.attachedRigidbody)
		{
			if (col.attachedRigidbody.velocity.magnitude < 0.1f)
			{
				touchingTimer = 0.1f;
			}
		}
	}

	void DriveIndicatorIfPresent()
	{
		IBaseCapturedIndicator indicator = GetComponent<IBaseCapturedIndicator>();

		if (indicator != null)
		{
			indicator.SetCapturedStatus( captured);
		}
	}

	void FixedUpdate()
	{
		bool touching = false;

		if (touchingTimer > 0)
		{
			touchingTimer -= Time.deltaTime;

			touching = true;
		}

		if (touching)
		{
			if (!captured)
			{
				captureTimer += Time.deltaTime;

				if (captureTimer >= 1.0f)
				{
					captured = true;

					Debug.Log( "Captured!");

					DriveIndicatorIfPresent();
				}
			}
		}
		else
		{
			captureTimer = 0.0f;

			captured = false;

			DriveIndicatorIfPresent();
		}
	}
}
