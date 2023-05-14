namespace AirHockey
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	// @kurtdekker - just to drive the goal mouth openings and keep track of goal-y stuff.

	public class GoalAndSidewallDriver : MonoBehaviour
	{
		public float NotionalWallThickness = 2.0f;

		public float GoalHeight = 5.0f;

		public Transform TopEdge;
		public Transform BottomEdge;

		public BoxCollider2D GoalBox;

		void Update()
		{
			float halfHeight = GoalHeight / 2;

			TopEdge.transform.localPosition = Vector2.up * halfHeight;
			BottomEdge.transform.localPosition = Vector2.down * halfHeight;

			GoalBox.size = new Vector2( NotionalWallThickness - 1, GoalHeight);
		}
	}
}
