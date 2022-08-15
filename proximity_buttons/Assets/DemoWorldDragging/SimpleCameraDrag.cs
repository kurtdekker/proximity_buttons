using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCameraDrag : MonoBehaviour
{
	Plane plane;
	Vector3 lastWorldPosition;

	void Start()
	{
		plane = new Plane( inNormal: Vector3.up, inPoint: Vector3.zero);
	}

	void Update()
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

				if (touch.phase == TouchPhase.Began)
				{
					lastWorldPosition = worldPosition;		// record first
				}
				else
				{
					// we're not particular: any other input we'll treat as sliding
					Vector3 delta = worldPosition - lastWorldPosition;

					// we do not update the lastWorldPosition, since the camera takes care of that

					Vector3 position = Camera.main.transform.position;

					position -= delta;

					Camera.main.transform.position = position;
				}
			}
		}
	}
}
