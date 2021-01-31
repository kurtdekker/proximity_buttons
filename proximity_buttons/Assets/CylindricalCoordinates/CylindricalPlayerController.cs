using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( CylindricalPosition))]
public class CylindricalPlayerController : MonoBehaviour
{
	CylindricalPosition cp;

	// degrees per second
	const float PlayerMoveSpeed = 50.0f;

	void Start ()
	{
		cp = GetComponent<CylindricalPosition>();

		cp.Angle = 0;
		cp.Depth = -0.9f * CylindricalPosition.FullDepth;
	}

	void UpdateMovement()
	{
		float moveLeftRight = 0;

		moveLeftRight += Input.GetAxisRaw( "Horizontal");

		float lateralMotion = moveLeftRight * PlayerMoveSpeed;

		cp.Angle += lateralMotion * Time.deltaTime;
	}

	void Update()
	{
		UpdateMovement();

		// project us onto the cylinder
		CylindricalMapper.Instance.ProjectCylindricalToWorld3D( cp, transform);
	}
}
