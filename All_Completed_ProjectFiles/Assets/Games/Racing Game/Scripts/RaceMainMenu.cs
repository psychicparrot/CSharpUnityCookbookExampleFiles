using GPC;

public class RaceMainMenu : MenuWithProfiles
{
	public override void LoadGameScene()
	{
		AddPlayers();
		base.LoadGameScene();
	}

	void AddPlayers()
	{
		BaseUserManager _baseUserManager = GetComponent<BaseUserManager>();
		_baseUserManager.ResetUsers();

		// there's no players set up so add default ones here
		for (int i = 0; i < 4; i++)
		{
			int playerID = _baseUserManager.AddNewPlayer();
			if (i == 0)
				_baseUserManager.SetType(playerID, 0);
			else
				_baseUserManager.SetType(playerID, 2);
		}
	}
}
