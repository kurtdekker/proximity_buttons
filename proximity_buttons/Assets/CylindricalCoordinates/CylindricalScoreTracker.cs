using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CylindricalScoreTracker : MonoBehaviour
{
	public static CylindricalScoreTracker Instance { get; private set; }

	const float TargetScore = 25;

	int Score;
	Text text;

	void Awake()
	{
		Instance = this;
	}

	void Start()
	{
		text = GetComponent<Text>();
		AddPoints(0);
	}

	public void AddPoints( int points)
	{
		Score += points;

		text.text = "Score:" + Score + "    (first to " + TargetScore + ")";

		if ((Score < -TargetScore) || (Score >= TargetScore))
		{
			Debug.Log( "GAME OVER: Score: " + Score);
			Debug.Break();
		}
	}
}
