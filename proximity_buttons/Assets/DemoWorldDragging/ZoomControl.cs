using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @kurtdekker - ultra simple zoom control for a camera using Lerp to ease
//
// To use: put on a Camera
//
// For best camera results delete this script and use Cinemachine.

public class ZoomControl : MonoBehaviour
{
	[Header( "Supply this or else we go looking for it:")]
	public Camera cam;

	[Header( "Degrees:")]
	public float WidestFOV = 60;
	public float TightestFOV = 30;

	[Header( "Key to activate tigher zoom")]
	public KeyCode ControlKeycode = KeyCode.Tab;

	[Header( "Larger numbers are snappier")]
	public float Snappiness = 5.0f;

	void Start()
	{
		if (!cam)
		{
			// look for it here
			cam = GetComponent<Camera>();

			if (!cam)
			{
				// look for the main one
				cam = Camera.main;
			}
		}
	}

	void Update ()
	{
		float desiredZoom = WidestFOV;		
		if (Input.GetKey( ControlKeycode))
		{
			desiredZoom = TightestFOV;
		}

		float fov = cam.fieldOfView;

		fov = Mathf.Lerp( fov, desiredZoom, Snappiness * Time.deltaTime);

		cam.fieldOfView = fov;
	}
}
