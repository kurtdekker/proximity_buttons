/*
	The following license supersedes all notices in the source code.

	Copyright (c) 2018 Kurt Dekker/PLBM Games All rights reserved.

	http://www.twitter.com/kurtdekker

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoProximityButtonsForUI : MonoBehaviour
{
	public RectTransform crosshairs;

	public Text outputText;

	public ProximityButtonForUI pb4ui1;	// movement
	public ProximityButtonForUI pb4ui2;	// actions

	public void TestCoordinateTransforms()
	{
		MicroTouch[] mts = MicroTouch.GatherMicroTouches();

		if (mts.Length == 0)
		{
			int steps = (4 + 1 + pb4ui1.ButtonSubRectangles.Length);

			int corner = ((int)(Time.time * 5)) % steps;

			// default is center of super rectangle
			Vector3 pos = new Vector2( pb4ui1.ContainingRectangle.rect.center.x, pb4ui1.ContainingRectangle.rect.center.y);
			pos += pb4ui1.ContainingRectangle.position;
			switch( corner)
			{
			case 0 :	// handled above
				break;
			case 1 :
				pos = new Vector2( pb4ui1.ContainingRectangle.rect.x, pb4ui1.ContainingRectangle.rect.y);
				pos += pb4ui1.ContainingRectangle.position;
				break;
			case 2 :
				pos = new Vector2( pb4ui1.ContainingRectangle.rect.x + pb4ui1.ContainingRectangle.rect.width, pb4ui1.ContainingRectangle.rect.y);
				pos += pb4ui1.ContainingRectangle.position;
				break;
			case 3 :
				pos = new Vector2( pb4ui1.ContainingRectangle.rect.x + pb4ui1.ContainingRectangle.rect.width, pb4ui1.ContainingRectangle.rect.y + pb4ui1.ContainingRectangle.rect.height);
				pos += pb4ui1.ContainingRectangle.position;
				break;
			case 4 :
				pos = new Vector2( pb4ui1.ContainingRectangle.rect.x, pb4ui1.ContainingRectangle.rect.y + pb4ui1.ContainingRectangle.rect.height);
				pos += pb4ui1.ContainingRectangle.position;
				break;
			default :
				corner -= 5;		// now we index the remaining ButtonSubRectangles
				pos = pb4ui1.ButtonSubRectangles[corner].position;
				break;
			}

			crosshairs.position = pb4ui1.ScreenPosToUIPos(pos);

			return;
		}
		foreach( MicroTouch mt in mts)
		{
			Vector2 pos = pb4ui1.ScreenPosToUIPos( mt.position);

			crosshairs.position = pos;
		}
	}

	void Update()
	{
		TestCoordinateTransforms();

		string output = "";

		// get the movement input
		var names = pb4ui1.GetButtonTouchedNames();
		foreach( var s in names)
		{
			if (s == null) output += "--";
			else output += s;

			output += "\n";
		}

		// get the action input
		names = pb4ui2.GetButtonTouchedNames();
		foreach( var s in names)
		{
			if (s == null) output += "--";
			else output += s;

			output += "\n";
		}

		outputText.text = output;
	}
}
