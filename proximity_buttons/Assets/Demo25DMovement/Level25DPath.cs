/*
	The following license supersedes all notices in the source code.

	Copyright (c) 2021 Kurt Dekker/PLBM Games All rights reserved.

	http://www.twitter.com/kurtdekker

	Redistribution and use in source and binary forms, with or without
	modification, are permitted provided that the following conditions are
	met:

	Redistributions of source code must retain the above copyright notice,
	this list of conditions and the following disclaimer.

	Redistributions in binary form must reproduce the above copyright
	notice, this list of conditions and the following disclaimer in the
	documentation and/or other materials provided with the distribution.

	Neither the name of the Kurt Dekker/PLBM Games nor the names of its
	contributors may be used to endorse or promote products derived from
	this software without specific prior written permission.

	THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS
	IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED
	TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
	PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
	HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
	SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
	TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
	PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
	LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
	NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
	SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @kurtdekker
// This is a cheap-and-cheerful linear follower; you can replace it
// with one that follows a Bezier curve if you like, as it is interfaced.

public class Level25DPath : MonoBehaviour, ILevel25DPath
{
	// TODO: lots of room to optimize:
	// - cache and dirty all Transforms
	// - cache and clear total linear along distance
	Vector3[] Points;

	void RefreshPoints( bool force = false)
	{
		if (force || Points == null)
		{
			Points = new Vector3[ transform.childCount];
			for (int i = 0; i < transform.childCount; i++)
			{
				Points[i] = transform.GetChild(i).position;
			}
		}
	}

	void Awake()
	{
		RefreshPoints(true);
	}

	void OnDrawGizmos()
	{
		RefreshPoints( true);

		for (int i = 0; i < Points.Length; i++)
		{
			Gizmos.DrawWireSphere( Points[i], 0.1f);

			if (i < Points.Length - 1)
			{
				Gizmos.DrawLine( Points[i], Points[i+1]);
			}
		}
	}

	public float GetDistanceAlongPath(Vector3 worldPosition)
	{
		int segment = -1;
		float lowestCloseness = 0;
		float lowestAlong = 0;

		float along = 0;
		// in a triangular sense, find which segment we're "closest" to
		for (int i = 0; i < Points.Length - 1; i++)
		{
			var p1 = Points[i];
			var p2 = Points[i + 1];

			var span = Vector3.Distance( p1, p2);

			var d1 = (worldPosition - p1).magnitude;
			var d2 = (worldPosition - p2).magnitude;

			var closeness = (d1 + d2) / span;

			if ((segment < 0) || (closeness < lowestCloseness))
			{
				segment = i;
				lowestCloseness = closeness;
				lowestAlong = along;
			}

			along += span;
		}

		Vector3 pos = MathUtils.NearestPointOnLine( Points[segment], Points[segment + 1], worldPosition);

		return lowestAlong + Vector3.Distance( pos, Points[segment]);
	}

	public void GetCurveInfo (float along, out Vector3 worldPosition, out Vector3 alongVector)
	{
		for (int i = 0; i < Points.Length - 1; i++)
		{
			Vector3 p1 = Points[i];
			Vector3 p2 = Points[i + 1];

			p1.y = 0;
			p2.y = 0;

			float distance = Vector3.Distance( p2, p1);

			if (along < distance)
			{
				float fraction = Mathf.InverseLerp( 0, distance, along);

				worldPosition = Vector3.Lerp( Points[i], Points[i + 1], fraction);

				alongVector = Points[i + 1] - Points[i];
				alongVector.y = 0;

				return;
			}

			along -= distance;
		}

		// second-from-last two points
		int m = Points.Length - 2;
		int n = Points.Length - 1;

		worldPosition = Points[n];
		alongVector = Points[n] - Points[m];
		alongVector.y = 0;
	}

	public float GetTotalLength ()
	{
		float distance = 0;

		for (int i = 0; i < Points.Length - 1; i++)
		{
			distance += Vector3.Distance( Points[i], Points[i + 1]);
		}

		return distance;
	}
}
