/*
	The following license supersedes all notices in the source code.

	Copyright (c) 2019 Kurt Dekker/PLBM Games All rights reserved.

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

public class Enemy1 : MonoBehaviour
{
	float speed;

	public static Enemy1 Attach( GameObject go, float speed)
	{
		var e1 = go.AddComponent<Enemy1>();
		e1.speed = speed;
		return e1;
	}

	int stutter;

	void Update ()
	{
		if (!DSM.GameRunning.bValue) return;

		Vector3 delta = TwinStickGameManager.I.PlayerPosition - transform.position;

		if (delta.magnitude < 1.2f)
		{
			TwinStickGameManager.I.GameOver();
		}

		// continuously face the player
		{
			float angle = Mathf.Rad2Deg * Mathf.Atan2( delta.x, delta.z);
			transform.rotation = Quaternion.Euler( 0, angle, 0);
		}

		if (stutter > 0)
		{
			stutter--;
			return;
		}

		stutter = Random.Range( 4, 10);

		Vector3 motion = delta.normalized * speed * stutter;

		Vector3 pos = transform.position;
		pos += motion * Time.deltaTime;
		pos.y = 0;
		transform.position = pos;
	}
}
