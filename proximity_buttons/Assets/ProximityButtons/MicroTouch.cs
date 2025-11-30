using UnityEngine;
using System.Collections;

public class MicroTouch
{
	public int fingerId;
	public TouchPhase phase;
	public Vector3 position;

	public static MicroTouch[] GatherMicroTouches()
	{
		// decide if we need extra room for mouse touches
		int mouseTouches = 0;
		bool includeMouse0 = false;
		bool includeMouse1 = false;
		switch (Application.platform)
		{
		case RuntimePlatform.WindowsEditor:
		case RuntimePlatform.WindowsPlayer:
		case RuntimePlatform.OSXEditor:
		case RuntimePlatform.OSXPlayer:
		case RuntimePlatform.WebGLPlayer :
			if (Input.GetMouseButton(0) || Input.GetMouseButtonUp (0))
			{
				includeMouse0 = true;
				mouseTouches++;
			}
			if (Input.GetMouseButton(1) || Input.GetMouseButtonUp (1))
			{
				includeMouse1 = true;
				mouseTouches++;
			}
			break;
		}
		
		int numTouches = Input.touches.Length;

		numTouches += mouseTouches;

		MicroTouch[] mts = new MicroTouch[numTouches];

		int n = 0;

		if (includeMouse0)
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

		if (includeMouse1)
		{
			MicroTouch mt = new MicroTouch();
			mt.fingerId = -98;
			mt.position = Input.mousePosition;
			mt.phase = TouchPhase.Moved;
			if (Input.GetMouseButtonDown(1))
			{
				mt.phase = TouchPhase.Began;
			}
			if (Input.GetMouseButtonUp(1))
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
