using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OurStyles
{
	public static float NotionalPointWidth
	{
		get
		{
			return (Screen.width > Screen.height) ? 480.0f : 320.0f;
		}
	}

	private static Dictionary<int,GUIStyle> _LABELLJ;
	public static GUIStyle LABELLJ( int fontsize)
	{
		if (_LABELLJ == null)
		{
			_LABELLJ = new Dictionary<int, GUIStyle>();
		}
		if (!_LABELLJ.ContainsKey(fontsize))
		{
			GUIStyle gst = new GUIStyle();
			gst.alignment = TextAnchor.MiddleLeft;
			gst.fontSize = (int)((fontsize * Screen.width) / NotionalPointWidth);
			gst.normal.textColor = Color.white;
			_LABELLJ[fontsize] = gst;
		}
		return _LABELLJ[fontsize];
	}

	private static Dictionary<int,GUIStyle> _LABELULX;
	public static GUIStyle LABELULX( int fontsize)
	{
		if (_LABELULX == null)
		{
			_LABELULX = new Dictionary<int, GUIStyle>();
		}
		if (!_LABELULX.ContainsKey(fontsize))
		{
			GUIStyle gst = new GUIStyle();
			gst.alignment = TextAnchor.UpperLeft;
			gst.wordWrap = true;
			gst.fontSize = (int)((fontsize * Screen.width) / NotionalPointWidth);
			gst.normal.textColor = Color.white;
			_LABELULX[fontsize] = gst;
		}
		return _LABELULX[fontsize];
	}

	private static Dictionary<int,GUIStyle> _LABELURX;
	public static GUIStyle LABELURX( int fontsize)
	{
		if (_LABELURX == null)
		{
			_LABELURX = new Dictionary<int, GUIStyle>();
		}
		if (!_LABELURX.ContainsKey(fontsize))
		{
			GUIStyle gst = new GUIStyle();
			gst.alignment = TextAnchor.UpperRight;
			gst.wordWrap = true;
			gst.fontSize = (int)((fontsize * Screen.width) / NotionalPointWidth);
			gst.normal.textColor = Color.white;
			_LABELURX[fontsize] = gst;
		}
		return _LABELURX[fontsize];
	}
	
	private static Dictionary<int,GUIStyle> _LABELRJ;
	public static GUIStyle LABELRJ( int fontsize)
	{
		if (_LABELRJ == null)
		{
			_LABELRJ = new Dictionary<int, GUIStyle>();
		}
		if (!_LABELRJ.ContainsKey(fontsize))
		{
			GUIStyle gst = new GUIStyle();
			gst.alignment = TextAnchor.MiddleRight;
			gst.fontSize = (int)((fontsize * Screen.width) / NotionalPointWidth);
			gst.normal.textColor = Color.white;
			_LABELRJ[fontsize] = gst;
		}
		return _LABELRJ[fontsize];
	}

	private static Dictionary<int,GUIStyle> _LABELCJ;
	public static GUIStyle LABELCJ( int fontsize)
	{
		if (_LABELCJ == null)
		{
			_LABELCJ = new Dictionary<int, GUIStyle>();
		}
		if (!_LABELCJ.ContainsKey(fontsize))
		{
			GUIStyle gst = new GUIStyle();
			gst.alignment = TextAnchor.MiddleCenter;
			gst.fontSize = (int)((fontsize * Screen.width) / NotionalPointWidth);
			gst.normal.textColor = Color.white;
			_LABELCJ[fontsize] = gst;
		}
		return _LABELCJ[fontsize];
	}
	
	private static GUISkin _SKINBASE;
	private static GUISkin SKINBASE
	{
		get
		{
			if (_SKINBASE == null)
			{
				_SKINBASE = Resources.Load( "GUISkins/SKINBASE") as GUISkin;
			}
			return _SKINBASE;
		}
	}
	private static Dictionary<int,GUIStyle> _BUTTONBASE;
	public static GUIStyle BUTTONBASE( int fontsize)
	{
		if (_BUTTONBASE == null)
		{
			_BUTTONBASE = new Dictionary<int, GUIStyle>();
		}
		if (!_BUTTONBASE.ContainsKey(fontsize))
		{
			GUIStyle gst = new GUIStyle( SKINBASE.button);
			gst.alignment = TextAnchor.MiddleCenter;
			gst.fontSize = (int)((fontsize * Screen.width) / NotionalPointWidth);
			gst.normal.textColor = Color.white;
			_BUTTONBASE[fontsize] = gst;
		}
		return _BUTTONBASE[fontsize];
	}
	private static Dictionary<int,GUIStyle> _BOXBASE;
	public static GUIStyle BOXBASE( int fontsize)
	{
		if (_BOXBASE == null)
		{
			_BOXBASE = new Dictionary<int, GUIStyle>();
		}
		if (!_BOXBASE.ContainsKey(fontsize))
		{
			GUIStyle gst = new GUIStyle( SKINBASE.box);
			gst.alignment = TextAnchor.UpperCenter;
			gst.fontSize = (int)((fontsize * Screen.width) / NotionalPointWidth);
			gst.normal.textColor = Color.white;
			_BOXBASE[fontsize] = gst;
		}
		return _BOXBASE[fontsize];
	}
}
