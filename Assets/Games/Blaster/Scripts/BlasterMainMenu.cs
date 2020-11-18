using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GPC;

public class BlasterMainMenu : MenuWithProfiles
{
	public BaseUserManager _baseUserManager;

	void Awake()
	{
		// set up LevelVars static vars so that the sceneloader knows what it's supposed to be loading
		LevelVars.coreSceneName = "blaster_core";
		LevelVars.currentLevel = 1;
		LevelVars.levelNamePrefix = "blaster_level_";
	}

	// override the load game scene so that it'll load the sceneloader rather than just go for the game
	public override void StartGame()
	{
		if (_baseUserManager == null)
			_baseUserManager = GetComponent<BaseUserManager>();

		// clear out list of players
		_baseUserManager.ResetUsers();

		// add a new player to the game
		_baseUserManager.AddNewPlayer();

		// load the scene loader, without showing the static loading screen it'd do by default
		SceneManager.LoadScene("blaster_sceneLoader");
	}
}
