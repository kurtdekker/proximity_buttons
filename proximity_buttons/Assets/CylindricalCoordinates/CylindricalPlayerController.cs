using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( CylindricalPosition))]
public class CylindricalPlayerController : MonoBehaviour
{
	CylindricalPosition cp;

	// degrees per second
	public float PlayerMoveSpeed = 200.0f;

	public GameObject TemplatePlayerShot;

	void Start ()
	{
		TemplatePlayerShot.SetActive( false);

		cp = GetComponent<CylindricalPosition>();

		cp.Angle = 0;
		cp.Depth = -0.9f * CylindricalPosition.FullDepth;
	}

	void UpdateMovement()
	{
		float moveLeftRight = 0;

		moveLeftRight += Input.GetAxisRaw( "Horizontal");

		float lateralMotion = moveLeftRight * PlayerMoveSpeed;

		cp.Angle += lateralMotion * Time.deltaTime;
	}

	bool prevShoot;
	void UpdateShooting()
	{
		bool Shoot = false;

		if (Input.GetAxis( "Fire1") > 0.5f)
		{
			Shoot = true;
		}

		if (Shoot && !prevShoot)
		{
			var copy = Instantiate<GameObject>(TemplatePlayerShot);
			copy.SetActive( true);
			var ballistic = CylindricalBallisticItem.Attach( copy, cp,
				angleVelocity: 0,		// our shots don't curve!
				depthVelocity: CylindricalPosition.FullDepth * 4);
			copy.AddComponent<TTL>().ageLimit = 0.5f;
		}

		prevShoot = Shoot;
	}

	void Update()
	{
		UpdateMovement();

		UpdateShooting();

		// project us onto the cylinder
		CylindricalMapper.Instance.ProjectCylindricalToWorld3D( cp, transform);
	}
}
