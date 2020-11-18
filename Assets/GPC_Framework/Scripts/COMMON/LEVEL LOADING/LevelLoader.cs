using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GPC
{
	[AddComponentMenu("CSharpBookCode/Utility/Level Loader")]

	public class LevelLoader : MonoBehaviour
	{
		public bool loadOnSceneStart;
		public string coreSceneName;
		public string levelSceneToLoad;

		public virtual void Start()
		{
			if (loadOnSceneStart)
				LoadLevel();
		}

		public void LoadLevel()
		{
			LoadLevel(levelSceneToLoad);
		}

		void LoadLevel(string levelSceneName)
		{
			StartCoroutine(LoadAsyncLevels(levelSceneName));
		}

		IEnumerator LoadAsyncLevels(string whichScene)
		{
			Scene loaderScene = SceneManager.GetActiveScene();

			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(whichScene, LoadSceneMode.Additive);

			// Wait until the asynchronous scene fully loads
			while (!asyncLoad.isDone)
			{
				yield return null;
			}

			AsyncOperation asyncLoad2 = SceneManager.LoadSceneAsync(coreSceneName, LoadSceneMode.Additive);

			// Wait until the asynchronous scene fully loads
			while (!asyncLoad2.isDone)
			{
				yield return null;
			}

			// disable the camera before the scene starts to avoid Unity getting stuck in some sort of complaining loop
			// about more than one camera in the scene :(
			Camera.main.gameObject.SetActive(false);

			SceneManager.MergeScenes(SceneManager.GetSceneByName(whichScene), SceneManager.GetSceneByName(coreSceneName));
			SceneManager.SetActiveScene(SceneManager.GetSceneByName(coreSceneName));
			AsyncOperation asyncLoad3 = SceneManager.UnloadSceneAsync(loaderScene);

			// Wait until the asynchronous scene fully unloads
			while (!asyncLoad3.isDone)
			{
				yield return null;
			}

			yield return new WaitForSeconds(1);
		}
	}
}