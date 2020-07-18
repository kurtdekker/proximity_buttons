using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoCarMasterScene : MonoBehaviour
{
	[Tooltip("First scene will be marked as Active.")]
	public	string[]	ComponentScenesToLoad;

	void Start ()
	{
		StartCoroutine(SceneUtility.LoadAdditionalScenes( ComponentScenesToLoad));
	}
}
