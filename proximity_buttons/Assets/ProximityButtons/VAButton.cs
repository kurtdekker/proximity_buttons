using UnityEngine;
using System.Collections;

public class VAButton : MonoBehaviour
{
	Texture2D t2d_button_ring;
	Rect r_button_ring;
	Rect r_button_finger;

	public bool doClamp;
	public bool doNormalize;

	public string label;
	public Color labelColor;
	public Rect r_downable;
	public Vector3 outputRaw;	// pre-deadband
	public Vector3 output;
	public float minMagnitude;
	public bool detectedTap;
	public bool fingerDown;

	float beganTime;

	float deflectionRadius;

	public bool constrainFingerCenterWithinRing = true;

	Vector2 v2down;
	int fingerId;

	void Awake()
	{
		labelColor = new Color (0.7f, 0.7f, 0.7f);
	}

	void Start ()
	{
		if (minMagnitude == 0)
		{
			minMagnitude = 0.2f;
		}

		t2d_button_ring = Resources.Load(
			"Textures/uibuttons/20130926_button_ring") as Texture2D;
		float sz = Screen.height * 0.2f;
		r_button_ring = new Rect( 0, 0, sz, sz);
		sz *= 0.7f;
		r_button_finger = new Rect( 0, 0, sz, sz);

		deflectionRadius = sz / 2;
	}
	
	void UpdatePosition( Vector2 pos)
	{
		if (!fingerDown) return;
		
		// axis assignments transform
		output.x = (pos.x - v2down.x) / deflectionRadius;
		output.y = (v2down.y - pos.y) / deflectionRadius;
		output.z = 0;

		if (doNormalize)
		{
			if (output.magnitude >= 1.0f)
			{
				output = output.normalized;
			}
		}
		if (doClamp)
		{
			output.x = Mathf.Clamp( output.x, -1.0f, 1.0f);
			output.y = Mathf.Clamp( output.y, -1.0f, 1.0f);
		}

		outputRaw = output;

		if (output.magnitude <= minMagnitude)
		{
			output = Vector3.zero;
			return;
		}
	}

	void CheckConstraints()
	{
		if (constrainFingerCenterWithinRing)
		{
			Vector2 center;
			center = r_button_finger.center;
			// simple constrain by rectangle, not by radius
			if (center.x < r_button_ring.x)
			{
				center.x = r_button_ring.x;
			}
			if (center.x > r_button_ring.x + r_button_ring.width)
			{
				center.x = r_button_ring.x + r_button_ring.width;
			}
			if (center.y < r_button_ring.y)
			{
				center.y = r_button_ring.y;
			}
			if (center.y > r_button_ring.y + r_button_ring.height)
			{
				center.y = r_button_ring.y + r_button_ring.height;
			}
			r_button_finger.center = center;
		}
	}

	void Update ()
	{
		bool fingerDownNext = false;
		
		output = Vector3.zero;

		MicroTouch[] mts = MicroTouch.GatherMicroTouches();

		foreach (MicroTouch t in mts)
		{
			Vector2 pos = new Vector2( t.position.x, Screen.height - t.position.y);
			
			if (r_downable.Contains( pos))
			{
				if (!fingerDown)
				{
					if (t.phase == TouchPhase.Began)
					{
						beganTime = Time.time;
						fingerDown = true;
						fingerDownNext = true;
						fingerId = t.fingerId;
						v2down = pos;
						r_button_ring.x = v2down.x - r_button_ring.width / 2;
						r_button_ring.y = v2down.y - r_button_ring.height / 2;
						r_button_finger.x = pos.x - r_button_finger.width / 2;
						r_button_finger.y = pos.y - r_button_finger.height / 2;
						UpdatePosition( pos);
						CheckConstraints();
					}
				}
			}
			if (fingerDown)
			{
				if (t.fingerId == fingerId)
				{
					if ((t.phase == TouchPhase.Ended) ||
					    (t.phase == TouchPhase.Canceled))
					{
						if (Time.time - beganTime < 0.2f)
						{
							detectedTap = true;
						}
					}
					else
					{
						fingerDownNext = true;
						UpdatePosition( pos);
						r_button_finger.x = pos.x - r_button_finger.width / 2;
						r_button_finger.y = pos.y - r_button_finger.height / 2;
						CheckConstraints();
					}
				}
			}
		}
		
		fingerDown = fingerDownNext;
	}
	
	void OnGUI()
	{
		if (fingerDown)
		{
			GUI.DrawTexture( r_button_ring, t2d_button_ring);
			GUI.DrawTexture( r_button_finger, t2d_button_ring);
		}
		else
		{
			if (label != null)
			{
				GUI.color = labelColor;
				GUI.Label ( r_downable, label, OurStyles.LABELCJ(10));
			}
		}
	}
}
