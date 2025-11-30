using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Spaceship3D : MonoBehaviour
{
	bool fireRequested;

	int bulletsPending;
	const int bulletBurstSize = 3;
	const float bulletFiringInterval = 0.2f;
	float bulletFireCooldown;

	int firePointIndex;

	void ProcessShooting()
	{
		if (fireRequested)
		{
			fireRequested = false;
			bulletsPending = bulletBurstSize;
		}

		if (bulletFireCooldown > 0)
		{
			bulletFireCooldown -= Time.deltaTime;
		}

		if (bulletsPending > 0)
		{
			if (bulletFireCooldown <= 0)
			{
				// fire!
				bulletFireCooldown = bulletFiringInterval;

				bulletsPending--;

				// TODO: sound?

				Vector3 firePosition = transform.position;

				firePointIndex++;
				if ((firePointIndex & 1) == 0)
				{
					firePosition += transform.right * +1.0f;
				}
				else
				{
					firePosition += transform.right * -1.0f;
				}

				Bullet.Fire(BulletPrefab, firePosition, transform.forward, 100);
			}
		}
	}
}
