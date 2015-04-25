using UnityEngine;
using System.Collections;

// Seal THIS class you OO bitches!!!
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
			if (Input.GetMouseButton(0))
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
			mt.phase = Input.GetMouseButtonDown(0) ?
				TouchPhase.Began : TouchPhase.Moved;
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
