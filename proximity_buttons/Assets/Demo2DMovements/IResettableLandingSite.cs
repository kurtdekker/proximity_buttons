using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResettableLandingSite
{
	int GetIdentifier();

	bool IsPlayerTouching();

	bool IsCaptured();

	void ResetMe();
}
