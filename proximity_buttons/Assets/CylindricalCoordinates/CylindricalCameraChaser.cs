using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylindricalCameraChaser : MonoBehaviour
{
	public Transform Target;

	public Vector3 Offset;

	const float Snappiness = 5.0f;

	void Reset()
	{
		Offset = new Vector3( 0, 0, -5);
	}

	void LateUpdate ()
	{
		if (Target)
		{
			var pos = Target.position + Offset;

			transform.position = Vector3.Lerp( transform.position,
				pos, Snappiness * Time.deltaTime);
		}
	}
}
