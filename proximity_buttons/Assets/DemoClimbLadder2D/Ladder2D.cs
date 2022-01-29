using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @kurtdekker - a ladder you can climb in 2D
// TODO: put one of these on each ladder:
//	- make a trigger-marked Collider2D where you want the ladder to "Activate"
//	- make sure the Collider2D is marked as trigger

public class Ladder2D : MonoBehaviour
{
	// catch ALL Rigidbody2Ds tripping into our trigger
	void OnTriggerEnter2D( Collider2D col)
	{
		TrySetPlayerLadder( col, this);
	}
	void OnTriggerExit2D( Collider2D col)
	{
		TrySetPlayerLadder( col, null);
	}

	// see if what walked into us was a "player ladderable"
	// if it was, give it the ladder, or take it away (null);
	void TrySetPlayerLadder( Collider2D col, Ladder2D ladder)
	{
		var player = col.GetComponent<PlayerLadderable>();

		player.SetLadder( ladder);
	}
}
