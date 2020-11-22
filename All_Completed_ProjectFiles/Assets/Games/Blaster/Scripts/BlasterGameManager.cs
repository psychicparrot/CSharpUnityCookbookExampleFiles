using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GPC;

public class BlasterGameManager : BaseGameManager
{
	public Transform _playerPrefab;
	public Transform _playerSpawnpoint;
	public BaseCameraController _cameraManager;
	public Transform _thePlayer;
	public BlasterPlayer _thePlayerScript;
	public List<BlasterPlayer> _playerScripts;
	public BlasterUIManager _uiManager;
	public ScreenAndAudioFader _fader;
	public int enemyCounter;
	public BaseUserManager _baseUserManager;
	public BaseSoundManager _soundComponent;

	public static BlasterGameManager instance { get; private set; }

	public void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		// start with audio muted and fade covering the screen
		_fader.SetFadedOut();

		// schedule a fade in after a little time 
		Invoke("StartFadeIn", 1f);

		SetTargetState(Game.State.loaded);
	}

	void StartFadeIn()
	{
		_fader.FadeIn();
	}

	void Init()
	{
		// if we're running this in the editor, LevelVars.currentLevel will not have been set to 1 (as it should be at the start)
		// so we just do a little check here so that the game will run OK from anywhere..
		if (LevelVars.currentLevel == 0)
		{
			LevelVars.coreSceneName = "blaster_core";
			LevelVars.levelNamePrefix = "blaster_level_";
			LevelVars.currentLevel = 1;
		}

		CreatePlayers();
		_cameraManager.SetTarget(_thePlayer);

		// now we've done init, we can start the level..
		SetTargetState(Game.State.levelStarting);
	}

	void CreatePlayers()
	{
		// refresh our player script list
		_playerScripts = new List<BlasterPlayer>();

		List<UserData> _playerInfo = _baseUserManager.GetPlayerList();

		// for editor testing..
		if (_playerInfo.Count == 0)
			_baseUserManager.AddNewPlayer();

		// iterate through the player list and add the players to the scene ..
		for (int i = 0; i <_playerInfo.Count; i++)
		{
			// remember to name your spawnpoints correctly! PlayerSpawnPoint_1, PlayerSpawnPoint_2 etc. depending on how many players!
			string spawnPointString = "PlayerSpawnPoint_" + (i + 1).ToString();

			_playerSpawnpoint = GameObject.Find(spawnPointString).transform;

			_thePlayer = Instantiate(_playerPrefab, _playerSpawnpoint.position, _playerSpawnpoint.rotation, transform);

			// grab a reference to the player script through the new transform we just instantiated
			BlasterPlayer _aPlayerScript = _thePlayer.GetComponent<BlasterPlayer>();

			// assign ID to this player from the player list
			_aPlayerScript.SetPlayerDetails(_playerInfo[i].id);

			_playerScripts.Add(_aPlayerScript);

			if(i==0)
				_thePlayerScript = _aPlayerScript;
		}

		// set the score display 
		_uiManager.UpdateScoreUI(_thePlayerScript.GetScore());
		_uiManager.UpdateLivesUI(_thePlayerScript.GetLives());

		BaseCameraController _theCam = FindObjectOfType<BaseCameraController>();
		_theCam.SetTarget(_thePlayerScript.transform);

		// clear out the object..
		_playerInfo = null;
	}

	public void RegisterEnemy()
	{
		enemyCounter++;
	}

	public void EnemyDestroyed()
	{
		// before reacting to a player destroyed, we should check that we're still in game playing mode
		if (currentGameState != Game.State.gamePlaying)
			return;

		enemyCounter--;
		_thePlayerScript.AddScore(150);
		UpdateUIScore(_thePlayerScript.GetScore());

		if (enemyCounter <= 0)
			SetTargetState(Game.State.levelEnding);
	}

	public void UpdateUIScore(int theScore)
	{
		_uiManager.UpdateScoreUI(theScore);
	}

	public void PlayerDestroyed()
	{
		// before reacting to a player destroyed, we should check that we're still in game playing mode
		if (currentGameState != Game.State.gamePlaying)
			return;

		if (_thePlayerScript.GetLives() <= 0)
			SetTargetState(Game.State.gameEnding);
		else
			SetTargetState(Game.State.restartingLevel);
	}

	public void NextLevel()
	{
		// increase the current level counter in the LevelVars static variable, so that the levelloader can use it later
		LevelVars.currentLevel++;

		// check to see which level we're on
		if (LevelVars.currentLevel == 4)
		{
			// they finished the game!
			Invoke("LoadGameCompleteScene", 1f);
		}
		else
		{
			Debug.Log("LOADING NEXT LEVEL..");
			// game isn't completed yet, so..
			// load the sceneLoader scene (which will load the next level)..
			SceneManager.LoadScene("blaster_sceneLoader");
		}
	}

	void CallFadeOut()
	{
		_fader.FadeOut();
	}

	// ---------------------------------------------------------------------------------------
	// GAME STATE FUNCTIONS (CALLED BY THE UPDATETARGETSTATE  / UPDATECURRENTSTATE FUNCTIONS)

	public override void Loaded()
	{
		Init();
		OnLoaded.Invoke();
	}

	public override void LevelStarting()
	{
		OnLevelStarting.Invoke();
		Invoke("LevelStarted", 5f);
	}

	public override void LevelStarted()
	{
		OnLevelStarted.Invoke();

		// now we find all the enemy spawners
		GameObject[] _spawners = GameObject.FindGameObjectsWithTag("EnemySpawner");

		// next, iterate through the list of enemy spawners and send a message to start their spawn timers
		for (int i = 0; i < _spawners.Length; i++)
		{
			_spawners[i].SendMessage("StartSpawnTimer");
		}

		// level is started now, so we can move on to 'playing' state
		SetTargetState(Game.State.gamePlaying);
	}

	public override void LevelEnding()
	{
		OnLevelEnding.Invoke();

		// play level complete sound
		_soundComponent.PlaySoundByIndex(1);

		Invoke("CallFadeOut", 4f);
		Invoke("NextLevel", 6f);
	}

	public override void GameEnding()
	{
		OnGameEnding.Invoke();

		// play level complete sound
		_soundComponent.PlaySoundByIndex(0);

		Invoke("CallFadeOut", 2f);
		Invoke("GameEnded", 3f);
	}

	public override void GameEnded()
	{
		OnGameEnded.Invoke();
		LoadMainMenu();
	}

	public override void RestartLevel()
	{
		Invoke("CallFadeOut", 2f);
		Invoke("LoadSceneLoaderToRestart", 4f);
	}

	void LoadSceneLoaderToRestart()
	{
		SceneManager.LoadScene("blaster_sceneLoader");
	}


	void LoadMainMenu()
	{
		SceneManager.LoadScene("blaster_menu");
	}


	void LoadGameCompleteScene()
	{
		SceneManager.LoadScene("blaster_menu");
	}
}
