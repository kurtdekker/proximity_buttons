/*
    The following license supersedes all notices in the source
    code.
*/

/*
    Copyright (c) 2015 Kurt Dekker/PLBM Games All rights reserved.
    
    Redistribution and use in source and binary forms, with or without
    modification, are permitted provided that the following conditions are
    met:
    
    Redistributions of source code must retain the above copyright notice,
    this list of conditions and the following disclaimer.
    
    Redistributions in binary form must reproduce the above copyright
    notice, this list of conditions and the following disclaimer in the
    documentation and/or other materials provided with the distribution.
    
    Neither the name of the Kurt Dekker/PLBM Games nor the names of its
    contributors may be used to endorse or promote products derived from
    this software without specific prior written permission.
    
    THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS
    IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED
    TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
    PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
    HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
    SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
    TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
    PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
    LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
    NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
    SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

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
