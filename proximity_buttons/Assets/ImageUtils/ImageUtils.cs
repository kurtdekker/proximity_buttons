/*
    The following license supersedes all notices in the source code.
*/

/*
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

public static class ImageUtils
{
	const int FILL_STACKMAX = 10240;

	public static void FloodFill( Texture2D readTexture, Texture2D writeTexture, Color targetColor, Color stopColor, float tolerance, int x, int y)
	{
		Color[] readColors = readTexture.GetPixels();
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
			return readColors[ XYADDR( xxx, yyy)];
		};

		Color fill_prevcolor = readColors[ XYADDR( x, y)];
		if (targetColor != fill_prevcolor)
		{
			while(true)
			{
				
			}
		}

		writeTexture.SetPixels( writeColors);
		writeTexture.Apply();
	}
}
