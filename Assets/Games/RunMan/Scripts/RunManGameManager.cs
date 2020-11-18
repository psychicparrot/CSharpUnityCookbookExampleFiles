using UnityEngine;
using UnityEngine.SceneManagement;
using GPC;

[RequireComponent(typeof(BaseProfileManager))]

public class RunManGameManager : BaseGameManager
{
	public RunManUIManager _uiManager;
	public float runSpeed = 1f;
	public float distanceToSpawnPlatform = 20f;
	public bool isRunning;
	public Transform _platformPrefab;	
	public RunManCharacterController _RunManCharacter;

	private float distanceCounter;

	public float playAreaTopY = -0.4f;
	public float playAreaBottomY = 0.2f;
	public float platformStartX = 3f;

	public static RunManGameManager instance { get; private set; }
	
	// set instance in this script's constructor
	public RunManGameManager()
	{
		instance = this;
	}

	public void Start()
	{
		Debug.Log("SetTargetGameState=" + targetGameState);

		SetTargetState(Game.State.loaded);
	}

	public override void UpdateTargetState()
	{
		Debug.Log("targetGameState=" + targetGameState);

		if (targetGameState == currentGameState)
			return;

		switch (targetGameState)
		{
			case Game.State.loaded:
				Loaded();
				break;

			case Game.State.gameStarting:
				GameStarting();
				StartGame();
				break;

			case Game.State.gameStarted:
				// fire the game started event
				GameStarted(); 
				SetTargetState(Game.State.gamePlaying);
				break;

			case Game.State.gamePlaying:
				break;

			case Game.State.gameEnding:
				GameEnding();
				EndGame();
				break;

			case Game.State.gameEnded:
				GameEnded();
				break;
		}

		// IMPORTANT: now update the current state to reflect the change
		currentGameState = targetGameState;
	}

	public override void Loaded()
	{
		base.Loaded();

		// set high score
		_uiManager.SetHighScore(GetComponent<BaseProfileManager>().GetHighScore());

		// reset score
		_RunManCharacter._playerStats.SetScore(0);

		SetTargetState(Game.State.gameStarting);
	}

	void StartGame()
	{
		runSpeed = 0;

		// add a little delay to the start of the game, for people to prepare to run
		Invoke("StartRunning", 2f);
	}

	void StartRunning()
	{
		isRunning = true;
		
		_RunManCharacter.canControl = true;

		runSpeed = 1f;
		distanceCounter = 1f;

		SetTargetState(Game.State.gameStarted);

		// start animation
		_RunManCharacter.StartRunAnimation();

		InvokeRepeating("AddScore", 0.5f, 0.5f);
	}

	void EndGame()
	{
		// stop running
		isRunning = false;
		runSpeed = 0;

		// schedule return to main menu in 4 seconds
		Invoke("ReturnToMenu", 4f);

		// stop the repeating invoke that awards score
		CancelInvoke("AddScore");

		// let's try to store the high score (profile manager takes care of checking if it's a high score or not, we just submit a score)
		if (GetComponent<BaseProfileManager>().SetHighScore(_RunManCharacter._playerStats.GetScore())== true)
		{
			_uiManager.ShowGotHighScore();
		}
		
		// update final score ui
		_uiManager.SetFinalScore(_RunManCharacter._playerStats.GetScore());

		// update target state
		SetTargetState(Game.State.gameEnded);
	}

	void AddScore()
	{
		_RunManCharacter._playerStats.AddScore(1);
	}

	void ReturnToMenu()
	{
		SceneManager.LoadScene("runMan_menu");
	}

	public void PlayerFell()
	{
		SetTargetState(Game.State.gameEnding);
	}

	void SpawnPlatform()
	{
		runSpeed += 0.02f;
		distanceCounter = 0;

		float randomY = Random.Range(playAreaTopY, playAreaBottomY);
		Vector2 startPos = new Vector2(platformStartX, randomY);

		Instantiate(_platformPrefab, startPos, Quaternion.identity);

		distanceCounter = Random.Range(-0.5f, 0.25f);
	}

	void Update()
	{
		if (isRunning)
		{
			distanceCounter += (runSpeed * Time.deltaTime);

			// check to see if we need to spawn another platform (based on distance travelled)
			if ((distanceCounter >= distanceToSpawnPlatform))
				SpawnPlatform();

			_uiManager.SetScore(_RunManCharacter._playerStats.GetScore());
		}
	}
}
