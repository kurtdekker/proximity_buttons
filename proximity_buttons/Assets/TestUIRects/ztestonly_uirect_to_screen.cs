using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ztestonly_uirect_to_screen : MonoBehaviour
{
	public RectTransform crosshairs;

	ProximityButtonForUI pb4ui;	// we'll find it

	public void TestCoordinateTransforms()
	{
		MicroTouch[] mts = MicroTouch.GatherMicroTouches();

		if (mts.Length == 0)
		{
			int steps = (4 + 1 + pb4ui.ButtonSubRectangles.Length);

			int corner = ((int)(Time.time * 5)) % steps;

			// default is center of super rectangle
			Vector3 pos = new Vector2( pb4ui.ContainingRectangle.rect.center.x, pb4ui.ContainingRectangle.rect.center.y);
			pos += pb4ui.ContainingRectangle.position;
			switch( corner)
			{
			case 0 :	// handled above
				break;
			case 1 :
				pos = new Vector2( pb4ui.ContainingRectangle.rect.x, pb4ui.ContainingRectangle.rect.y);
				pos += pb4ui.ContainingRectangle.position;
				break;
			case 2 :
				pos = new Vector2( pb4ui.ContainingRectangle.rect.x + pb4ui.ContainingRectangle.rect.width, pb4ui.ContainingRectangle.rect.y);
				pos += pb4ui.ContainingRectangle.position;
				break;
			case 3 :
				pos = new Vector2( pb4ui.ContainingRectangle.rect.x + pb4ui.ContainingRectangle.rect.width, pb4ui.ContainingRectangle.rect.y + pb4ui.ContainingRectangle.rect.height);
				pos += pb4ui.ContainingRectangle.position;
				break;
			case 4 :
				pos = new Vector2( pb4ui.ContainingRectangle.rect.x, pb4ui.ContainingRectangle.rect.y + pb4ui.ContainingRectangle.rect.height);
				pos += pb4ui.ContainingRectangle.position;
				break;
			default :
				corner -= 5;		// now we index the remaining ButtonSubRectangles
				pos = pb4ui.ButtonSubRectangles[corner].position;
				break;
			}

			crosshairs.position = pb4ui.ScreenPosToUIPos(pos);

			return;
		}
		foreach( MicroTouch mt in mts)
		{
			Vector2 pos = pb4ui.ScreenPosToUIPos( mt.position);

			crosshairs.position = pos;
		}
	}

	void Start ()
	{
		pb4ui = GetComponent<ProximityButtonForUI>();
	}

	void Update()
	{
		TestCoordinateTransforms();
	}
}
