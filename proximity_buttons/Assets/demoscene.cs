using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class demoscene : MonoBehaviour
{
	ProximityButtonSet pbsMove, pbsFire;

	ProximityButtonSet.ProximityButton pbLeft, pbRight, pbMove, pbFire;

	List<GameObject> indicators;

	void AddIndicator( Vector3 pos)
	{
		GameObject go = GameObject.CreatePrimitive (PrimitiveType.Cube);
		go.transform.position = pos;
		indicators.Add (go);
	}

	void Start ()
	{
		DetectReorientation ();

		indicators = new List<GameObject> ();
		AddIndicator (new Vector3 (-3.0f, 1.0f));
		AddIndicator (new Vector3 (-1.5f, 1.0f));
		AddIndicator (new Vector3 ( 1.5f, 1.0f));
		AddIndicator (new Vector3 ( 3.0f, 1.0f));
	}

	int detectedW, detectedH;
	void DetectReorientation()
	{
		if (Screen.width == detectedW && Screen.height == detectedH)
		{
			return;
		}
		detectedW = Screen.width;
		detectedH = Screen.height;

		if (pbsMove != null && pbsMove) Destroy ( pbsMove);
		if (pbsFire != null && pbsFire) Destroy ( pbsFire);

		float size = Mathf.Min (Screen.width, Screen.height) * 0.18f;
		
		pbsMove = ProximityButtonSet.Create (size);
		pbsFire = ProximityButtonSet.Create (size);

		float xinset = 1.7f;
		float yinset = 1.3f;

		pbLeft = pbsMove.AddButton ("left", new Vector3 (size, Screen.height - size * yinset));
		pbRight = pbsMove.AddButton ("right", new Vector3 (size * xinset, Screen.height - size));
		
		pbMove = pbsMove.AddButton ("move", new Vector3 (Screen.width - size * xinset, Screen.height - size));
		pbFire = pbsMove.AddButton ("fire", new Vector3 (Screen.width - size, Screen.height - size * yinset));
	}

	void ApplyPBEffect( ProximityButtonSet.ProximityButton pb, GameObject box)
	{
		box.transform.localScale = Vector3.one * (pb.fingerDown ? 1.0f : 0.4f);
	}

	void Update()
	{
		DetectReorientation ();

		ApplyPBEffect (pbLeft, indicators [0]);
		ApplyPBEffect (pbRight, indicators [1]);
		ApplyPBEffect (pbMove, indicators [2]);
		ApplyPBEffect (pbFire, indicators [3]);
	}

	void OnGUI()
	{
		Rect r = new Rect (Screen.width * 0.1f, Screen.height * 0.01f,
		                  Screen.width * 0.8f, Screen.height * 0.30f);

		GUI.Label ( r, "ProximityButtons demo. Works in landscape or protrait.",
		           OurStyles.LABELCJ (14));
	}
}
