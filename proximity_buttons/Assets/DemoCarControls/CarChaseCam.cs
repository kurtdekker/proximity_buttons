using UnityEngine;
using System.Collections;

public class CarChaseCam : MonoBehaviour
{
	public Transform target;

	public float DistanceBehind;
	public float DistanceAbove;

	Vector3 lookpoint;
	Vector3 bepoint;

	const float snapBe = 5.0f;
	const float snapLook = 10.0f;

	void FixedUpdate ()
	{
		// where the camera should BE
		Vector3 newBe = target.position + target.forward * DistanceBehind + Vector3.up * DistanceAbove;	

		// where the camera should look
		Vector3 newLook = target.position;

		bepoint = Vector3.Lerp( bepoint, newBe, snapBe * Time.deltaTime);
		lookpoint = Vector3.Lerp( lookpoint, newLook, snapLook * Time.deltaTime);

		transform.position = bepoint;
		transform.LookAt (lookpoint);
	}
}
