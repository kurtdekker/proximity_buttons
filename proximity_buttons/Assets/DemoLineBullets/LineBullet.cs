using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineBullet : MonoBehaviour
{
	[Header( "We need it!")]
	public LineRenderer LR;

	[Header( "If you monkey with this in real time it might be kinda cool...")]
	public float BulletSpeed = 50;

	// 10% beyond the right side of screen, kill each shot off.
	// if your shots are too fast and you see gaps, make this bigger!
	const float killPointXPercent = 1.1f;

	List<Vector3> shotPoints = new List<Vector3>();

	// peew!
	public void AddPoint( Vector3 point)
	{
		shotPoints.Add( point);
	}

	// keep flickering between shots to a minimum
	public void SetMuzzle( Vector3 muzzle)
	{
		lastMuzzle = muzzle;
	}
	Vector3? lastMuzzle;

	void LateUpdate ()
	{
		Vector3 lowerLeft = CornerTransformDriver.Instance.LowerLeft.position;
		Vector3 upperRight = CornerTransformDriver.Instance.UpperRight.position;

		float killPointX = Mathf.LerpUnclamped( lowerLeft.x, upperRight.x, killPointXPercent);

		// cull off right
		shotPoints.RemoveAll( sp => sp.x > killPointX);

		// render 'em:
		int count = shotPoints.Count;

		// one extra for the muzzle gap filler segment while firing!
		if (lastMuzzle != null)
		{
			count++;
		}

		LR.positionCount = count;
		LR.SetPositions( shotPoints.ToArray());

		// we
		if (lastMuzzle != null)
		{
			LR.SetPosition( shotPoints.Count, (Vector3)lastMuzzle);
		}

		// move 'em
		float xMove = BulletSpeed * Time.deltaTime;
		for (int i = 0; i < shotPoints.Count; i++)
		{
			Vector3 position = shotPoints[i];
			position.x += xMove;
			shotPoints[i] = position;
		}

		if (shotPoints.Count == 0)
		{
			Destroy(gameObject);
		}

		lastMuzzle = null;
	}
}
