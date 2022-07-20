using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInformation : MonoBehaviour
{
	[Header( "Cell spacing.")]
	public Vector2 Spacing;

	[Header( "Where in world coordinates is cell (0,0)?")]
	public Vector3 WorldOrigin;

	[Header( "Ultra-simple rectangular bounds containment.")]
	public int MinX;
	public int MaxX;
	public int MinY;
	public int MaxY;

	void Reset()
	{
		// defaults
		Spacing = new Vector2( 1, 1);

		MaxX = +5;
		MinX = -MaxX;
		MaxY = +3;
		MinY = -MaxY;
	}

	// this maps the cell to the world, however you like
	// you would change this to suit how you like
	public Vector3 ComputeWorldPositionFromGridCell( int cellX, int cellY)
	{
		// this is mapping for a traditional 2D layout in X,Y
		return WorldOrigin + new Vector3( cellX * Spacing.x, cellY * Spacing.y, 0);

		// you might use something like this for 3D:
		// this maps X,Y cell to world, in this case X/Z
		//		return WorldOrigin + new Vector3( cellX * Spacing.x, 0, cellY * Spacing.y);
	}

}
