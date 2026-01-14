using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// do not drag this into the scene; it will take care of itself when sites register
//
// watches all sites and when they're all captured, releases them again after a pause.

public class LandingSensorResetter : MonoBehaviour
{
	static LandingSensorResetter _instance;
	public static LandingSensorResetter Register(IResettableLandingSite site)
	{
		if (!_instance)
		{
			_instance = new GameObject("LandingSensorResetter.Register();").AddComponent<LandingSensorResetter>();
		}
		_instance.Add(site);
		return _instance;
	}
	void Add(IResettableLandingSite site)
	{
		if (AllSites == null)
		{
			AllSites = new Dictionary<int, IResettableLandingSite>();
		}
		int id = site.GetIdentifier();
		AllSites.Add(id, site);
	}

	Dictionary<int, IResettableLandingSite> AllSites;
	float AllCapturedTimer;

	// how long after you leave do the bases reset?
	const float AllCapturedReleaseTime = 5.0f;

	private void Update()
	{
		bool considerResetting = true;

		foreach( var kvp in AllSites)
		{
			var site = kvp.Value;

			if (site.IsPlayerTouching())
			{
				considerResetting = false;
				break;
			}

			if (!site.IsCaptured())
			{
				considerResetting = false;
				break;
			}
		}

		if (considerResetting)
		{
			AllCapturedTimer += Time.deltaTime;

			if (AllCapturedTimer >= AllCapturedReleaseTime)
			{
				foreach (var kvp in AllSites)
				{
					var site = kvp.Value;
					site.ResetMe();
				}
			}
		}
		else
		{
			AllCapturedTimer = 0;
		}
	}
}
