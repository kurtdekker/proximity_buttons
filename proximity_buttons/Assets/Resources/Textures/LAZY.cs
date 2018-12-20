using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LAZY
{
	static Texture2D _t2d_crosshairs;
	public static Texture2D t2d_crosshairs
	{
		get
		{
			if (!_t2d_crosshairs)
			{
				_t2d_crosshairs = Resources.Load<Texture2D>( "Textures/plus9");
			}
			return _t2d_crosshairs;
		}
	}
}