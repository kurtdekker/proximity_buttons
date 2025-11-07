using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonToMyGames : MonoBehaviour
{
	void Start()
	{
		Button b = GetComponent<Button>();

		b.onClick.AddListener( delegate {
			//Application.OpenURL("http:/www.plbm.com");
			Application.OpenURL("https://itunes.apple.com/us/app/impulse-one/id1001814482");
			Application.OpenURL("https://play.google.com/store/apps/details?id=com.plbm.impulse1");
		});
	}
}
