using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProximityButtonSet : MonoBehaviour
{
	public class ProximityButton
	{
		public string label;
		public Vector2 position;
		public bool fingerDown;
		public bool prevFingerDown;

		public ProximityButton( string label, Vector2 position)
		{
			this.label = label;
			this.position = position;
		}
	}

	List<ProximityButton> pbses;

	float diameter;

	public ProximityButton AddButton( string label, Vector2 position)
	{
		ProximityButton pb = new ProximityButton (label, position);
		pbses.Add (pb);
		return pb;
	}

	public static ProximityButtonSet Create( float diameter)
	{
		ProximityButtonSet pbs = new GameObject ("ProximityButtonSet.Create();").
			AddComponent<ProximityButtonSet> ();
		pbs.diameter = diameter;
		pbs.pbses = new List<ProximityButton> ();
		return pbs;
	}
	
	void Update ()
	{
		MicroTouch[] mts = MicroTouch.GatherMicroTouches();

		foreach( ProximityButton pb in pbses)
		{
			pb.prevFingerDown = pb.fingerDown;
			pb.fingerDown = false;
		}

		foreach (MicroTouch t in mts)
		{
			Vector2 pos;
			pos = new Vector2( t.position.x, Screen.height - t.position.y);
		
			ProximityButton pbClosest = null;
			float distClosest = 0;
			foreach( ProximityButton pb in pbses)
			{
				float distance = Vector3.Distance( pb.position, pos);
				if (distance < diameter)
				{
					if ((pbClosest == null) ||
					    (distance < distClosest))
					{
						pbClosest = pb;
						distClosest = distance;
					}
				}
			}
			if (pbClosest != null)
			{
				pbClosest.fingerDown = true;
			}
		}
	}
	
	void OnGUI()
	{
		foreach( ProximityButton pb in pbses)
		{
			GUI.color = new Color( 0.7f, 0.7f, 0.7f, 0.7f);
			Rect r = new Rect( 0, 0, Screen.width * 0.10f, Screen.height * 0.05f);
			r.center = pb.position;
			GUI.Label ( r, pb.label, OurStyles.LABELCJ(10));
		}
	}
}
