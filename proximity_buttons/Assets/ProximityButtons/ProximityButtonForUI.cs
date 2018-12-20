using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProximityButtonForUI : MonoBehaviour
{
	[Tooltip("Populate this with RectTransforms with the things you want considered as touchable points. " +
		"These do NOT have to be buttons and don't even have to be raycastable at this stage.")]
	public RectTransform[] ButtonSubRectangles;

	[Tooltip("Popoulate this with a separate RectTransform that defines the area you want considered close enough " +
		"for the individual RectTransforms inside. Also used to find the root canvas RectTransform.")]
	public RectTransform ContainingRectangle;

	static Vector2 center
	{
		get
		{
			return new Vector2( Screen.width, Screen.height) / 2;
		}
	}

	RectTransform canvasRT;
	CanvasScaler scaler;

	void Awake()
	{
		var canvas = ContainingRectangle.GetComponentInParent<Canvas>();
		canvasRT = canvas.GetComponent<RectTransform>();
		scaler = canvasRT.GetComponent<CanvasScaler>();
	}

	public RectTransform crosshairs;

	public string[] GetButtonTouchedNames()
	{
		MicroTouch[] mts = MicroTouch.GatherMicroTouches();

		if (mts.Length == 0) return null;

		foreach( MicroTouch mt in mts)
		{
			Vector2 pos = mt.position;

			switch( scaler.uiScaleMode)
			{
			case CanvasScaler.ScaleMode.ConstantPixelSize :
				pos = (pos * canvasRT.rect.height) / Screen.height;
				break;
			case CanvasScaler.ScaleMode.ScaleWithScreenSize :
				break;
			case CanvasScaler.ScaleMode.ConstantPhysicalSize :
				break;
			default :
				Debug.LogError( GetType() +
					".GetButtonTouchedNames(): unhandled CanvasScaler ScaleMode: " +
					scaler.uiScaleMode.ToString());
				break;
			}

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
