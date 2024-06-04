using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @kurtdekker - to use: DO NOT DROP THIS INTO ANY SCENE!!!
// 
// this expects an orthographic camera!
//
// access the public LowerLeft / UpperRight fields to get corners, like so:
//
// Vector3 myLowerLeftCornerPosition = CornerTransformDriver.Instance.LowerLeft.position;
//
// DO NOT DROP THIS INTO ANY SCENE!! ONLY ACCESS .Instance!!
// 

public class CornerTransformDriver : MonoBehaviour
{
	static CornerTransformDriver _Instance;
	public static CornerTransformDriver Instance
	{
		get
		{
			if (!_Instance)
			{
				_Instance = new GameObject( "CornerTransformDriver").AddComponent<CornerTransformDriver>();
			}
			return _Instance;
		}
	}

	[Header( "Supply or we find / make.")]
	public Camera cam;

	// adjust if you like in code
	public float IntoScene = 10;

	public Transform LowerLeft { get; private set; }
	public Transform UpperRight { get; private set;}

	void Awake ()
	{
		LowerLeft = new GameObject( "LowerLeft").transform;
		UpperRight = new GameObject( "UpperRight").transform;
	}

	void Start()
	{
		// tidy up
		LowerLeft.transform.SetParent( transform);
		UpperRight.transform.SetParent( transform);
	}

	void LateUpdate ()
	{
		if (!cam)
		{
			cam = Camera.main;
		}

		if (cam)
		{
			LowerLeft.position = cam.ViewportToWorldPoint( new Vector3( 0, 0, IntoScene));
			UpperRight.position = cam.ViewportToWorldPoint( new Vector3( 1, 1, IntoScene));
		}
		else
		{
			Debug.LogError( "Can't find Main Camera. Fix please.");
			Debug.Break();
		}
	}
}
