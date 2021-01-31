using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylindricalMapper : MonoBehaviour
{
	// so everybody has access to the current one always
	public static CylindricalMapper Instance {get; private set; }

	void Awake()
	{
		Instance = this;
	}

	// cheap and cheerful way to map a CylindricalPosition
	// into 3D space... this is just a default cylinder

	public float Radius;
	public float Length;

	// this is where all the magic happens
	public void ProjectCylindricalToWorld3D( CylindricalPosition cp, Transform tr)
	{
		// first handle position
		var pos = transform.position;		// center

		// agreed based on setup of scene (+Z cylinder)
		// plus I want zero to be the bottom (down)
		var offset = Vector3.down * Radius;

		// and rotated around the long axis of the cylinder
		offset = Quaternion.Euler( 0, 0, cp.Angle) * offset;

		// move it to the outside
		pos += offset;

		// again, agreed because we are +Z
		var forward = Vector3.forward;

		// normalize apply depth
		forward *= cp.Depth / 100.0f;

		// scale to length of cylinder
		forward *= Length;

		pos += forward;

		tr.position = pos;

		// now to handle rotation
		Quaternion rotation = Quaternion.identity;

		// again , by +Z cylinder agreement
		rotation = Quaternion.Euler( 0, 0, cp.Angle) * rotation;

		tr.rotation = rotation;
	}
}
	