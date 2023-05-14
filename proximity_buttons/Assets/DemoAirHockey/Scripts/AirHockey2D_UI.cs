namespace AirHockey
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;

	public partial class AirHockey2D
	{
		public Text[] PlayerScoreReadouts;

		void UI_DrivePlayerScores()
		{
			for (int i = 0; i < NUMPLAYERS; i++)
			{
				Player player = Players[i];

				string result = System.String.Format( "{0}", player.Score);

				PlayerScoreReadouts[i].text = result;
			}
		}

	}
}
