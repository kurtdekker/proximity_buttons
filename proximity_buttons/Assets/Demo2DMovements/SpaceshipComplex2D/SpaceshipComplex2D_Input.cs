using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SpaceshipComplex2D
{
	void Update_GatherInput()
	{
		// gather all input

		Vector2 input = Vector2.zero;

		if (Input.GetKey(KeyCode.LeftArrow)) input.x = -1;
		if (Input.GetKey(KeyCode.RightArrow)) input.x = +1;
		if (Input.GetKey(KeyCode.UpArrow))
		{
			engineOn = true;
			input.y = +1;
		}
		if (Input.GetKey(KeyCode.DownArrow)) input.y = -1;

		if (Input.GetKeyDown(KeyCode.Space))
		{
			engineOn = !engineOn;
		}

		float powerLeft = 0;
		float powerRight = 0;

		if (engineOn)
		{
			powerLeft = 1;
			powerRight = 1;

			if (input.y > 0)
			{
				powerLeft += input.y * ClimbAuthority;
				powerRight += input.y * ClimbAuthority;
			}
			if (input.y < 0)
			{
				powerLeft += input.y * DescentAuthority;
				powerRight += input.y * DescentAuthority;
			}

			powerLeft += input.x * TiltAuthority;
			powerRight -= input.x * TiltAuthority;
		}


		// now feed the power into the thruster accumulators

		thrustLeftAccumulator = Mathf.Lerp(
			thrustLeftAccumulator,
			powerLeft,
			IndividualEngineResponsiveness * Time.deltaTime);
		thrustRightAccumulator = Mathf.Lerp(
			thrustRightAccumulator,
			powerRight,
			IndividualEngineResponsiveness * Time.deltaTime);


		// TODO: if no fuel, cut power!


		leftFlame.SetFlame(thrustLeftAccumulator);
		rightFlame.SetFlame(thrustRightAccumulator);
	}
}
