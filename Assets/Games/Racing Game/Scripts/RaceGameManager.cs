using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using GPC;

public class RaceGameManager : BaseGameManager
{
	[Header("Game Specific")]
	public string mainMenuSceneName = "mainMenu";
	public int totalLaps = 3;
	public Transform _playerParent;
	public IsoCamera _cameraScript;
	public GameObject _playerGO1;
	public GameObject[] _playerPrefabList;
	public RaceUIManager _uiManager;
	public BaseSoundManager _soundController;
	public WaypointsController _WaypointController;
	public bool didInit;
	// user / player data
	public BaseUserManager _baseUserManager;
	public List<UserData> _playerInfo;
	private List<Transform> _startPoints;
	private int focusPlayerID;
	private List<RacePlayerController> _playerList;
	private List<Transform> _playerTransforms;
	private bool oldIsWrongWay;
	private bool doneFinalMessage;
	// position checking
	private int theLap;
	private int numberOfRacers;

	public static RaceGameManager instance { get; private set; }

	public RaceGameManager()
	{
		instance = this;
	}

	void Start()
	{
		Init();
	}

	void Init()
	{
		// tell race manager to prepare for the race
		GlobalRaceManager.instance.InitNewRace(totalLaps);

		SetupPlayers();

		// do initial lap counter display
		UpdateLapCounter(1);

		// start the game in 3 seconds from now
		Invoke("StartRace", 4);

		InvokeRepeating("UpdatePositions", 0.5f, 0.5f);

		// hide our count in numbers
		HideCount();

		// schedule count in messages
		Invoke("ShowCount3", 1);
		Invoke("ShowCount2", 2);
		Invoke("ShowCount1", 3);
		Invoke("HideCount", 4);

		// hide final position text
		_uiManager.HideFinalMessage();
		doneFinalMessage = false;

		// start by hiding our wrong way message
		_uiManager.HideWrongWay();

		UpdateWrongWay(false);

		didInit = true;
	}

	void SetupPlayers()
	{
		// if we can't find a ref to user manager, try to find one instead..
		if (_baseUserManager == null)
			_baseUserManager = GetComponent<BaseUserManager>();

		// get player list from user manager
		_playerInfo = _baseUserManager.GetPlayerList();

		if (_playerInfo.Count < 1)
		{
			Debug.Log("USING DEBUG PLAYERS..");

			// there's no players set up so add default ones here
			for (int i = 0; i < 4; i++)
			{
				int playerID = _baseUserManager.AddNewPlayer();
				if (i == 0)
				{
					// if this is the first player, make it human
					_baseUserManager.SetType(playerID, 0);
				}
				else
				{
					// all other players are AI
					_baseUserManager.SetType(playerID, 2);
				}
			}

			// now we can grab that player list from user manager
			_playerInfo = _baseUserManager.GetPlayerList();
		}

		// get number of racers to spawn from the player list
		numberOfRacers = _playerInfo.Count;

		GetStartPoints();
		GetWaypointsController();

		// we are going to use the array full of start positions that must be set in the editor, which means we always need to
		// make sure that there are enough start positions for the number of players

		_playerTransforms = new List<Transform>();
		_playerList = new List<RacePlayerController>();

		for (int i = 0; i < numberOfRacers; i++)
		{
			Transform _newPlayer = Spawn(_playerPrefabList[0].transform, _startPoints[i].position, _startPoints[i].rotation);
			RacePlayerController _raceController =  _newPlayer.GetComponent<RacePlayerController>();

			_newPlayer.parent = _playerParent;

			// assign the user's ID based on the list from baseUserManager
			_raceController.userID = _playerInfo[i].id;

			// store the race controller in our list so we can talk to it as needed later on
			_playerList.Add(_raceController);
			_playerTransforms.Add(_newPlayer);

			// SET UP AI
			BaseAIController _tempAI = _newPlayer.GetComponent<BaseAIController>();

			// tell each player where to find the waypoints
			_tempAI.SetWayController(_WaypointController);
			// tell players race controller, too..
			_raceController._waypointsController = _WaypointController;

			if (_baseUserManager.GetType(_playerInfo[i].id) == 0 && _playerGO1==null)
			{
				// focus on the first vehicle
				_playerGO1 = _newPlayer.gameObject;
				_playerGO1.AddComponent<AudioListener>();
				focusPlayerID = i;
			}

			// SET INPUT TO PLAYER TYPE
			// ---------------------------------------------------------------------
			if (_baseUserManager.GetType(_playerInfo[i].id) == 0) // <-- get the type of this player from baseusermanager
			{
				_newPlayer.GetComponent<RaceInputController>().SetInputType(RaceInputController.InputTypes.player1);

				// tell race controller that this is an AI vehicle
				_raceController.AIControlled = false;

				// set the skin
				_raceController.GetComponent<MaterialSetter>().SetMaterial(0);
			}
			else if (_baseUserManager.GetType(_playerInfo[i].id) == 2)
			{
				_newPlayer.GetComponent<RaceInputController>().SetInputType(RaceInputController.InputTypes.noInput);

				// tell race controller that this is an AI vehicle
				_raceController.AIControlled = true;

				// let the AI controller know to control this vehicle
				_tempAI.AIControlled = true;

				// set the skin
				_raceController.GetComponent<MaterialSetter>().SetMaterial(1);
			}
			// ---------------------------------------------------------------------
		}

		// look at the main camera and see if it has an audio listener attached
		AudioListener tempListener = Camera.main.GetComponent<AudioListener>();

		// if we found a listener, let's destroy it
		if (tempListener != null)
			Destroy(tempListener);

		// tell the camera script to target this new player
		_cameraScript.SetTarget(_playerGO1.GetComponent<CameraTarget>().cameraTarget);

		// lock all the players until we're ready to go
		LockPlayers(true);
	}

	void StartRace()
	{
		// play start race sound
		_soundController.PlaySoundByIndex(1);

		// the SetPlayerLocks function tells all players to unlock
		LockPlayers(false);
	}

	void LockPlayers(bool aState)
	{
		// tell all the players to set their locks
		for (int i = 0; i < numberOfRacers; i++)
		{
			RacePlayerController aRaceCarController = _playerList[i];
			aRaceCarController.SetLock(aState);
		}
	}

	void UpdatePositions()
	{
		// here we need to talk to the race controller to get what we need to display on screen
		theLap = GlobalRaceManager.instance.GetLapsDone(focusPlayerID) + 1;

		// update the display
		UpdateLapCounter(theLap);
	}

	void UpdateLapCounter(int theLap)
	{
		// if we've finished all the laps we need to finish, let's cap the number so that we can
		// have the AI cars continue going around the track without any negative implications
		if (theLap > totalLaps)
			theLap = totalLaps;

		// now we set the text of our GUIText object lap count display
		_uiManager.SetLaps(theLap, totalLaps);
	}

	public void RaceComplete()
	{
		// set all players to AI so that they continue driving
		for (int i = 0; i < _playerList.Count; i++)
		{
			// set to AI
			_playerList[i].AIControlled = true;

			// disable the keyboard input component for AI players..
			_playerList[i].GetComponent<RaceInputController>().SetInputType(RaceInputController.InputTypes.noInput);

		}

		if (!doneFinalMessage)
		{
			doneFinalMessage = true;
			_soundController.PlaySoundByIndex(2);

			// show final text
			_uiManager.ShowFinalMessage();

			// drop out of the race scene completely in 10 seconds...
			Invoke("FinishRace", 10);
		}
	}

	void FinishRace()
	{
		SceneManager.LoadScene(mainMenuSceneName);
	}

	void ShowCount1()
	{
		_uiManager.ShowCount(1);
		_soundController.PlaySoundByIndex(0);
	}
	void ShowCount2()
	{
		_uiManager.ShowCount(2);
		_soundController.PlaySoundByIndex(0);
	}
	void ShowCount3()
	{
		_uiManager.ShowCount(3);
		_soundController.PlaySoundByIndex(0);
	}

	void HideCount()
	{
		_uiManager.ShowCount(0);
	}

	void GetStartPoints()
	{
		_startPoints = new List<Transform>();

		GameObject _startParent = GameObject.Find("StartPoints");
		foreach (Transform sp in _startParent.transform)
		{
			_startPoints.Add(sp);
		}
	}

	void GetWaypointsController()
	{
		_WaypointController = FindObjectOfType<WaypointsController>();
	}

	public void UpdateWrongWay(bool isWrongWay)
	{
		if (isWrongWay == oldIsWrongWay)
			return;

		if (isWrongWay)
		{
			_uiManager.ShowWrongWay();
		}
		else
		{
			_uiManager.HideWrongWay();
		}

		oldIsWrongWay = isWrongWay;
	}

	Transform Spawn(Transform spawnObject, Vector3 spawnPosition, Quaternion spawnRotation)
	{
		return Instantiate(spawnObject, spawnPosition, spawnRotation);
	}
}