using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

namespace GPC
{
	[AddComponentMenu("CSharpBookCode/Base/Sound Controller")]

	public class SoundObject
	{
		public AudioSource source;
		public GameObject sourceGO;
		public Transform sourceTR;

		public SoundObject(AudioClip aClip, string aName, float aVolume, AudioMixerGroup theMixer, Transform myParent)
		{
			sourceGO = new GameObject("AudioSource_" + aName);
			sourceGO.transform.parent = myParent;
			sourceTR = sourceGO.transform;
			source = sourceGO.AddComponent<AudioSource>();
			source.outputAudioMixerGroup = theMixer;
			source.playOnAwake = false;
			source.clip = aClip;
			source.volume = aVolume;
			source.maxDistance = 2000;
		}

		public void PlaySound(Vector3 atPosition)
		{
			sourceTR.position = atPosition;
			source.PlayOneShot(source.clip);
		}
	}

	public class BaseSoundManager : MonoBehaviour
	{
		public static BaseSoundManager instance;

		public AudioClip[] GameSounds;
		public AudioMixerGroup theAudioMixerGroup;
		public Transform theListenerTransform;
		private List<SoundObject> soundObjectList;
		private SoundObject tempSoundObj;
		public float volume = 1;

		public void Awake()
		{
			instance = this;
		}

		void Start()
		{
			soundObjectList = new List<SoundObject>();
			foreach (AudioClip theSound in GameSounds)
			{
				soundObjectList.Add(new SoundObject(theSound, theSound.name, volume, theAudioMixerGroup, transform));
			}
		}

		public void PlaySoundByIndex(int anIndexNumber, Vector3 aPosition)
		{
			tempSoundObj = (SoundObject)soundObjectList[anIndexNumber];
			tempSoundObj.PlaySound(aPosition);
		}

		public void PlaySoundByIndex(int anIndexNumber)
		{
			if (theListenerTransform == null)
			{
				AudioListener theListener = FindObjectOfType<AudioListener>();
				if(theListener!=null)
					theListenerTransform = theListener.transform;
			}

			tempSoundObj = (SoundObject)soundObjectList[anIndexNumber];
			if (theListenerTransform != null)
				tempSoundObj.PlaySound(theListenerTransform.position);
			else
				tempSoundObj.PlaySound(Vector3.zero);
		}

		public static IEnumerator StartFadeIn(AudioMixer audioMixer, string exposedParam, float duration)
		{
			float targetVolume = 1;
			float currentTime = 0;
			float currentVol = -80f;

			currentVol = Mathf.Pow(10f, currentVol / 20f);
			float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);

			while (currentTime < duration)
			{
				currentTime += Time.deltaTime;
				float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
				audioMixer.SetFloat(exposedParam, Mathf.Log10(newVol) * 20f);
				yield return null;
			}

			yield break;
		}

		public static IEnumerator StartFadeOut(AudioMixer audioMixer, string exposedParam, float duration)
		{
			float targetVolume = -80;
			float currentTime = 0;
			float currentVol;
			audioMixer.GetFloat(exposedParam, out currentVol);
			currentVol = Mathf.Pow(10f, currentVol / 20f);

			float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);

			while (currentTime < duration)
			{
				currentTime += Time.deltaTime;
				float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
				audioMixer.SetFloat(exposedParam, Mathf.Log10(newVol) * 20f);
				yield return null;
			}

			yield break;
		}

		public static IEnumerator StartFade(AudioMixer audioMixer, string exposedParam, float duration, float targetVolume)
		{
			float currentTime = 0;
			float currentVol;
			audioMixer.GetFloat(exposedParam, out currentVol);
			currentVol = Mathf.Pow(10f, currentVol / 20f);
			float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);

			while (currentTime < duration)
			{
				currentTime += Time.deltaTime;
				float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
				audioMixer.SetFloat(exposedParam, Mathf.Log10(newVol) * 20f);
				yield return null;
			}
			yield break;
		}
	}
}