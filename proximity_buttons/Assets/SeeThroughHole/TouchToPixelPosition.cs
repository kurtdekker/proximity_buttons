using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchToPixelPosition : MonoBehaviour
{
	public RectTransform CenterInConstantPixelSizeCanvas;

	void Update ()
	{
		bool input = false;
		Vector2 position = Vector2.zero;

		var mts = MicroTouch.GatherMicroTouches();

		// only function with exactly one touch
		if (mts.Length == 1)
		{
			var mt = mts[0];

			position = mt.position;

			input = true;
		}

		if (input)
		{
			CenterInConstantPixelSizeCanvas.position = position;
		}
	}
}
