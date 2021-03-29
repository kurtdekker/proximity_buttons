using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamerThrower : MonoBehaviour
{
	public ParticleSystem ps;

	void Update ()
	{
		bool emit = false;

		emit = Input.GetKey( KeyCode.F);

		var em = ps.emission;
		em.enabled = emit;
	}
}
