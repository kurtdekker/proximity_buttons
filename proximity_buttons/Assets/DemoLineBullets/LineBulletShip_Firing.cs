using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class LineBulletShip : MonoBehaviour
{
	float gunHeat;
	const float fireRate = 0.1f;

	// if nonzero means we have a bullet on its way out
	LineBullet shotInProgress;

	void UpdatePlayerFiring()
	{
		if (gunHeat > 0)
		{
			gunHeat -= Time.deltaTime;
		}

		// input vars
		bool pressed = false;
		bool released = false;


		// TODO: do all your input here
		KeyCode keyCode = KeyCode.Space;
		if (Input.GetKeyDown( keyCode))
		{
			pressed = true;
		}
		if (Input.GetKeyUp( keyCode))
		{
			released = true;
		}
		// done gathering input


		// act!
		if (shotInProgress)
		{
			if (released)
			{
				shotInProgress.AddPoint( ShipMuzzle.position);
				shotInProgress = null;	// shot is done!
			}
		}
		else
		{
			if (pressed)
			{
				// peew!
				gunHeat = 0;
				shotInProgress = Instantiate<LineBullet>( BulletPrefab);
				shotInProgress.gameObject.SetActive(true);
			}
		}

		if (shotInProgress)
		{
			if ( gunHeat <= 0)
			{
				gunHeat = fireRate;
				shotInProgress.AddPoint( ShipMuzzle.position);
			}

			shotInProgress.SetMuzzle( ShipMuzzle.position);
		}
	}
}
