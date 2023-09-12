/*
	The following license supersedes all notices in the source code.

	Copyright (c) 2023 Kurt Dekker/PLBM Games All rights reserved.

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

public class TestFloodfill : MonoBehaviour
{
	[Header( "Change this:")]
	public Texture2D sourceTexture;

	[Header( "This is only for the Unlit material; the texture")]
	[Header( "specified will be replaced with the filled one.")]
	public Material material;

	GameObject quad;

	Texture2D workTexture;

	Collider collider;

	void Start ()
	{
		material = new Material( material);

		material.mainTexture = sourceTexture;

		quad = GameObject.CreatePrimitive( PrimitiveType.Quad);
		quad.GetComponent<Renderer>().material = material;

		workTexture = Instantiate<Texture2D>( sourceTexture);

		material.mainTexture = workTexture;
		quad.GetComponent<Renderer>().material = material;

		collider = quad.GetComponent<Collider>();
	}

	static Color ColorVeryDark = Color.black;
	bool TestIfVeryDark( Color c)
	{
		if (c.r > 0.05f) return false;
		if (c.g > 0.05f) return false;
		if (c.b > 0.05f) return false;
		return true;
	}

	void Update()
	{
		var mts = MicroTouch.GatherMicroTouches();

		if (mts.Length == 1)
		{
			var mt = mts[0];

			if (mt.phase == TouchPhase.Began)
			{
				Vector3 mousePosition = mt.position;

				// assumes ortho!!
				Camera cam = Camera.main;

				Ray ray = cam.ScreenPointToRay( mousePosition);

				RaycastHit hit;
				if (collider.Raycast( ray, out hit, 1000))
				{
					var coord = hit.textureCoord;

					int width = workTexture.width;
					int height = workTexture.height;

					int x = (int)(coord.x * width);
					int y = (int)(coord.y * height);

					Debug.Log( System.String.Format( "{0},{1}", x, y));

					ImageUtils.FloodFill( sourceTexture, workTexture, Color.red, ColorVeryDark, x, y, TestIfVeryDark);
				}
			}
		}
	}
}
