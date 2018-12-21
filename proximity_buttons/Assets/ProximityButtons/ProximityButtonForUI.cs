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

	// Returns a string array of the same length as the
	// number of subrectangle areas in the enclosing box.
	// Returns null if you aren't touching it, or the GameObject name if you are
	public string[] GetButtonTouchedNames()
	{
		MicroTouch[] allTouches = MicroTouch.GatherMicroTouches();

		List<MicroTouch> withins = new List<MicroTouch>();

		// Transform the screen touch coordinates into canvas UI coordinates
		// If they are within the super rect, add them to our checked touches
		for (int i = 0; i < allTouches.Length; i++)
		{
			var p = ScreenPosToUIPos( allTouches[i].position);
			if (p.x >= ContainingRectangle.rect.x + ContainingRectangle.position.x)
			{
				if (p.x < ContainingRectangle.rect.x + ContainingRectangle.rect.width + ContainingRectangle.position.x)
				{
					if (p.y >= ContainingRectangle.rect.y + ContainingRectangle.position.y)
					{
						if (p.y < ContainingRectangle.rect.y + ContainingRectangle.rect.height + ContainingRectangle.position.y)
						{
							allTouches[i].position = p;
							withins.Add( allTouches[i]);
						}
					}
				}
			}
		}

		string[] results = new string[ ButtonSubRectangles.Length];

		for (int t = 0; t < withins.Count; t++)
		{
			float distance = 0;
			int closestb = 0;

			for (int b = 0; b < ButtonSubRectangles.Length; b++)
			{
				float d = Vector3.Distance( withins[t].position, ButtonSubRectangles[b].position);
				if ( b == 0 || d < distance)
				{
					distance = d;
					closestb = b;
				}
			}

			results[closestb] = ButtonSubRectangles[closestb].name;
		}

		return results;
	}
}
