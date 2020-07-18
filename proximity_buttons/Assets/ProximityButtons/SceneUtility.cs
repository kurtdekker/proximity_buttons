using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SceneUtility
{
	public static IEnumerator LoadAdditionalScenes( string[] scenes)
	{
		for (int i = 0; i < scenes.Length; i++)
		{
			var sceneName = scenes[i];

			UnityEngine.SceneManagement.SceneManager.LoadScene(
				sceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);

			if (i == 0)
			{
				yield return null;

				UnityEngine.SceneManagement.SceneManager.SetActiveScene(
					UnityEngine.SceneManagement.SceneManager.GetSceneByName( sceneName));
			}
		}
	}
}
