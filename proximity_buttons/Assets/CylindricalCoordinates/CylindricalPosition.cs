using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylindricalPosition : MonoBehaviour
{
	public const float FullDepth = 100;

	// Just a bucket to contain cylindrical position: angle and depth
	// We'll agree 0 to 360
	public float Angle;
	// We'll agree -FullDepth to FullDepth
	public float Depth;
}
