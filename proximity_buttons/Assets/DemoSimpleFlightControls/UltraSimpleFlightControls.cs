using UnityEngine;

// @kurtdekker
//
// Ultra-cheeseball flight control
//
// To use:
//	- stick it on a blank GameObject in your scene
//	- make sure there is a MainCamera in the scene

public class UltraSimpleFlightControls : MonoBehaviour
{
	float speed = 10;
	float roll = -80;
	float pitch = 60;

	Rigidbody rb;

	VAButton vab;

	void CreateVABs()
	{
		if (vab)
		{
			Destroy(vab);
		}

		vab = gameObject.AddComponent<VAButton>();
		vab.r_downable = new Rect( 0.0f, 0.2f, 1.0f, 0.8f);
		vab.doClamp = true;
		vab.doNormalize = false;
	}

	void Start()
	{
		rb = gameObject.AddComponent<Rigidbody>();
		rb.useGravity = false;

		var cam = Camera.main;
		cam.transform.SetParent( transform);
		cam.transform.localPosition = Vector3.zero;
		cam.transform.localRotation = Quaternion.identity;

		CreateVABs();
		OrientationChangeSensor.Create( transform, CreateVABs);
	}

	// inputs
	float xm, ym;

	void GatherKeyboardInput()
	{
		xm += Input.GetAxis("Horizontal");
		ym += Input.GetAxis("Vertical");
	}

	void GatherTouchInput()
	{
		if (vab)
		{
			if (vab.fingerDown)
			{
				var output = vab.output;
				xm += output.x;
				ym += output.y;
			}
		}
	}

	void FixedUpdate ()
	{
		xm = 0;
		ym = 0;

		GatherKeyboardInput();
		GatherTouchInput();

		var rot = rb.rotation;

		rot = rot * Quaternion.Euler( 0, 0, roll * xm * Time.deltaTime);
		rot = rot * Quaternion.Euler( pitch * ym * Time.deltaTime, 0, 0);

		// copied from / to PSFlightPlayer - keep in sync!!
		float turn = -rb.transform.right.y * 8;
		// technically this should be a turn around world coords...
		rot = rot * Quaternion.Euler( 0, turn * Time.deltaTime, 0);

		rb.MoveRotation( rot);

		float step = speed * Time.deltaTime;

		var pos = rb.position + transform.forward * step;

		rb.MovePosition( pos);
	}
}
