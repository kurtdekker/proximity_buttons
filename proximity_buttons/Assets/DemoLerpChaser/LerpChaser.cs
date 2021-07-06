using UnityEngine;

// @kurtdekker
//
// - make a blank scene, add 2 cubes
// - Put this script on the chaser cube
// - Drag the chasee cube into TargetToChase
// - Pick a Snappiness value
// - Press play
// - Move the chasee (via scene) and observe the chaser

public class LerpChaser : MonoBehaviour
{
	[Header( "Drag a target object in here.")]
	public Transform TargetToChase;

	[Header( "Choose how aggressively it chases.")]
	public float Snappiness;

	void Reset()
	{
		Snappiness = 2.0f;
	}

	void Update ()
	{
		transform.position = Vector3.Lerp(
			transform.position,
			TargetToChase.position,
			Snappiness * Time.deltaTime);	
	}
}
