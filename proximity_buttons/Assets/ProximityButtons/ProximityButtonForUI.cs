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

	RectTransform canvasRT;
	CanvasScaler scaler;

	void Awake()
	{
		var canvas = ContainingRectangle.GetComponentInParent<Canvas>();
		if (canvas.renderMode != RenderMode.ScreenSpaceOverlay)
		{
			Debug.LogError( GetType() + ".Awake(): Unsupported Canvas.RenderMode: " + canvas.renderMode.ToString());
		}
		canvasRT = canvas.GetComponent<RectTransform>();
		scaler = canvasRT.GetComponent<CanvasScaler>();
	}

	public Vector3 ScreenPosToUIPos( Vector3 pos)
	{
		switch( scaler.uiScaleMode)
		{
		case CanvasScaler.ScaleMode.ConstantPixelSize :
			pos = (pos * canvasRT.rect.height) / Screen.height;
			break;

//		case CanvasScaler.ScaleMode.ScaleWithScreenSize :
//			break;

//		case CanvasScaler.ScaleMode.ConstantPhysicalSize :
//			break;

		default :
			Debug.LogError( GetType() +
				".GetButtonTouchedNames(): unhandled CanvasScaler ScaleMode: " +
				scaler.uiScaleMode.ToString());
			break;
		}
		return pos;
	}

	public string[] GetButtonTouchedNames()
	{
		MicroTouch[] mts = MicroTouch.GatherMicroTouches();

		string[] results = new string[ ButtonSubRectangles.Length];

		return results;
	}

	void Update ()
	{
		GetButtonTouchedNames();
	}
}
