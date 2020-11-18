using UnityEngine.SceneManagement;
using GPC;

public class RunManMainMenu : MenuWithProfiles
{
	BaseUserManager _baseUserManager;

	void Start()
	{
		Init();
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

		// load the main scene
		SceneManager.LoadScene(gameSceneName);
	}
}
