using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @kurtdekker - for use with a sprite to show capture status through color;

public class BaseCapturedSpriteColor : MonoBehaviour, IBaseCapturedIndicator
{
	public SpriteRenderer TargetSprite;

	public Color CanBeCaptured;

	public Color HasBeenCaptured;

	public bool InitialCaptureStatus;

	bool captured;

	void Reset()
	{
		CanBeCaptured = Color.gray;
		HasBeenCaptured = Color.green;
		InitialCaptureStatus = false;;
	}

	void DriveCaptureStatus( bool _captured)
	{
		captured = _captured;

		if (TargetSprite)
		{
			TargetSprite.color = captured ? HasBeenCaptured : CanBeCaptured;
		}
	}

	void Start()
	{
		if (!TargetSprite)
		{
			TargetSprite = GetComponent<SpriteRenderer>();
		}

		captured = InitialCaptureStatus;

		DriveCaptureStatus(InitialCaptureStatus);
	}

	public void SetCapturedStatus (bool captured)
	{
		DriveCaptureStatus( captured);
	}
}
