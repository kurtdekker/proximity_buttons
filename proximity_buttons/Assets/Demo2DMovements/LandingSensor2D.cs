using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @kurtdekker - cheesy landing sensor - put this on the base

public class LandingSensor2D : MonoBehaviour, IResettableLandingSite
{
	bool captured;

	float touchingTimer;

	float captureTimer;

	const float TouchTimerStart = 0.1f;

	void OnCollisionStay2D( Collision2D collision)
	{
		// TODO: decide if the thing that hit us is actually the player

		// decide if the player is quiescent enough to consider "landed."
		Collider2D col = collision.collider;
		if (col.attachedRigidbody)
		{
			if (col.attachedRigidbody.velocity.magnitude < 0.1f)
			{
				touchingTimer = TouchTimerStart;
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

	private void Start()
	{
		LandingSensorResetter.Register(this);
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
			// we don't uncapture ourselves: see LandingSensorResetter.cs
			//captureTimer = 0.0f;
			//captured = false;
			//DriveIndicatorIfPresent();
		}
	}

	int IResettableLandingSite.GetIdentifier()
	{
		return GetInstanceID();
	}

	bool IResettableLandingSite.IsPlayerTouching()
	{
		if (touchingTimer > 0)
		{
			return true;
		}
		return false;
	}

	bool IResettableLandingSite.IsCaptured()
	{
		return captured;
	}

	void IResettableLandingSite.ResetMe()
	{
		captureTimer = 0.0f;

		captured = false;

		DriveIndicatorIfPresent();
	}
}
