using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemeAnimator : MonoBehaviour
{
	public float Speed;
	public int Dir4;		// 0 north, 1 east, 2 south, 3 west

	float frameno;

	public Sprite[] Stand4Way;
	public Sprite[] WalkNorth;
	public Sprite[] WalkEast;
	public Sprite[] WalkSouth;
	public Sprite[] WalkWest;

	Sprite[][] Directions;

	SpriteRenderer SR;

	void Start()
	{
		Directions = new Sprite[][] {
			WalkNorth,
			WalkEast,
			WalkSouth,
			WalkWest,
		};

		SR = GetComponent<SpriteRenderer>();
	}

	void Update()
	{
		// designed to cause an error
		int iframe = -1;
		Sprite[] table = null;

		if (Speed == 0)
		{
			iframe = Dir4 & 3;
			table = Stand4Way;
		}
		else
		{
			frameno += Speed;

			iframe = ((int)frameno) & 3;		// 0-3 only

			table = Directions[Dir4];
		}

		SR.sprite = table[iframe];
	}
}
