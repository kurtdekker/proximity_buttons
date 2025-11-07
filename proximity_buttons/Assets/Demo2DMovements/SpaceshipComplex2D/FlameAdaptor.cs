using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameAdaptor : MonoBehaviour
{
    public ParticleSystem ps;

	ParticleSystem.EmissionModule em;
	ParticleSystem.ShapeModule sh;
	ParticleSystem.MainModule main;
	float baseRateOverTime = 600;

	private void Start()
	{
		main = ps.main;
		em = ps.emission;
		sh = ps.shape;

		SetFlame(0);
	}

	// normalized 0 to 1
	public void SetFlame( float flame)
	{
		float rate = baseRateOverTime * flame;
		em.rateOverTimeMultiplier = rate;

		float radius = 0.02f + 0.25f * flame;
		sh.radius = radius;

		float minSize = 0.10f + 0.30f * flame;
		float maxSize = 0.20f + 0.50f * flame;
		main.startSize = new ParticleSystem.MinMaxCurve(minSize, maxSize);
	}
}
