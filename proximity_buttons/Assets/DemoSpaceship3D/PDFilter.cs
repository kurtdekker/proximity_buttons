
// from https://chat.openai.com/share/9541ed9d-a7f2-4b3e-b156-bb66a2dc0612

public class PDFilter
{
	// Gains
	private float KP;	// Proportional Gain
	private float KD;	// Derivative Gain

	// Internal state
	private float previousError = 0.0f;

	public PDFilter(float kp, float kd)
	{
		KP = kp;
		KD = kd;
	}

	public void SetGains(float kp, float kd)
	{
		KP = kp;
		KD = kd;
	}

	public float Compute(float currentValue, float targetValue, float deltaTime)
	{
		// guard against unstable values
		if (deltaTime < 0.001f)
		{
			return 0;
		}

		float error = targetValue - currentValue;

		// Compute derivative of error. Dividing by deltaTime gives rate of change.
		float derivative = (error - previousError) / deltaTime;

		// Compute output
		float output = KP * error + KD * derivative;

		// Update state for next iteration
		previousError = error;

		return output;
	}
}
