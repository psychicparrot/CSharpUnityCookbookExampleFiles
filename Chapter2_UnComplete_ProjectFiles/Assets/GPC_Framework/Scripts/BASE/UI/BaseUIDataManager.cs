using UnityEngine;
using System.Collections;

namespace GPC
{
	[AddComponentMenu("CSharpBookCode/Base/UI Data Manager")]

	public class BaseUIDataManager : MonoBehaviour
	{
		// the actual UI drawing is done by a script deriving from this one		
		public int player_score;
		public int player_lives;
		public int player_highscore;

		public string gamePrefsName = "DefaultGame"; // DO NOT FORGET TO SET THIS IN THE EDITOR!!

		public void UpdateScoreP1(int aScore)
		{
			player_score = aScore;
			if (player_score > player_highscore)
				player_highscore = player_score;
		}

		public void UpdateLivesP1(int alifeNum)
		{
			player_lives = alifeNum;
		}

		public void UpdateScore(int aScore)
		{
			player_score = aScore;
		}

		public void UpdateLives(int alifeNum)
		{
			player_lives = alifeNum;
		}

		public void LoadHighScore()
		{
			// grab high score from prefs
			if (PlayerPrefs.HasKey(gamePrefsName + "_highScore"))
			{
				player_highscore = PlayerPrefs.GetInt(gamePrefsName + "_highScore");
			}
		}

		public void SaveHighScore()
		{
			// as we know that the game is over, let's save out the high score too
			PlayerPrefs.SetInt(gamePrefsName + "_highScore", player_highscore);
		}
	}
}