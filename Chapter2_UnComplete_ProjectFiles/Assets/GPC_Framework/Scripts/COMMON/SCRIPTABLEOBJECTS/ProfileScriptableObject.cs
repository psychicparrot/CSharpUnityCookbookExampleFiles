using UnityEngine;
using System;

namespace GPC
{
	[CreateAssetMenu(fileName = "ProfileData", menuName = "CSharpFramework/ProfileScriptableObject")]

	[Serializable]
	public class ProfileScriptableObject : ScriptableObject
	{
		public Profiles theProfileData;
	}

	[Serializable]
	public class Profiles
	{
		public ProfileData[] profiles;
	}

	[Serializable]
	public class ProfileData
	{
		[SerializeField]
		public int myID;
		[SerializeField]
		public bool inUse;

		[SerializeField]
		public string profileName = "EMPTY";
		[SerializeField]
		public string playerName = "Anonymous";

		[SerializeField]
		public int highScore;
		[SerializeField]
		public float sfxVolume;
		[SerializeField]
		public float musicVolume;
	}
}