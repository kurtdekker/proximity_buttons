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

public class TwinStickGameManager : MonoBehaviour
{
	public static TwinStickGameManager I {get; private set;}

	public GameObject EnemyExplosion;

	void Awake()
	{
		I = this;
	}

	const float PlayAreaSize = 50;

	public Vector3 PlayerPosition;

	public enum Phase
	{
		PREWAVE,
		PLAYING,
		GAMEOVER,
	}

	float timer;
	Phase phase;

	GameObject[] EnemyPrefabs;

	void Start()
	{
		EnemyExplosion.SetActive( false);

		DSM.GameRunning.bValue = true;
		DSM.Score.iValue = 0;
		DSM.Wave.iValue = 0;

		phase = Phase.PREWAVE;

		{
			GameObject go = GameObject.Find( "EnemyPrefabs");
			EnemyPrefabs = new GameObject[ go.transform.childCount];
			for (int i = 0; i < go.transform.childCount; i++)
			{
				EnemyPrefabs[i] = go.transform.GetChild(0).gameObject;
			}
			go.SetActive( false);
		}

		AllEnemies = new List<GameObject>();
		AllBullets = new List<GameObject>();
	}

	List<GameObject> AllEnemies;

	List<GameObject> AllBullets;

	public void AddBullet( GameObject bullet)
	{
		AllBullets.Add( bullet);
	}

	void SpawnEnemies(int enemyCount)
	{
		float minSpeed = 2.0f + DSM.Wave.iValue * 0.5f;
		float maxSpeed = 2.5f + DSM.Wave.iValue * 0.6f;

		var what = EnemyPrefabs[ DSM.Wave.iValue % EnemyPrefabs.Length];

		for (int i = 0; i < enemyCount; i++)
		{
			var e = Instantiate<GameObject>( what);
			e.SetActive( false);

			// spawn "away" from the player
			if (Mathf.Abs( PlayerPosition.x) > Mathf.Abs( PlayerPosition.z))
			{
				if (PlayerPosition.x < 0)
				{
					e.transform.position = new Vector3( PlayAreaSize / 2, 0,
						Random.Range( -PlayAreaSize / 2, PlayAreaSize / 2));
				}
				else
				{
					e.transform.position = new Vector3( -PlayAreaSize / 2, 0,
						Random.Range( -PlayAreaSize / 2, PlayAreaSize / 2));
				}
			}
			else
			{
				if (PlayerPosition.z < 0)
				{
					e.transform.position = new Vector3(
						Random.Range( -PlayAreaSize / 2, PlayAreaSize / 2),
						0, PlayAreaSize / 2);
				}
				else
				{
					e.transform.position = new Vector3(
						Random.Range( -PlayAreaSize / 2, PlayAreaSize / 2),
						0, -PlayAreaSize / 2);
				}
			}

			float chosenSpeed = Random.Range( minSpeed, maxSpeed);

			// accelerant for later-wave added guys... better get busy man!!
			chosenSpeed += timer / 20;

			Enemy1.Attach( e, chosenSpeed);

			e.SetActive( true);

			AllEnemies.Add( e.gameObject);
		}
	}

	public void GameOver()
	{
		if (DSM.GameRunning.bValue)
		{
			DSM.AudioDeath.Poke();

			DSM.GameRunning.bValue = false;
		}
	}

	float SideFeedInterval
	{
		get
		{
			if (DSM.Wave.iValue > 0)
				return 10.0f / DSM.Wave.iValue;
			return 10.0f;
		}
	}

	void CheckBulletsAgainstEnemies()
	{
		AllEnemies.RemoveAll( x => !x);
		AllBullets.RemoveAll( x => !x);

		for (int i = 0; i < AllBullets.Count; i++)
		{
			var b = AllBullets[i];
			if (b)
			{
				for (int j = 0; j < AllEnemies.Count; j++)
				{
					var e = AllEnemies[j];
					if (b && e)
					{
						float d = (e.transform.position - b.transform.position).sqrMagnitude;
						if (d < 1.2f)
						{
							var ex = Instantiate<GameObject>( EnemyExplosion);
							ex.SetActive( false);
							ex.transform.position = e.transform.position;
							ex.SetActive( true);

							DSM.Score.iValue += DSM.Wave.iValue;

							DSM.AudioHit.Poke();

							Destroy(b);
							Destroy(e);
						}
					}
				}
			}
		}
	}

	void CheckPlayerAgainstBorder()
	{
		if (PlayerPosition.x < -PlayAreaSize / 2)
		{
			GameOver();
		}
		if (PlayerPosition.x >  PlayAreaSize / 2)
		{
			GameOver();
		}
		if (PlayerPosition.z < -PlayAreaSize / 2)
		{
			GameOver();
		}
		if (PlayerPosition.z >  PlayAreaSize / 2)
		{
			GameOver();
		}
	}

	void Update ()
	{
		CheckBulletsAgainstEnemies();

		CheckPlayerAgainstBorder();

		timer += Time.deltaTime;

		switch( phase)
		{
		case Phase.PREWAVE :
			if (timer >= 2.0f)
			{
				timer = 0;
				phase++;
				DSM.Wave.iValue++;

				// how many at start of wave
				int enemyCount = DSM.Wave.iValue * 3 + 2;
				SpawnEnemies( enemyCount);
			}
			break;

		case Phase.PLAYING :
			if (timer >= DSM.Wave.iValue * 10 + 10)
			{
				timer -= SideFeedInterval;

				// how many to sidefeed if you linger
				SpawnEnemies( DSM.Wave.iValue);
			}

			if (AllEnemies.Count <= 0)
			{
				timer = 0;
				phase = Phase.PREWAVE;
			}

			break;

		case Phase.GAMEOVER :
			break;
		}
	}

	void OnUserIntent( Datasack ds)
	{
		switch( ds.Value)
		{
		case "ButtonAgain":
			UnityEngine.SceneManagement.SceneManager.LoadScene(
				UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
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
