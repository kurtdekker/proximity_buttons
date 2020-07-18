using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Car
{
	VAButton vab;

	void CreateVAB()
	{
		if (vab) Destroy(vab);

		vab = gameObject.AddComponent<VAButton>();

		vab.r_downable = MR.SR( 0, 0, 1, 1);
		vab.doClamp = true;
		vab.doNormalize = false;
		vab.minMagnitude = 0.2f;
	}

	void StartMobile()
	{
		CreateVAB();
		OrientationChangeSensor.Create( transform, CreateVAB);
	}

	void FixedUpdateMobile()
	{
		Vector3 output = vab.output;

		if (Input.touches.Length > 1)
		{
			brake = 1.0f;
			return;
		}
			
		if( vab.fingerDown)
		{
			accelerate += output.y;

			steer += output.x;
		}
	}
}
