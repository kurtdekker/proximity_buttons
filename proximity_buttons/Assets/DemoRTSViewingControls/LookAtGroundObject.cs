using UnityEngine;

// @kurtdekker
//
//	An ultra-simple RTS look-at-ground-object script, mimicking the
//	type of camera control in (for instance) World Of Warcraft.
//
// To use:
//	- put this script on the camera
//	- make your own input script (with your favorite input source: keyboard, touch, whatever)
//	- give your input script a reference to this script
//	- make your input script modify the public variables below
//	- profit!

public class LookAtGroundObject : MonoBehaviour
{
	[Header( "Either provide the look Transform here, OR...")]
	public Transform GroundObject;

	[Header( "... feed this with the world focus point.")]
	public Vector3 GroundSpot;

	[Header( "How far back / up from the object (dolly).")]
	[Range( 5, 40)]
	public float Distance = 10;

	[Header( "Heading around the compass")]
	public float Heading;

	[Header( "Regard angle: high is overhead.")]
	[Range( 10, 80)]
	public float Angle = 45;

	[Header( "This can be handy: look at head instead of feet.")]
	public float Raise = 2.0f;

	void LateUpdate ()
	{
		// get the target object if you provided one
		if (GroundObject)
		{
			GroundSpot = GroundObject.position;
		}

		// first around the heading
		Quaternion rot = Quaternion.Euler( 0, Heading, 0);

		// next our regard angle
		rot = rot * Quaternion.Euler( -Angle, 0, 0);

		// and back up vector
		Vector3 backup = rot * Vector3.forward;

		// stare point could be up a bit from the ground
		Vector3 StarePoint = GroundSpot + Vector3.up * Raise;

		// bring it all together
		Vector3 ViewpointPosition = StarePoint + backup * Distance;

		// make it so
		transform.position = ViewpointPosition;
		transform.LookAt( StarePoint);
	}
}
