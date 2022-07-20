using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Gathers grid moving input for the player.
// Typically this interface is called from DemoMoveGridCell.cs

public class PlayerGridInput : MonoBehaviour, IGridMoveInput
{
	public void GetMovement( out int xmove, out int ymove)
	{
		xmove = 0;
		ymove = 0;

		// we only need to change the cell; the above function
		// will notice our world position is different
		if (Input.GetKeyDown( KeyCode.W) ||
			Input.GetKeyDown( KeyCode.UpArrow))
		{
			ymove = +1;
		}
		if (Input.GetKeyDown( KeyCode.S) ||
			Input.GetKeyDown( KeyCode.DownArrow))
		{
			ymove = -1;
		}
		if (Input.GetKeyDown( KeyCode.A) ||
			Input.GetKeyDown( KeyCode.LeftArrow))
		{
			xmove = -1;
		}
		if (Input.GetKeyDown( KeyCode.D) ||
			Input.GetKeyDown( KeyCode.RightArrow))
		{
			xmove = +1;
		}
	}
}
