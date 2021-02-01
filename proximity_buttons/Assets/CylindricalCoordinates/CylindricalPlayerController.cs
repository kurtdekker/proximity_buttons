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
		// turn it off; we'll turn on each copy we make
		TemplatePlayerShot.SetActive( false);

		cp = GetComponent<CylindricalPosition>();

		// start at bottom
		cp.Angle = 0;
		// right near nearest edge
		cp.Depth = -0.95f * CylindricalPosition.FullDepth;
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

			// set speed going up the tube
			var ballistic = CylindricalBallisticItem.Attach( copy, cp,
				angleVelocity: 0,		// our shots don't curve!
				depthVelocity: CylindricalPosition.FullDepth * 4);
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
