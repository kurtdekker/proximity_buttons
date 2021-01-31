using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// WARNING: don't add this to things in scene; call Attach()
// below to add it to existing GameObjects.

public class CylindricalBallisticItem : MonoBehaviour
{
	public static CylindricalBallisticItem Attach(
		GameObject go,
		CylindricalPosition sourcePosition,
		float angleVelocity,
		float depthVelocity)
	{
		var cbi = go.AddComponent<CylindricalBallisticItem>();

		// this one is for position, as usual
		cbi.cp = go.AddComponent<CylindricalPosition>();

		// don't copy the CylindricalPosition, that's on another
		// GameObject! Just copy its internal values
		cbi.cp.Angle = sourcePosition.Angle;
		cbi.cp.Depth = sourcePosition.Depth;

		// this one is for velocity
		cbi.velocity = go.AddComponent<CylindricalPosition>();

		cbi.velocity.Angle = angleVelocity;
		cbi.velocity.Depth = depthVelocity;

		cbi.Update();

		return cbi;
	}

	CylindricalPosition cp;

	CylindricalPosition velocity;

	void UpdateMovement()
	{
		// move in cylindrical space
		cp.Depth += velocity.Depth * Time.deltaTime;
		cp.Angle += velocity.Angle * Time.deltaTime;
	}

	void Update()
	{
		UpdateMovement();

		// project us onto the cylinder
		CylindricalMapper.Instance.ProjectCylindricalToWorld3D( cp, transform);
	}
}
