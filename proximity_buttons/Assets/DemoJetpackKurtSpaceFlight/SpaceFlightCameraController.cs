using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @kurtdekker
//
// put this on the camera, give it the player
//
// if you want more schmancy camera, install Cinemachine from the Unity package manager!
//

public class SpaceFlightCameraController : MonoBehaviour
{
	public Transform FirstPersonCameraAnchor;
	public Transform ThirdPersonRegardPoint;
	public float DistanceBehind;
	public float DistanceAbove;

	public GameObject VisualGeometry;

	void Reset()
	{
		DistanceBehind = 10.0f;
		DistanceAbove = 3.0f;
	}

	bool wasForcedFirstTime;
	bool drivenOnOff;
	void DriveVisualGeometry( bool onoff)
	{
		if (!wasForcedFirstTime || (onoff != drivenOnOff))
		{
			Renderer[] renderers = VisualGeometry.GetComponentsInChildren<Renderer>();

			foreach( var r in renderers)
			{
				r.enabled = onoff;
			}

			drivenOnOff = onoff;

			wasForcedFirstTime = true;
		}
	}

	void Update()
	{
		bool toggle = false;

		if (Input.GetKeyDown( KeyCode.C))
		{
			toggle = true;
		}

		if (toggle)
		{
			DSM.SpaceFlight.FirstPersonCamera.bToggle();
		}
	}

	void UpdateFirstPerson()
	{
		DriveVisualGeometry(false);

		transform.position = FirstPersonCameraAnchor.position;
		transform.rotation = FirstPersonCameraAnchor.rotation;
	}

	void UpdateThirdPerson()
	{
		DriveVisualGeometry(true);

		Vector3 flattenedForward = ThirdPersonRegardPoint.forward;
		flattenedForward.y = 0;

		var pos = ThirdPersonRegardPoint.position + -flattenedForward.normalized * DistanceBehind + Vector3.up * DistanceAbove;

		transform.position = pos;
		transform.LookAt( ThirdPersonRegardPoint);
	}

	void LateUpdate ()
	{
		if (DSM.SpaceFlight.FirstPersonCamera.bValue)
		{
			UpdateFirstPerson();
		}
		else
		{
			UpdateThirdPerson();
		}
	}
}
