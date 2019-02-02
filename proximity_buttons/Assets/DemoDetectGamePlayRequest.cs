﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoDetectGamePlayRequest : MonoBehaviour
{
	void OnUserIntent( Datasack ds)
	{
		switch( ds.Value)
		{
		case "ButtonPlayGame1" :
			UnityEngine.SceneManagement.SceneManager.LoadScene( "DemoTwinStickShooter");
			break;
		case "ButtonPlayGame2" :
			UnityEngine.SceneManagement.SceneManager.LoadScene( "DemoMoveAndShootAndGrenade");
			break;
		}
	}

	void OnEnable()
	{
		DSM.UserIntent.OnChanged += OnUserIntent;
	}
	void OnDisable()
	{
		DSM.UserIntent.OnChanged -= OnUserIntent;
	}
}
