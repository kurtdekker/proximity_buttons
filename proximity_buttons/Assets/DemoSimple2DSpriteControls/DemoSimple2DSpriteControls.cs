using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoSimple2DSpriteControls : MonoBehaviour
{
	MemeAnimator meme;

	void Start ()
	{
		var go = Instantiate<GameObject>( Resources.Load<GameObject>(
			"Textures/MemeMonkeyFight/MemeWalk4Way_Prefab"));

		meme = go.GetComponent<MemeAnimator>();
	}
	
	void Update ()
	{
		var which = (int)(Time.time) & 7;

		meme.Dir4 = which / 2;

		if ((which & 1) != 0)
		{
			meme.Speed = 0.25f;
		}
		else
		{
			meme.Speed = 0.0f;
		}
	}
}
