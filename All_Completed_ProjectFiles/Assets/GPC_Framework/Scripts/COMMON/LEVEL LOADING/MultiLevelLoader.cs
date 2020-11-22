namespace GPC
{
	public class MultiLevelLoader : LevelLoader
	{
		public override void Start()
		{
			// construct a level name from the values held in LevelVars static vars (hopefully set by the main menu at game start)
			levelSceneToLoad = LevelVars.levelNamePrefix + LevelVars.currentLevel.ToString();
			coreSceneName = LevelVars.coreSceneName;
			LoadLevel();
		}
	}
}