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

	Vector2 ScreenPosToUIPos( Vector2 pos)
	{
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
		return pos;
	}

	public RectTransform crosshairs;

	public string[] GetButtonTouchedNames()
	{
		MicroTouch[] mts = MicroTouch.GatherMicroTouches();

		if (mts.Length == 0)
		{
			int steps = (4 + 1 + ButtonSubRectangles.Length);

			int corner = ((int)(Time.time * 5)) % steps;

			// default is center of super rectangle
			Vector3 pos = new Vector2( ContainingRectangle.rect.center.x, ContainingRectangle.rect.center.y);
			pos += ContainingRectangle.position;
			switch( corner)
			{
			case 0 :	// handled above
				break;
			case 1 :
				pos = new Vector2( ContainingRectangle.rect.x, ContainingRectangle.rect.y);
				pos += ContainingRectangle.position;
				break;
			case 2 :
				pos = new Vector2( ContainingRectangle.rect.x + ContainingRectangle.rect.width, ContainingRectangle.rect.y);
				pos += ContainingRectangle.position;
				break;
			case 3 :
				pos = new Vector2( ContainingRectangle.rect.x + ContainingRectangle.rect.width, ContainingRectangle.rect.y + ContainingRectangle.rect.height);
				pos += ContainingRectangle.position;
				break;
			case 4 :
				pos = new Vector2( ContainingRectangle.rect.x, ContainingRectangle.rect.y + ContainingRectangle.rect.height);
				pos += ContainingRectangle.position;
				break;
			default :
				corner -= 5;		// now we index the remaining ButtonSubRectangles
				pos = ButtonSubRectangles[corner].position;
				break;
			}

			crosshairs.position = ScreenPosToUIPos(pos);

			return null;
		}

		foreach( MicroTouch mt in mts)
		{
			Vector2 pos = ScreenPosToUIPos( mt.position);

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
