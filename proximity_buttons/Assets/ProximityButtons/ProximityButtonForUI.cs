using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityButtonForUI : MonoBehaviour
{
	[Tooltip("Populate this with RectTransforms with the things you want considered as touchable points. " +
		"These do NOT have to be buttons and don't even have to be raycastable at this stage.")]
	public RectTransform[] ButtonSubRectangles;

	[Tooltip("Popoulate this with a separate RectTransform that defines the area you want considered close enough " +
		"for the individual RectTransforms inside. Also used to find the root canvas RectTransform.")]
	public RectTransform SuperRectangle;

	static Vector2 center
	{
		get
		{
			return new Vector2( Screen.width, Screen.height) / 2;
		}
	}

	public RectTransform crosshairs;

	public string[] GetButtonTouchedNames()
	{
		MicroTouch[] mts = MicroTouch.GatherMicroTouches();

		if (mts.Length == 0) return null;

		Canvas canvas = SuperRectangle.GetComponentInParent<Canvas>();
		RectTransform canvasRT = canvas.GetComponent<RectTransform>();

		float ratio = canvasRT.rect.height / Screen.height;

		foreach( MicroTouch mt in mts)
		{
			Vector2 pos = mt.position * ratio;

			crosshairs.position = pos;
		}

		string[] results = new string[ ButtonSubRectangles.Length];

		return results;
	}

	void Update ()
	{
		GetButtonTouchedNames();
	}
}
