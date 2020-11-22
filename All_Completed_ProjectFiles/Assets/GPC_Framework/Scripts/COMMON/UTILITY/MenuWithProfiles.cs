using UnityEngine;
using GPC;

[RequireComponent(typeof(BaseProfileManager))]

public class MenuWithProfiles : BaseMainMenuManager
{
	public BaseProfileManager _profileManager;

	public override void Init()
    {
		_profileManager = GetComponent<BaseProfileManager>();

		// init the profile manager to get preferences from
		_profileManager.Init();

		// need to grab profile volumes before base.Init sets up the UI sliders
		LoadProfileVolumes();

		// now we can run the base init - we've set up the profile info
		base.Init();
	}

	void LoadProfileVolumes()
	{
		// get volume values from profile manager
		_theAudioMixer.SetFloat("SoundVol", _profileManager.GetSFXVolume());
		_theAudioMixer.SetFloat("MusicVol", _profileManager.GetMusicVolume());
	}

	public override void SetSFXVolumeLevel(float aValue)
	{
		float dbVol = Mathf.Log10(aValue) * 20f;
		_theAudioMixer.SetFloat("SoundVol", dbVol);

		// write the volume change to the profile
		_profileManager.SetSFXVolume(dbVol);
	}

	public override void SetMusicVolumeLevel(float aValue)
	{
		float dbVol = Mathf.Log10(aValue) * 20f;
		_theAudioMixer.SetFloat("MusicVol", dbVol);

		// write the volume change to the profile
		_profileManager.SetMusicVolume(dbVol);
	}

	public override void OptionsBackToMainMenu()
	{
		// save updated settings
		_profileManager.SaveProfiles();

		// return to main menu from options
		_canvasManager.LastCanvas();
	}
}
