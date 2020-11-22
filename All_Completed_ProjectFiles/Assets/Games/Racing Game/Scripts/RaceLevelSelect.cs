using UnityEngine;
using UnityEngine.SceneManagement;
using GPC;

public class RaceLevelSelect : MonoBehaviour
{
	public void LoadLevel(int whichLevel)
	{
		LevelVars.currentLevel = whichLevel;
		LevelVars.levelNamePrefix = "race_level_";
		LevelVars.coreSceneName = "race_core";

		SceneManager.LoadScene("race_sceneLoader");
	}

    public void BackToMenu()
	{
		SceneManager.LoadScene("race_mainMenu");
	}
}
