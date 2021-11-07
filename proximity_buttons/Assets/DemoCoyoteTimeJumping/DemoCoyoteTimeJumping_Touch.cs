using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// from this forum post, just curious about this coyote jump stuff and acme anvils.
// https://forum.unity.com/threads/coyote-time-variable-height-jump-with-touch-controls.1192519/

public partial class DemoCoyoteTimeJumping
{
	ProximityButtonSet pbs_Move, pbs_Jump;

	ProximityButtonSet.ProximityButton pbLeft, pbRight, pbJump;

	void CreateProximityButtons()
	{
		if (pbs_Jump) Destroy(pbs_Jump);
		if (pbs_Move) Destroy(pbs_Move);

		var sz = MR.MINAXIS * 0.20f;
		var hsz = sz / 2;

		pbs_Move = ProximityButtonSet.Create( sz);
		pbLeft = pbs_Move.AddButton( "left", new Vector3( hsz, Screen.height - hsz));
		pbRight = pbs_Move.AddButton( "right", new Vector3( hsz + sz, Screen.height - hsz));

		pbs_Jump = ProximityButtonSet.Create( sz);
		pbJump = pbs_Move.AddButton( "jump", new Vector3( Screen.width - hsz, Screen.height - hsz));
	}

	void UpdateAddMobileTouchButtons()
	{
		if (pbs_Jump && pbs_Move)
		{
			if (pbLeft.fingerDown) leftIntent = true;
			if (pbRight.fingerDown) rightIntent = true;
			if (pbJump.fingerTouched) jumpIntent = true;
		}
	}
}
