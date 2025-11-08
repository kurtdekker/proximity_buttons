using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RToReload : MonoBehaviour
{
	void Reset()
	{
		name = GetType().ToString();
	}

	void DoSceneReload()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(
			UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			DoSceneReload();
		}
	}

	void OnUserIntent( Datasack ds)
	{
		switch( ds.Value)
		{
		case "ButtonReload":
			DoSceneReload();
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
