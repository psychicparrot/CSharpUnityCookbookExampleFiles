using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace GPC
{
	public class CanvasManager : MonoBehaviour
	{
		public CanvasGroup[] _UICanvasGroups;
		public float fadeDuration = 0.25f;
		public float waitBetweenFadeTime = 0.1f;

		[Space]
		public CanvasGroup _FadeCanvasGroup;

		private int currentGroup;
		private int lastGroup;

		void Awake()
		{
			HideAll();
		}

		public void HideAll()
		{
			currentGroup = 0;
			lastGroup = 0;

			for (int i = 0; i < _UICanvasGroups.Length; i++)
			{
				_UICanvasGroups[i].alpha = 0;
				_UICanvasGroups[i].interactable = false;
				_UICanvasGroups[i].blocksRaycasts = false;
			}
		}

		public void HideCanvas(int indexNum)
		{
			StartCoroutine(FadeCanvasOut(_UICanvasGroups[indexNum], fadeDuration));
		}

		public void HideCanvas(int indexNum, bool doFade)
		{
			if (doFade)
			{
				StartCoroutine(FadeCanvasOut(_UICanvasGroups[indexNum], fadeDuration));
			}
			else
			{
				_UICanvasGroups[indexNum].alpha = 0;
				_UICanvasGroups[indexNum].interactable = false;
				_UICanvasGroups[indexNum].blocksRaycasts = false;
			}
		}

		public void ShowCanvas(int indexNum)
		{
			lastGroup = currentGroup;
			currentGroup = indexNum;

			StartCoroutine(FadeCanvasIn(_UICanvasGroups[indexNum], fadeDuration));
		}

		public void ShowCanvas(int indexNum, bool doFade)
		{
			lastGroup = currentGroup;
			currentGroup = indexNum;

			if (doFade)
			{
				StartCoroutine(FadeCanvasIn(_UICanvasGroups[indexNum], fadeDuration));
			}
			else
			{
				_UICanvasGroups[indexNum].alpha = 1;
				_UICanvasGroups[indexNum].interactable = true;
				_UICanvasGroups[indexNum].blocksRaycasts = true;
			}
		}

		public void LastCanvas()
		{
			TimedSwitchCanvas(currentGroup, lastGroup);
		}

		public void FadeOut(float timeVal)
		{
			StartCoroutine(FadeCanvasIn(_FadeCanvasGroup, timeVal));
		}

		public void FadeIn(float timeVal)
		{
			StartCoroutine(FadeCanvasOut(_FadeCanvasGroup, timeVal));
		}

		public void FaderOn()
		{
			_FadeCanvasGroup.alpha = 1;
		}

		public void FaderOff()
		{
			_FadeCanvasGroup.alpha = 0;
		}

		public void TimedSwitchCanvas(int indexFrom, int indexTo)
		{
			lastGroup = indexFrom;
			currentGroup = indexTo;

			StartCoroutine(StartFadeCanvasSwitch(_UICanvasGroups[indexFrom], _UICanvasGroups[indexTo], fadeDuration, waitBetweenFadeTime));
		}

		public static IEnumerator FadeCanvasIn(CanvasGroup theGroup, float fadeDuration)
		{
			float currentTime = 0;
			float currentAlpha = theGroup.alpha;

			theGroup.interactable = true;
			theGroup.blocksRaycasts = true;
			while (currentTime < fadeDuration)
			{
				currentTime += Time.deltaTime;
				float newAlpha = Mathf.Lerp(currentAlpha, 1, currentTime / fadeDuration);
				theGroup.alpha = newAlpha;
				yield return null;
			}
		}

		public static IEnumerator FadeCanvasOut(CanvasGroup theGroup, float fadeDuration)
		{
			float currentTime = 0;
			float currentAlpha = theGroup.alpha;

			theGroup.interactable = false;
			theGroup.blocksRaycasts = false;

			while (currentTime < fadeDuration)
			{
				currentTime += Time.deltaTime;
				float newAlpha = Mathf.Lerp(currentAlpha, 0, currentTime / fadeDuration);
				theGroup.alpha = newAlpha;
				yield return null;
			}
		}

		public static IEnumerator StartFadeCanvasSwitch(CanvasGroup startGroup, CanvasGroup endGroup, float fadeDuration, float waitTime)
		{
			// fade out old UI
			float currentTime = 0;
			float currentAlpha = startGroup.alpha;

			startGroup.interactable = false;
			startGroup.blocksRaycasts = false;

			while (currentTime < fadeDuration)
			{
				currentTime += Time.deltaTime;
				float newAlpha = Mathf.Lerp(currentAlpha, 0, currentTime / fadeDuration);
				startGroup.alpha = newAlpha;
				yield return null;
			}

			yield return new WaitForSeconds(waitTime);

			// now fade in new UI
			currentTime = 0;
			currentAlpha = endGroup.alpha;

			endGroup.interactable = true;
			endGroup.blocksRaycasts = true;

			while (currentTime < fadeDuration)
			{
				currentTime += Time.deltaTime;
				float newAlpha = Mathf.Lerp(currentAlpha, 1, currentTime / fadeDuration);
				endGroup.alpha = newAlpha;
				yield return null;
			}
		}
	}
}