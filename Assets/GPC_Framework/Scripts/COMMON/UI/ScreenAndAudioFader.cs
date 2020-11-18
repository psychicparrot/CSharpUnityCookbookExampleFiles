using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using GPC;

public class ScreenAndAudioFader : MonoBehaviour
{
	public AudioMixer _theMixer;
	public CanvasManager _canvasManager;

	public float fadeTime = 1f;
	public string volumeGroupName = "MasterVol";

	public void SetFadedOut()
	{
		_canvasManager.FaderOn();
		_theMixer.SetFloat(volumeGroupName, -80f);
	}

	public void ClearFade()
	{
		_canvasManager.FaderOff();
		StartCoroutine(BaseSoundManager.StartFade(_theMixer, volumeGroupName, 0.001f, 0f));
	}

	public void Mute()
	{
		StartCoroutine(BaseSoundManager.StartFade(_theMixer, volumeGroupName, 0.001f, -80f));
	}

	public void FadeIn()
	{
		StartCoroutine(BaseSoundManager.StartFadeIn(_theMixer, volumeGroupName, fadeTime));

		_canvasManager.FaderOn(); 
		_canvasManager.FadeIn(fadeTime);
	}

	public void FadeOut()
	{
		Debug.Log("FADE OUT!");
		StartCoroutine(BaseSoundManager.StartFadeOut(_theMixer, volumeGroupName, fadeTime));
		_canvasManager.FadeOut(fadeTime);
	}
}
