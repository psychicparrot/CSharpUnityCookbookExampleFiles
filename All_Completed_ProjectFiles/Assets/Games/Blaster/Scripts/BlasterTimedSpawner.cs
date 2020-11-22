using GPC;

public class BlasterTimedSpawner : TimedSpawner
{
	public override void Start()
	{
		// do everything the base script does
		base.Start();

		// tell gamemanager about this enemy
		RegisterEnemy();
	}

	void RegisterEnemy()
	{
		// make sure we have a game manager before doing anything here
		if (BlasterGameManager.instance == null)
		{
			Invoke("RegisterEnemy", 0.1f);
			return;
		}

		// at the start of the game, we need to register this spawner as an enemy before it has spawned
		// so that we have an initial enemy count right away
		BlasterGameManager.instance.RegisterEnemy();
	}
}
