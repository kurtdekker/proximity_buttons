using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoEscapeToMainDemo : MonoBehaviour
{
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene( "DemoProximityButtonsForUI");
		}
	}
}
