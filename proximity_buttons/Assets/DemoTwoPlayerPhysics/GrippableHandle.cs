/*
	The following license supersedes all notices in the source code.

	Copyright (c) 2020 Kurt Dekker/PLBM Games All rights reserved.

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

// This basic item goes on a grippable handle on the end of a payload object
//
// This GameObject should go on a handle child GameObject of the payload.
//
// Make sure the transform of this object is "outward" from the payload.
//
// This should have a FixedJoint to the main payload's Rigidbody.

[RequireComponent(typeof(Rigidbody))]
public class GrippableHandle : MonoBehaviour, IGrippable
{
	public float radius = 1.0f;

	// editor visuals
	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere( transform.position, radius);
	}

	// registration/deregistration
	void OnEnable()
	{
		if (Application.isPlaying)
		{
			GrippableManager.Instance.Register(this);
		}
	}

	void Disable()
	{
		if (Application.isPlaying)
		{
			GrippableManager.Instance.Unregister(this);
		}
	}

	#region IGrippable (see docs)

	public float GetRadius ()
	{
		return radius;
	}

	public Rigidbody GetRigidbody ()
	{
		return GetComponent<Rigidbody>();
	}

	public bool IsWithinReach ( Transform player)
	{
		Vector3 delta = player.position - transform.position;
		delta.y = 0;

		float reachRadius = radius;

		return delta.magnitude < reachRadius;
	}

	// if we pick this handle up, nobody else should see it
	bool currentlyGripped;
	public void SetGripped (bool gripped)
	{
		currentlyGripped = gripped;
		if (gripped)
		{
			GrippableManager.Instance.Unregister(this);
			return;
		}
		if (!gripped)
		{
			GrippableManager.Instance.Register(this);
			return;
		}
	}

	public bool IsGripped ()
	{
		return currentlyGripped;
	}

	#endregion IGrippable (see docs)
}
