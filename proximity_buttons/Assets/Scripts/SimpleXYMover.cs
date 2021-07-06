using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleXYMover : MonoBehaviour
{
	public float Speed;

	void Reset()
	{
		Speed = 10.0f;
	}

	VAButton vab_move;

	void CreateVABs()
	{
		if (vab_move)
		{
			Destroy( vab_move);
		}

		vab_move = gameObject.AddComponent<VAButton>();
		vab_move.r_downable = MR.SR( 0.0f, 0.2f, 1.0f, 0.8f);
		vab_move.doClamp = true;
		vab_move.doNormalize = false;
	}

	void Start()
	{
		CreateVABs();
		OrientationChangeSensor.Create( transform, CreateVABs);
	}

	void AddUnityInputs( ref Vector3 move)
	{
		move.x += Input.GetAxisRaw( "Horizontal");
		move.y += Input.GetAxisRaw( "Vertical");
	}

	void Update ()
	{
		Vector3 move = vab_move.outputRaw;

		AddUnityInputs(ref move);

		move *= Speed;

		transform.position += move * Time.deltaTime;
	}
}
