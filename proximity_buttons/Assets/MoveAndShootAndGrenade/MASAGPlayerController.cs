using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MASAGPlayerController : MonoBehaviour
{
	float PlayerFacing;
	const float PlayerSpeed = 10.0f;

	float CameraFacing;
	Camera Cam;

	VAButton vab;
	ProximityButtonSet pbs;

	ProximityButtonSet.ProximityButton pbShoot;
	ProximityButtonSet.ProximityButton pbGrenade;

	void CreateButtons()
	{
		if (vab)
		{
			Destroy(vab);
		}
		if (pbs)
		{
			Destroy(pbs.gameObject);
		}

		float sz = MR.MINAXIS * 0.5f;

		vab = gameObject.AddComponent<VAButton>();
		vab.r_downable = new Rect( 0, Screen.height - sz, sz, sz);
		vab.label = "move";
		vab.doClamp = true;
		vab.doNormalize = false;

		float thirds = sz / 3;
		float y = Screen.height - sz / 2;

		pbs = ProximityButtonSet.Create(sz);
		pbShoot = pbs.AddButton( "shoot", new Vector2( Screen.width - thirds * 1, y));
		pbGrenade = pbs.AddButton( "shoot", new Vector2( Screen.width - thirds * 2, y));
	}

	Vector3 CameraOffset;
	const float CameraSnappiness = 5.0f;

	void Start()
	{
		Cam = Camera.main;

		CameraOffset = Cam.transform.position - transform.position;

		OrientationChangeSensor.Create( transform, () => {
			CreateButtons();
		});
		CreateButtons();
	}

	bool prevShooting;
	bool prevGrenading;

	void UpdatePlayerControls()
	{
		Vector3 motion = Vector3.zero;
		if (vab.fingerDown)
		{
			motion = new Vector3( vab.outputRaw.x, 0, vab.outputRaw.y);
		}

#if UNITY_EDITOR
		if (Input.GetKey( KeyCode.LeftArrow)) motion.x = -1;
		if (Input.GetKey( KeyCode.RightArrow)) motion.x = 1;
		if (Input.GetKey( KeyCode.UpArrow)) motion.z = 1;
		if (Input.GetKey( KeyCode.DownArrow)) motion.z = -1;
#endif

		motion = Quaternion.Euler( 0, CameraFacing, 0) * motion;

		PlayerFacing = Mathf.Atan2( motion.x, motion.z) * Mathf.Rad2Deg;

		bool shooting = pbShoot.fingerDown;
		bool grenading = pbGrenade.fingerDown;

#if UNITY_EDITOR
		shooting |= Input.GetKey( KeyCode.Alpha1);
		shooting |= Input.GetKey( KeyCode.Space);
		grenading |= Input.GetKey( KeyCode.Alpha2);
		grenading |= Input.GetKey( KeyCode.LeftControl);
#endif

		if (shooting)
		{
			motion = Vector3.zero;
		}

		if (grenading)
		{
			motion = Vector3.zero;
		}

		transform.rotation = Quaternion.Euler( 0, PlayerFacing, 0);

		Vector3 position = transform.position;
		position += motion * PlayerSpeed * Time.deltaTime;
		position.y = 0;
		transform.position = position;

		prevShooting = shooting;
		prevGrenading = grenading;
	}

	void UpdateCamera()
	{
		var newCameraPosition = transform.position + CameraOffset;

		Cam.transform.position = Vector3.Lerp(
			Cam.transform.position, newCameraPosition, CameraSnappiness * Time.deltaTime);

		Cam.transform.LookAt( transform.position);

		CameraFacing = Cam.transform.eulerAngles.y;
	}

	void Update()
	{
		UpdatePlayerControls();
		UpdateCamera();
	}
}
