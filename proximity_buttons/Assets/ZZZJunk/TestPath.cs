using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// To use:
// - put in scene with an ILevel25DPath in it
// - drag the capsule around
// - watch the cube follow the path

public class TestPath : MonoBehaviour
{
	ILevel25DPath path;

	GameObject capsule, cube;

	void Start ()
	{
		path = InterfaceHelpers.FindInterfaceOfType<ILevel25DPath>();

		capsule = GameObject.CreatePrimitive( PrimitiveType.Capsule);
		cube = GameObject.CreatePrimitive( PrimitiveType.Cube);
	}

	void Update()
	{
		float along = path.GetDistanceAlongPath( capsule.transform.position);

		Debug.Log( along);

		Vector3 pos = Vector3.zero;
		Vector3 face = Vector3.zero;

		path.GetCurveInfo( along, out pos, out face);

		cube.transform.position = pos;
		cube.transform.forward = face;
	}
}
