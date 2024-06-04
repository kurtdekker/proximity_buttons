using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class LineBulletShip : MonoBehaviour
{
	[Header( "Can also be an example left in scene.")]
	public LineBullet BulletPrefab;

	[Header( "Connect both!!!")]
	public Transform PlayerShip;
	public Transform ShipMuzzle;

	void Start ()
	{
		// if in scene, turn it off; (makes it work for prefabs or scene objects)
		if (BulletPrefab.gameObject.activeInHierarchy)
		{
			BulletPrefab.gameObject.SetActive(false);
		}
	}
	
	void Update ()
	{
		UpdatePlayerMovement();
		UpdatePlayerFiring();
	}
}
