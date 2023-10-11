using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @kurtdekker - lets things optionally trip
// some type of indication of base status.

public interface IBaseCapturedIndicator
{
	void SetCapturedStatus(bool captured);
}
