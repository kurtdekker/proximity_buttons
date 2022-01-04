using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @kurtdekker - makes two random noises, low and high

public class ProceduralEngineNoise : MonoBehaviour
{
	// 0.0 to 1.0f
	public float VolumeControl;

	// 1.0f is nominal
	public float PitchControl = 1.0f;

	float[] NoiseArray;

	// indexes that get moved along the noise array
	float LowRumbleIndex;
	float HighHissIndex;

	const float LowPitchRate = 0.002f;
	const float HighPitchRate = 1.0f;

	const float LowVolumeRatio = 0.3f;
	const float HighVolumeRatio = 0.2f;

	void Awake()
	{
		NoiseArray = new float[44100];

		for (int i = 0; i < NoiseArray.Length; i++)
		{
			NoiseArray[i] = Random.value;
		}
	}

	void Start()
	{
		// causes the OnAudioFilterRead() to get pinged
		gameObject.AddComponent<AudioSource>();
	}

	void OnAudioFilterRead(float[] buffer, int NumChannels)
	{
		int index = 0;

		for (int i = 0; i < buffer.Length; )
		{
			LowRumbleIndex += PitchControl * LowPitchRate;
			HighHissIndex += PitchControl * HighPitchRate;

			if (LowRumbleIndex >= NoiseArray.Length) LowRumbleIndex -= NoiseArray.Length;
			if (HighHissIndex >= NoiseArray.Length) HighHissIndex -= NoiseArray.Length;

			float ThisSample = 0.0f;

			int iLow = (int)LowRumbleIndex;
			int iHigh = (int)HighHissIndex;

			ThisSample += NoiseArray[iLow] * LowVolumeRatio * VolumeControl;

			ThisSample += NoiseArray[iHigh] * HighVolumeRatio * VolumeControl;

			for (int channel = 0; channel < NumChannels; channel++)
			{
				buffer[index] += ThisSample;

				index++;
				i++;
			}
		}
	}
}
