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

// This is attached to a player when he starts to grip an IGrippable

public class GrippingBridge : MonoBehaviour
{
	IGrippable ig;
	Rigidbody rb1;

	public static GrippingBridge Attach( Rigidbody player, IGrippable ig)
	{
		var bridge = player.GetComponent<GrippingBridge>();

		// if one is on here already, something has gone wrong, so complain
		if (bridge)
		{
			Debug.LogError( "ERROR: player '" + player.name + "' already had a GrippingBridge!");
			return bridge;
		}

		bridge = player.gameObject.AddComponent<GrippingBridge>();

		bridge.rb1 = player;
		bridge.ig = ig;

		bridge.ig.SetGripped(true);

		return bridge;
	}

	public	void		LetGo()
	{
		ig.SetGripped(false);
		Destroy(this);
	}

	public void MyUpdate ()
	{
		var rb2 = ig.GetRigidbody();
		var radius = ig.GetRadius();
		var halfRadius = radius / 2;

		Vector3 gripPosition = rb2.position;

		Vector3 standPosition = rb2.position + rb2.transform.forward * radius;

		// control the player: if you get more than half a radius away from the
		// optimal standpoint, really push you hard back there
		Vector3 playerOffset = standPosition - rb1.position;
		playerOffset.y = 0;
		if (playerOffset.magnitude >= halfRadius)
		{
			rb1.AddForce( playerOffset * 100);

			// drive the object towards the player
			Vector3 objectOffset = rb1.position - gripPosition;
			objectOffset.y = 0;
			if (objectOffset.magnitude >= radius)
			{
				rb2.AddForce( objectOffset * 100);
			}
		}

		// control the carried object

	}
}
