namespace AirHockey
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	// @kurtdekker - gets placed on goals to report objects enteringthem.
	//
	// test: how to tell "is this the ball?"
	//
	// action: what to do when it is
	//

	public class Trigger2DForwarder : MonoBehaviour
	{
		System.Func<GameObject,bool> test;
		System.Action action;

		public static Trigger2DForwarder Attach(
			GameObject trigger,
			System.Func<GameObject,bool> test,
			System.Action action)
		{
			var tf = trigger.AddComponent<Trigger2DForwarder>();

			tf.test = test;
			tf.action = action;

			return tf;
		}

		void OnTriggerEnter2D( Collider2D col)
		{
			if (test( col.gameObject))
			{
				action();
			}
		}
	}

}
