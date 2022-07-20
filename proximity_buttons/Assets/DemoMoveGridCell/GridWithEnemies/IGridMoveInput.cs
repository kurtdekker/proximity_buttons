using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGridMoveInput
{
	void GetMovement( out int xmove, out int ymove);
}
