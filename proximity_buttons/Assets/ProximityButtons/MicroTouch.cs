using UnityEngine;
using System.Collections;

public class MicroTouch
{
	public int fingerId;
	public TouchPhase phase;
	public Vector3 position;

	public static MicroTouch[] GatherMicroTouches()
	{
		bool includeMouse = false;
		if ((Application.platform == RuntimePlatform.WindowsEditor) ||
		    (Application.platform == RuntimePlatform.OSXEditor))
		{
			if (Input.GetMouseButton(0) || Input.GetMouseButtonUp (0))
			{
				includeMouse = true;
			}
		}
		
		int numTouches = Input.touches.Length;
		if (includeMouse)
		{
			numTouches++;
		}
		MicroTouch[] mts = new MicroTouch[numTouches];
		int n;
		n = 0;
		if (includeMouse)
		{
			MicroTouch mt = new MicroTouch();
			mt.fingerId = -99;
			mt.position = Input.mousePosition;
			mt.phase = TouchPhase.Moved;
			if (Input.GetMouseButtonDown(0))
			{
				mt.phase = TouchPhase.Began;
			}
			if (Input.GetMouseButtonUp(0))
			{
				mt.phase = TouchPhase.Ended;
			}
			mts[n++] = mt;
		}
		foreach (Touch t in Input.touches)
		{
			MicroTouch mt = new MicroTouch();
			mt.fingerId = t.fingerId;
			mt.position = t.position;
			mt.phase = t.phase;
			mts[n++] = mt;
		}
		
		return mts;
	}
}
