﻿/*
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

public static class ImageUtils
{
	// if your destination image has a lot of small areas that require
	// lots of maneuvering to get into (ie., a massive amount of u-shaped
	// areas in it), you may need to increase this number.
	const int FILL_STACKMAX = 10240;

	// instead of a hard stop color (which must be absolutely precise numerically),
	// you may supply a TestStopColor delegate.
	//
	// WARNING: in that case you must still supply a stopColor that WOULD test for true in your delegate
	//
	public static void FloodFill(
		Texture2D readTexture, Texture2D writeTexture,
		Color fill_color, Color stopColor,
		int x, int y,
		System.Func<Color,bool> TestStopColor = null)
	{
		Color[] readColors = readTexture.GetPixels();
		int width = readTexture.width;
		int height = readTexture.height;
		int stride = readTexture.width;

		Color[] writeColors = writeTexture.GetPixels();

		int fill_stackptr = 0;
		int[] fill_stackx = new int[ FILL_STACKMAX];
		int[] fill_stacky = new int[ FILL_STACKMAX];

		System.Func<int,int,int> XYADDR = (xxx,yyy) => {
			return xxx + stride * yyy;
		};

		System.Action<int,int> PUSH = (xxx,yyy) => {
			fill_stackx[fill_stackptr] = xxx;
			fill_stacky[fill_stackptr] = yyy;
			fill_stackptr++;
		};
		System.Action<System.Action<int,int>> POP = (setxy) => {
			fill_stackptr--;
			setxy( fill_stackx[fill_stackptr], fill_stacky[fill_stackptr]);
		};

		System.Func<int,int,Color> GETPIXEL = (xxx,yyy) => {
			if (xxx < 0) return stopColor;
			if (xxx >= width) return stopColor;
			if (yyy < 0) return stopColor;
			if (yyy >= height) return stopColor;
			return readColors[ XYADDR( xxx, yyy)];
		};

		System.Action<int,int> SETPIXEL = (xxx,yyy) => {
			if (xxx < 0) return;
			if (xxx >= width) return;
			if (yyy < 0) return;
			if (yyy >= height) return;
			readColors[ XYADDR( xxx, yyy)] = fill_color;
			writeColors[ XYADDR( xxx, yyy)] = fill_color;
		};

		if (TestStopColor == null)
		{
			TestStopColor = (c1) => { return c1 == stopColor; };
		}

		Color fill_prevcolor = readColors[ XYADDR( x, y)];

		if (TestStopColor( fill_prevcolor))
		{
			return;
		}

		if (fill_color != fill_prevcolor)
		{
			while(true)
			{
				Color pixelBelow = GETPIXEL( x, y + 1);
				if ((TestStopColor( pixelBelow) == false) && (pixelBelow != fill_color))
				{
					PUSH( x, y + 1);
				}
				Color pixelAbove = GETPIXEL( x, y - 1);
				if ((TestStopColor( pixelAbove) == false) && (pixelAbove != fill_color))
				{
					PUSH( x, y - 1);
				}

				for (int leftright = 0; leftright < 2; leftright++)
				{
					int direction = leftright * 2 - 1;

					int x1 = x;
					int yAbove = y - 1;
					int yBelow = y + 1;
					Color colorAbove = fill_prevcolor;
					Color colorBelow = fill_prevcolor;

					do
					{
						if (TestStopColor( GETPIXEL( x1, y)) == false)
						{
							SETPIXEL( x1, y);
						}
						if (TestStopColor(colorAbove))
						{
							if (TestStopColor( GETPIXEL( x1, yAbove)) == false)
							{
								PUSH( x1, yAbove);
							}
						}
						if (TestStopColor( colorBelow))
						{
							if (TestStopColor( GETPIXEL( x1, yBelow)) == false)
							{
								PUSH( x1, yBelow);
							}
						}
						colorAbove = GETPIXEL( x1, yAbove);
						colorBelow = GETPIXEL( x1, yBelow);
						x1 += direction;
					}
					while( TestStopColor( GETPIXEL( x1, y)) == false);
				}

				if (fill_stackptr > 0)
				{
					POP( (xxx,yyy) => { x = xxx; y = yyy;});
				}
				else
				{
					break;
				}
			}
		}

		writeTexture.SetPixels( writeColors);
		writeTexture.Apply();
	}
}
