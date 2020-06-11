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

using UnityEngine;
using System.Collections;

public class TextLayer : MonoBehaviour
{
	Color[] colorPalette;
	int[,] grid;
	Color[,] colorGrid;

	public void Set( int x, int y, int c)
	{
		if ( x >= 0 && x < grid.GetLength (0))
		{
			if ( y >= 0 && y < grid.GetLength (1))
			{
				y = (grid.GetLength( 1) - 1) - y;
				grid[x, y] = c;
				colorGrid[ x, y] = textColor;
			}
		}
	}
	public int Get( int x, int y)
	{
		if ( x >= 0 && x < grid.GetLength (0))
		{
			if ( y >= 0 && y < grid.GetLength (1))
			{
				y = (grid.GetLength( 1) - 1) - y;
				return grid[x, y];
			}
		}
		return -1;
	}
	public Color GetColor( int x, int y)
	{
		if ( x >= 0 && x < grid.GetLength (0))
		{
			if ( y >= 0 && y < grid.GetLength (1))
			{
				y = (grid.GetLength( 1) - 1) - y;
				return colorGrid[x, y];
			}
		}
		return Color.magenta;
	}
	public void Cls( int c)
	{
		textColor = Color.white;
		for (int j = 0; j < grid.GetLength( 1); j++)
		{
			for (int i = 0; i < grid.GetLength( 0); i++)
			{
				Set ( i, j, c);
			}
		}
	}
	public void Cls()
	{
		Cls (32);		// Space
	}

	public int cursorx { get; private set; }
	public int cursory { get; private set; }
	Color textColor = Color.white;

	int EscapeCharacter;
	public void Putc( int c)
	{
		switch( EscapeCharacter)
		{
		case CharForegroundColor :
			SetTextColor( c - '@');
			EscapeCharacter = 0;
			return;
		}
		// escape code processing
		if (c == CharForegroundColor)
		{
			EscapeCharacter = c;
			return;
		}

		EscapeCharacter = 0;

		Set (cursorx, cursory, c);
		cursorx++;
	}

	public void Puts( string s)
	{
		for (int i = 0; i < s.Length; i++)
		{
			Putc( s[i]);
		}
	}

	public void Puts( int x, int y, string s)
	{
		Gotoxy (x, y);
		for (int i = 0; i < s.Length; i++)
		{
			Putc( s[i]);
		}
	}
	
	public const char CharForegroundColor = (char)1;

	public int CalcWidth( string s)
	{
		int count = 0;
		for (int i = 0; i < s.Length; i++)
		{
			switch (s[i])
			{
			case CharForegroundColor :
				count -= 2;
				break;
			}
			count++;
		}
		return count;
	}

	public void Centers( string s)
	{
		Gotoxy (cursorx - CalcWidth (s) / 2, cursory);
		Puts (s);
	}
	public void Centers( int x, int y, string s)
	{
		Gotoxy (x, y);
		Centers (s);
	}
	public void Rights( string s)
	{
		Gotoxy (cursorx - CalcWidth (s), cursory);
		Puts (s);
	}
	public void Rights( int x, int y, string s)
	{
		Gotoxy (x, y);
		Rights (s);
	}
	public void Vputs( string s)
	{
		int x = cursorx;
		int y = cursory;
		for (int i = 0; i < s.Length; i++)
		{
			cursorx = x;
			cursory = y + i;
			Putc ( s[i]);
		}
	}
	public void Vputs( int x, int y, string s)
	{
		Gotoxy (x, y);
		Vputs (s);
	}
	public void Vcenters( string s)
	{
		int x = cursorx;
		int y = cursory - CalcWidth ( s) / 2;
		for (int i = 0; i < s.Length; i++)
		{
			cursorx = x;
			cursory = y + i;
			Putc ( s[i]);
		}
	}
	public void Vcenters( int x, int y, string s)
	{
		Gotoxy (x, y - CalcWidth( s) / 2);
		Vputs (s);
	}

	public void Gotoxy( int x, int y)
	{
		cursorx = x;
		cursory = y;
	}

	public void SetTextColor( Color c)
	{
		// The shader I'm using considers 0.5f to be full color deflection,
		// so that it is capable of brightening grayscale textures.
		textColor = new Color( c.r * 0.5f, c.g * 0.5f, c.b * 0.5f);
	}
	public void SetTextColor( int n)
	{
		if (n >= 0 && n < colorPalette.Length)
		{
			SetTextColor (colorPalette [n]);
			return;
		}
		SetTextColor (Color.magenta);
	}

	float z;

	Color[] LoadColorPalette( string assetName)
	{
		Texture2D t2d = Resources.Load<Texture2D>("Palettes/" + assetName);
		if (t2d)
		{
			int n = t2d.width;
			Color[] colors = new Color[n];
			for (int i = 0; i < n; i++)
			{
				colors[i] = t2d.GetPixel ( i, 0);
			}
			return colors;
		}
		return null;
	}

	public int CycleColorInt( float stepRate)
	{
		return (int)((Time.time * stepRate) % colorPalette.Length);
	}

	public int CycleColorInt()
	{
		return CycleColorInt (8);
	}

	public static TextLayer Create( float z)
	{
		TextLayer tl = new GameObject ("TextLayer").AddComponent<TextLayer> ();
		tl.z = z;
		tl.grid = new int[TextMetrics.W, TextMetrics.H];
		tl.colorGrid = new Color[TextMetrics.W, TextMetrics.H];
		tl.transform.position = Vector3.forward * z;

		tl.colorPalette = tl.LoadColorPalette ("ega16");

		tl.Render ();

		return tl;
	}

	MeshFilter mf;
	MeshRenderer mr;

	Vector2[] uvs;
	Color[] vertexColors;

	const float dx = 1 / 16.0f;
	const float dy = 1 / 16.0f;

	public float XSZ { get; private set; }
	public float YSZ { get; private set; }

	void Render()
	{
		transform.localScale = TextMetrics.Scale;

		if (mf == null)
		{
			mf = gameObject.AddComponent<MeshFilter>();
			mr = gameObject.AddComponent<MeshRenderer>();

			mr.material = Resources.Load<Material>(
//				"Charsets/c64square"
				"Charsets/c64colored"
			);

			int numCells = TextMetrics.W * TextMetrics.H;
			Vector3[] verts = new Vector3[ numCells * 4];
			int[] tris = new int[numCells * 6];

			float bigger = Mathf.Max ( Screen.width, Screen.height);
			float smaller = Mathf.Min ( Screen.width, Screen.height);

			float height = TextMetrics.OrthographicSize * 2;
			YSZ = height;
			XSZ = ((height * bigger) / smaller);
			float ysz = YSZ / TextMetrics.H;
			float xsz = XSZ / TextMetrics.W;

			int nv = 0;
			int ni = 0;
			for (int j = 0; j < grid.GetLength( 1); j++)
			{
				for (int i = 0; i < grid.GetLength( 0); i++)
				{
					Vector3 v0 = new Vector3(
						xsz * (i - TextMetrics.W / 2),
						ysz * ((TextMetrics.H / 2 - 1) - j), z);
					verts[nv] = v0;
					verts[nv + 1] = v0 + new Vector3( xsz, 0);
					verts[nv + 2] = v0 + new Vector3( xsz, ysz);
					verts[nv + 3] = v0 + new Vector3( 0, ysz);

					tris[ni] = nv;
					tris[ni + 1] = nv + 3;
					tris[ni + 2] = nv + 2;

					ni += 3;

					tris[ni] = nv;
					tris[ni + 1] = nv + 2;
					tris[ni + 2] = nv + 1;

					nv += 4;
					ni += 3;
				}
			}

			mf.mesh.vertices = verts;
			mf.mesh.triangles = tris;

			uvs = new Vector2[ numCells * 4];

			vertexColors = new Color[ numCells * 4];
		}

		{
			float inset = 0.001f;

			int nv = 0;
			for (int j = 0; j < grid.GetLength( 1); j++)
			{
				for (int i = 0; i < grid.GetLength( 0); i++)
				{
					int c = Get (i, j);
					float fx = (c & 0x0f) / 16.0f;
					float fy = (15 - ((c >> 4) & 0x0f)) / 16.0f;
					uvs [nv] = new Vector2 (fx + inset, fy + inset);
					uvs [nv + 1] = new Vector2 (fx + dx - inset, fy + inset);
					uvs [nv + 2] = new Vector2 (fx + dx - inset, fy + dy - inset);
					uvs [nv + 3] = new Vector2 (fx + inset, fy + dy - inset);
					Color color = GetColor( i, j);
					vertexColors[nv] = color;
					vertexColors[nv + 1] = color;
					vertexColors[nv + 2] = color;
					vertexColors[nv + 3] = color;
					nv += 4;
				}
			}
			mf.mesh.uv = uvs;
			mf.mesh.colors = vertexColors;
		}
	}

	public System.Action finalLayer;

	void LateUpdate()
	{
		if (finalLayer != null) finalLayer();

		Render ();
	}
}
