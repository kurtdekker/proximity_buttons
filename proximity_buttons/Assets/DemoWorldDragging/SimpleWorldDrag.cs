using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @kurtdekker
//
// simple world drag: this object must have ONE collider on it

public class SimpleWorldDrag : MonoBehaviour
{
	Plane plane;
	Vector3 lastWorldPosition;

	void Start()
	{
		plane = new Plane( inNormal: Vector3.up, inPoint: Vector3.zero);
	}

	bool dragging;

	void Update ()
	{
		var touches = MicroTouch.GatherMicroTouches();

		if (touches.Length == 1)
		{
			var touch = touches[0];

			var ray = Camera.main.ScreenPointToRay( touch.position);

			// you ray must go through the plane or else the drag ends
			float enter = 0;
			// you could alternately use geometry to cast this against
			if (plane.Raycast( ray: ray, enter: out enter))
			{
				// at this point, using ray and enter, we can get our world position that we touched
				var worldPosition = ray.GetPoint( enter);

				RaycastHit hit = default(RaycastHit);

				// now let's see if we touched this object
				if (touch.phase == TouchPhase.Began)
				{
					bool connected = false;

					// see who we poked
					if (Physics.Raycast( ray: ray, hitInfo: out hit))
					{
						// make sure it is us
						if (hit.collider.gameObject == gameObject)
						{
							connected = true;
						}
					}

					if (connected)
					{
						dragging = true;

						// record world position when we touched
						lastWorldPosition = worldPosition;
					}
					else
					{
						dragging = false;		// must be someone else, or nobody
					}
				}

				// all subsequent consideration requires that we have been drag-picked-up already
				if (dragging)
				{
					switch( touch.phase)
					{
					case TouchPhase.Canceled :
					case TouchPhase.Ended :
						dragging = false;
						break;

					case TouchPhase.Moved :
					case TouchPhase.Stationary :
						// take delta and store current to last
						Vector3 delta = worldPosition - lastWorldPosition;
						lastWorldPosition = worldPosition;

						// move yourself however you must (eg, rigidbody, or whatever)
						transform.position += delta;

						break;
					}
				}
			}
			else
			{
				dragging = false;		// not dragging on the plane anymore
			}
		}
	}
}
