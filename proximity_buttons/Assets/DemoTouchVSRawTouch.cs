﻿/*
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
