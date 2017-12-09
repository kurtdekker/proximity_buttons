using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoTouchVSRawTouch : MonoBehaviour
{
	GameObject crosshairsTouch;
	GameObject crosshairsRaw;

	void Start ()
	{
		crosshairsTouch = GameObject.Find( "Crosshairs");

		crosshairsRaw = Instantiate<GameObject>( crosshairsTouch);
		foreach( Renderer r in crosshairsRaw.GetComponentsInChildren<Renderer>())
		{
			r.material.color = Color.red;
		}

		crosshairsTouch.transform.rotation = Quaternion.Euler( 0, 0, 2);
		crosshairsRaw.transform.rotation = Quaternion.Euler( 0, 0, -2);
	}

	void CastToWord( Transform tr, Vector3 tchpos)
	{
		tchpos.z = 10;
		tr.position = Camera.main.ScreenToWorldPoint( tchpos);
	}

	void Update ()
	{
		#if UNITY_EDITOR
		if (Input.GetMouseButton(0))
		{
			CastToWord( crosshairsTouch.transform, Input.mousePosition);
			CastToWord( crosshairsRaw.transform, Input.mousePosition);
		}
		return;
		#endif

		if (Input.touchCount > 0)
		{
			crosshairsTouch.SetActive( true);
			crosshairsRaw.SetActive( true);

			Touch t = Input.GetTouch( 0);

			CastToWord( crosshairsTouch.transform, t.position);
			CastToWord( crosshairsRaw.transform, t.rawPosition);
		}
		else
		{
			crosshairsTouch.SetActive( false);
			crosshairsRaw.SetActive( false);
		}		
	}
}
