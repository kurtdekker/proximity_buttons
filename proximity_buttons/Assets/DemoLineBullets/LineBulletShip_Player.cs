using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class LineBulletShip : MonoBehaviour
{
	const float playerInsetFractionFromLeft = 0.1f;		// 10% from left to right
	const float playerTopBottomGuard = 0.05f;

	const float playerUpDownSpeed = 20.0f;

	void UpdatePlayerMovement()
	{
		Vector3 lowerLeft = CornerTransformDriver.Instance.LowerLeft.position;
		Vector3 upperRight = CornerTransformDriver.Instance.UpperRight.position;

		float playerX = Mathf.Lerp( lowerLeft.x, upperRight.x, playerInsetFractionFromLeft);

		// input variable
		float yMove = 0;


		// TODO: do all your input here
		yMove += Input.GetAxisRaw( "Vertical");
		// end gathering input


		Vector3 position = PlayerShip.position;

		// align to correct X position
		position.x = playerX;

		// move up down
		position.y += yMove * playerUpDownSpeed * Time.deltaTime;

		// clampamundundoes
		position.y = Mathf.Clamp( position.y,
			Mathf.Lerp( lowerLeft.y, upperRight.y, playerTopBottomGuard),
			Mathf.Lerp( upperRight.y, lowerLeft.y, playerTopBottomGuard));

		PlayerShip.position = position;
	}
}
