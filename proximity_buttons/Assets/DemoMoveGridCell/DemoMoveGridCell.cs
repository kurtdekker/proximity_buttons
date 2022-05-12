using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @kurtdekker
// Ultra simple all-in-one-place grid-based mover
//
// To use:
//	- make a blank scene
//	- make a cube at (0,0) in front of the camera
//	- put this script on the cube
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

public class DemoMoveGridCell : MonoBehaviour
{
	[Header( "Leave it blank to move this GameObject")]
	public Transform TransformToMove;

	[Header( "Cell spacing.")]
	public Vector2 Spacing;

	[Header( "Where in world coordinates is cell (0,0)?")]
	public Vector3 WorldOrigin;

	[Header( "How fast do we move?")]
	public float MovementSpeed;

	[Header( "What integer cell are we in?")]
	[Header( "Set this to start off.")]
	// you could use a Vector2Int here if you like
	public Vector2 CellPosition;

	void Reset()
	{
		// defaults
		Spacing = new Vector2( 1, 1);
		MovementSpeed = 5.0f;
	}

	// this maps the cell to the world, however you like
	// you would change this to suit how you like
	Vector3 ComputeWorldPositionFromGridCell( int cellX, int cellY)
	{
		// this is mapping for a traditional 2D layout in X,Y
		return WorldOrigin + new Vector3( cellX * Spacing.x, cellY * Spacing.y, 0);

		// you might use something like this for 3D:
		// this maps X,Y cell to world, in this case X/Z
//		return WorldOrigin + new Vector3( cellX * Spacing.x, 0, cellY * Spacing.y);
	}

	// drives the cell position to the world position
	// returns true if you're done moving and have arrived.
	bool DriveGridCellPositionToWorldPosition( bool instantaneous = false)
	{
		Vector3 worldPosition = ComputeWorldPositionFromGridCell( (int)CellPosition.x, (int)CellPosition.y);

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
		// if you don't set this, we'll move ourselves
		if (!TransformToMove)
		{
			TransformToMove = transform;
		}

		// initial position.
		DriveGridCellPositionToWorldPosition(instantaneous: true);
	}

	void Update ()
	{
		// only consider the next move once we've arrived at the cell we're going to
		if (DriveGridCellPositionToWorldPosition())
		{
			int xmove = 0;
			int ymove = 0;

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

			// process the move, if any
			if (xmove != 0 || ymove != 0)
			{
				Vector2 newCellPosition = CellPosition;

				newCellPosition.x += xmove;
				newCellPosition.y += ymove;

				// TODO: here would be where we check if we can
				// legally move to the proposed new cell location.
				// if we cannot, then assign back the old position

				CellPosition = newCellPosition;
			}
		}
	}
}
