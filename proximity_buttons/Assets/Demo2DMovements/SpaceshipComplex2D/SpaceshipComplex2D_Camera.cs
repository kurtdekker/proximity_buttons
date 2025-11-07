using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SpaceshipComplex2D
{
	private void LateUpdate_Camera()
	{
		var cam = Camera.main;

		cam.orthographicSize = 30.0f;

		const float Snappiness = 8.0f;

		Vector3 cameraPosition = cam.transform.position;

		// watch the middle body
		var body = Bodies[Bodies.Count / 2];

		Vector3 playerPosition = body.position;

		playerPosition.z = cameraPosition.z;

		cameraPosition = Vector3.Lerp(cameraPosition, playerPosition, Snappiness * Time.deltaTime);

		cam.transform.position = cameraPosition;
	}

}
