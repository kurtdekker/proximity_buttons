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

		if (emit)
		{
			int count = (int)Mathf.Round (Random.Range( 100,150) * Time.deltaTime);
			ps.Emit( count);
		}
	}
}
