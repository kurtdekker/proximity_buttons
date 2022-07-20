using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @kurtdekker
//
// A slightly-more-complex grid mover that can accept
// input from various IGridMoveInput objects.
//
// To use:
//	- make a blank scene
//	- make a cube at (0,0) in front of the camera
//	- put this script on the cube
//	- add a script implementing IGridMoveInput to gather input
//		- candidates: PlayerGridInput
//
// DONE!
//
// Possible changes you might want to explore:
//
//	- size of grid spacing (change Spacing)
//
//	- speed of movment (change MovementSpeed)
//
//	- all the other publicly-exposed variables, such as WorldOrigin
//
//	- mapping from cell to world - see comments below
//
//	- allow continuous movement by key holding:
//		- replace Input.GetKeyDown() with Input.GetKey()
//
//	- use physics:
//		- replace direct transform setting with rigidbody.MovePosition
//
//	- turn to face:
//		- set the transform.forward to the direction vector when moving (not when stopped)
//
//	- constrain movement:
//		- have a world controller script with a "Can I move here?" method
//		- connect this script to use that before making each move.

public class DemoMoveGridCell2 : MonoBehaviour
{
	[Header( "Leave it blank to move this GameObject")]
	public Transform TransformToMove;

	[Header( "What grid are we moving upon?")]
	public GridInformation GridInfo;

	[Header( "How fast do we move?")]
	public float MovementSpeed;

	[Header( "What integer cell are we in?")]
	[Header( "Set this to start off.")]
	// you could use a Vector2Int here if you like
	public Vector2 CellPosition;

	void Reset()
	{
		// defaults
		MovementSpeed = 5.0f;
	}

	// Moves the world position when you change
	// the CellPosition in editor mode.
	void OnValidate()
	{
		DriveGridCellPositionToWorldPosition(true);
	}

	// drives the cell position to the world position
	// returns true if you're done moving and have arrived.
	bool DriveGridCellPositionToWorldPosition( bool instantaneous = false)
	{
		// if you don't set this, we'll move ourselves
		if (!TransformToMove)
		{
			TransformToMove = transform;
		}

		// if you don't find a grid, we'll search for it
		if (!GridInfo)
		{
			GridInfo = FindObjectOfType<GridInformation>();
		}

		Vector3 worldPosition = GridInfo.ComputeWorldPositionFromGridCell( (int)CellPosition.x, (int)CellPosition.y);

		Vector3 position = TransformToMove.position;

		if (instantaneous)
		{
			position = worldPosition;
		}

		position = Vector3.MoveTowards( position, worldPosition, MovementSpeed * Time.deltaTime);

		float distance = Vector3.Distance( position, worldPosition);

		TransformToMove.position = position;

		return distance < 0.01f;
	}

	void Start()
	{
		// initial position.
		DriveGridCellPositionToWorldPosition(instantaneous: true);
	}

	void Update ()
	{
		// only consider the next move once we've arrived at the cell we're going to
		if (DriveGridCellPositionToWorldPosition())
		{
			var input = GetComponent<IGridMoveInput>();

			if (input == null)
			{
				Debug.LogError( "Unable to find an IGridMoveInput object on " + name);
				return;
			}
				
			int xmove = 0;
			int ymove = 0;

			input.GetMovement( out xmove, out ymove);

			// process the move, if any
			if (xmove != 0 || ymove != 0)
			{
				Vector2 newCellPosition = CellPosition;

				newCellPosition.x += xmove;
				newCellPosition.y += ymove;

				// ultra-simple rectangular constraint
				if (newCellPosition.x < GridInfo.MinX) newCellPosition.x = GridInfo.MinX;
				if (newCellPosition.x > GridInfo.MaxX) newCellPosition.x = GridInfo.MaxX;
				if (newCellPosition.y < GridInfo.MinY) newCellPosition.y = GridInfo.MinY;
				if (newCellPosition.y > GridInfo.MaxY) newCellPosition.y = GridInfo.MaxY;

				// TODO: here would be where we check if we can
				// legally move to the proposed new cell location.
				// if we cannot, then assign back the old position

				CellPosition = newCellPosition;
			}
		}
	}
}
