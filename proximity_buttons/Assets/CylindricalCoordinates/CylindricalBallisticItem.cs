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
		cbi.position = go.AddComponent<CylindricalPosition>();

		// don't copy the CylindricalPosition, that's on another
		// GameObject! Just copy its internal values
		cbi.position.Angle = sourcePosition.Angle;
		cbi.position.Depth = sourcePosition.Depth;

		// this one is for velocity
		cbi.velocity = go.AddComponent<CylindricalPosition>();

		cbi.velocity.Angle = angleVelocity;
		cbi.velocity.Depth = depthVelocity;

		cbi.Update();

		return cbi;
	}

	// optionally supply a target it will check against
	// how close do you have to be?
	const float AngleToHit = 10;
	const float DepthToHit = 10;
	CylindricalPosition target;
	System.Action OnTargetHit;
	public void SetOptionalTargetToHit( CylindricalPosition _target, System.Action _OnTargetHit)
	{
		target = _target;
		OnTargetHit = _OnTargetHit;
	}
	void UpdateCheckTarget()
	{
		if (target)
		{
			float deltaAngle = Mathf.DeltaAngle( position.Angle, target.Angle);
			if (Mathf.Abs( deltaAngle) < AngleToHit)
			{
				float deltaDepth = Mathf.Abs( position.Depth - target.Depth);
				if (deltaDepth < DepthToHit)
				{
					OnTargetHit();
				}
			}
		}
	}

	CylindricalPosition position;

	CylindricalPosition velocity;

	void UpdateMovement()
	{
		// move in cylindrical space
		position.Depth += velocity.Depth * Time.deltaTime;
		position.Angle += velocity.Angle * Time.deltaTime;

		// kill it when it goes 10% beyond tube
		if ((position.Depth >= 1.1f * CylindricalPosition.FullDepth) ||
			(position.Depth <= -1.1f * CylindricalPosition.FullDepth))
		{
			Destroy(gameObject);
		}
	}

	void Update()
	{
		UpdateMovement();

		UpdateCheckTarget();

		// project us onto the cylinder
		CylindricalMapper.Instance.ProjectCylindricalToWorld3D( position, transform);
	}
}
