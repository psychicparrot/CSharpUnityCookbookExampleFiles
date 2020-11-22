using UnityEngine;
namespace GPC
{
	[AddComponentMenu("CSharpBookCode/Utility/Level Vars")]

	public class LevelVars : MonoBehaviour
	{ 
		public static int currentLevel;
		public static string levelNamePrefix = "game_level_";
		public static string coreSceneName = "game_core";
	}
}