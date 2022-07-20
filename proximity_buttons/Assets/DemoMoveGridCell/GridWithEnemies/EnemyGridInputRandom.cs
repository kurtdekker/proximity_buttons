using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Generates random movement input for enemies.
// Typically this interface is called from DemoMoveGridCell.cs

public class EnemyGridInputRandom : MonoBehaviour, IGridMoveInput
{
	float moveYet;
	float moveInterval;

	void ChooseMoveInterval()
	{
		moveInterval = Random.Range( 0.5f, 1.0f);
	}

	void Start()
	{
		ChooseMoveInterval();
	}

	void Update()
	{
		moveYet += Time.deltaTime;
	}

	public void GetMovement( out int xmove, out int ymove)
	{
		xmove = 0;
		ymove = 0;

		if (moveYet < moveInterval)
		{
			return;
		}

		moveYet = 0;
		ChooseMoveInterval();

		int dir4 = Random.Range( 0, 4);

		switch( dir4)
		{
		case 0 :
			ymove = 1;
			break;
		case 1 :
			xmove = 1;
			break;
		case 2 :
			ymove = -1;
			break;
		case 3 :
			xmove = -1;
			break;
		}
	}
}
