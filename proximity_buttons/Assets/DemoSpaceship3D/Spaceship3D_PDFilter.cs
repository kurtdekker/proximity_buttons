using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Spaceship3D : MonoBehaviour
{
	PDFilter rollFilter;
	PDFilter pitchFilter;
	PDFilter yawFilter;

	void SetupControlLawFilters()
	{
		float kp = 1.0f;
		float kd = 0.1f;

		rollFilter = new PDFilter(kp, kd);
		pitchFilter = new PDFilter(kp, kd);
		yawFilter = new PDFilter(kp, kd);
	}
}
