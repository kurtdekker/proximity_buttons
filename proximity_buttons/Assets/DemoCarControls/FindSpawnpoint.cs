using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindSpawnpoint : MonoBehaviour
{
	public string SpawnName = "PlayerSpawn";

	public GameObject Target;

	void Awake()
	{
		Target.SetActive( false);
	}

	IEnumerator Start()
	{
		while( true)
		{
			var sp = GameObject.Find( SpawnName);

			if (sp)
			{
				Target.transform.position = sp.transform.position;
				Target.transform.rotation = sp.transform.rotation;

				Target.SetActive( true);

				break;
			}

			yield return null;
		}
	}
}
