using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyPlatform
{
	public static bool IsMobile { get { return (
		Application.isEditor ||
		Application.platform == RuntimePlatform.Android ||
		Application.platform == RuntimePlatform.IPhonePlayer);
		}}
}
