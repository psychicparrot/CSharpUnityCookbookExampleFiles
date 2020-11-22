using UnityEngine;
using System;

namespace GPC
{
	public class BaseProfileManager : MonoBehaviour
	{
		[System.NonSerialized]
		public bool didInit;
		public ProfileScriptableObject profileObject;
		public static ProfileData loadedProfile;
		public string profileSaveName = "Game_ProfileData";

		private int numberOfProfileSlots = 3; // you can change this to whatever you want, but for this example we'll only need 3 slots

		public void Init()
		{
			// grab the ScriptableObject from Resources/ScriptableObjects folder
			if(profileObject==null)
				profileObject = Resources.Load("ScriptableObjects/ProfileData") as ProfileScriptableObject;

			string theFileName = Application.persistentDataPath + "/" + profileSaveName + ".json";

			// if we already have saved profiles on disk, load them.. otherwise, we create them!
			if (System.IO.File.Exists(theFileName))
			{
				LoadAllProfiles();
			}
			else
			{
				CreateEmptyProfiles();
			}

			// by default, we'll choose the first profile (we can load another after this, but for now this will be the place to save settings etc.)
			ChooseProfile(0);

			didInit = true;
		}

		public string GetProfileName(int whichSlot)
		{
			return profileObject.theProfileData.profiles[whichSlot].profileName;
		}

		public void ResetProfile(int whichSlot)
		{
			// here, we reset all of the variables in our profile to their default values

			profileObject.theProfileData.profiles[whichSlot].inUse = false;
			profileObject.theProfileData.profiles[whichSlot].musicVolume = 0; // default music and sound to full volume (NOTE: In decibels!)
			profileObject.theProfileData.profiles[whichSlot].sfxVolume = 0;
			profileObject.theProfileData.profiles[whichSlot].profileName = "EMPTY PROFILE";
			profileObject.theProfileData.profiles[whichSlot].highScore = 0;
		}

		public void CreateEmptyProfiles()
		{
			// create an array in the profileObject ScriptableObject, consisting of numberOfProfileSlots x ProfileData objects
			profileObject.theProfileData.profiles = new ProfileData[numberOfProfileSlots];

			// make empty profiles
			for (int i = 0; i < numberOfProfileSlots; i++)
			{
				profileObject.theProfileData.profiles[i] = new ProfileData();
			}

			// save the new empty profile file to disk
			SaveProfiles();
		}

		public void ChooseProfile(int whichSlot)
		{
			// we need to see if this profile is in use yet or not, if not we set it to used and name it
			if (profileObject.theProfileData.profiles[whichSlot].inUse == false)
			{
				profileObject.theProfileData.profiles[whichSlot].profileName = DateTime.Now.ToString();
				profileObject.theProfileData.profiles[whichSlot].inUse = true;
				profileObject.theProfileData.profiles[whichSlot].playerName = "Anonymous";
				profileObject.theProfileData.profiles[whichSlot].musicVolume = 0; // default music and sound to full volume (NOTE: In decibels!)
				profileObject.theProfileData.profiles[whichSlot].sfxVolume = 0;
				profileObject.theProfileData.profiles[whichSlot].highScore = 0;
			}

			// set the loaded profile and put it into a var for easy access
			loadedProfile = profileObject.theProfileData.profiles[whichSlot];

			Debug.Log("CHOSEN PROFILE " + whichSlot);
		}

		public void LoadAllProfiles()
		{
			Debug.Log("LOAD PROFILES!!!!");

			string theFileName = Application.persistentDataPath + "/" + profileSaveName + ".json";
			string jsonString = System.IO.File.ReadAllText(theFileName);

			Profiles tempProfiles = JsonUtility.FromJson<Profiles>(jsonString);
			profileObject.theProfileData = tempProfiles;
		}

		public void SaveProfiles()
		{
			// we never actually just save one profile, we save them all!
			Debug.Log("SAVE PROFILES!!!!");

			string jsonString = JsonUtility.ToJson(profileObject.theProfileData);

			// now write out the file
			string theFileName = Application.persistentDataPath + "/" + profileSaveName + ".json";
			System.IO.File.WriteAllText(theFileName, jsonString);
		}

		public void SetSFXVolume(float aVal)
		{
			loadedProfile.sfxVolume = aVal;
		}

		public void SetMusicVolume(float aVal)
		{
			loadedProfile.musicVolume = aVal;
		}

		public float GetSFXVolume()
		{
			return loadedProfile.sfxVolume;
		}

		public float GetMusicVolume()
		{
			return loadedProfile.musicVolume;
		}

		public bool SetHighScore(int score)
		{
			// if we have a high score, this function returns true.

			// if the incoming score is higher, set the high score
			if (loadedProfile.highScore < score)
			{
				loadedProfile.highScore = score;
				SaveProfiles();
				return true;
			}

			return false;
		}

		public int GetHighScore()
		{
			if (!didInit)
				Init();

			return loadedProfile.highScore;
		}
	}
}