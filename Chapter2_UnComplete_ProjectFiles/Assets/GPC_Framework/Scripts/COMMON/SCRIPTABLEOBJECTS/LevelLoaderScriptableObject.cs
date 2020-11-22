using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPC
{
	[CreateAssetMenu(fileName = "UserData", menuName = "CSharpFramework/LevelLoaderScriptableObject")]
	public class LevelLoaderDataObject : ScriptableObject
	{
		public string levelName = "level_1";
		public string friendlyName = "Introduction";
		public string levelDescription = "Welcome to the first level";
	}
}