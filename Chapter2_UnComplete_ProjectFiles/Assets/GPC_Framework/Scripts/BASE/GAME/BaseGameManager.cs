using UnityEngine;
using UnityEngine.Events;

namespace GPC
{
	[AddComponentMenu("CSharpBookCode/Base/GameManager")]

	public class BaseGameManager : MonoBehaviour
	{
		public Game.State currentGameState;
		public Game.State targetGameState;
		public Game.State lastGameState;

		private bool paused;

		public void SetTargetState(Game.State aState)
		{
			targetGameState = aState;

			if (targetGameState != currentGameState)
				UpdateTargetState();
		}

		public Game.State GetCurrentState()
		{
			return currentGameState;
		}

		[Header("Game Events")]

		public UnityEvent OnLoaded;

		public UnityEvent OnGameStarting;
		public UnityEvent OnGameStarted;

		public UnityEvent OnLevelStarting;
		public UnityEvent OnLevelStarted;
		public UnityEvent OnLevelEnding;
		public UnityEvent OnLevelEnded;

		public UnityEvent OnGameEnding;
		public UnityEvent OnGameEnded;
		public UnityEvent OnGamePause;
		public UnityEvent OnGameUnPause;

		public UnityEvent OnShowLevelResults;
		public UnityEvent OnShowGameResults;

		public UnityEvent OnRestartLevel;
		public UnityEvent OnRestartGame;

		public virtual void Loaded() { OnLoaded.Invoke(); }
		public virtual void GameStarting() { OnGameStarting.Invoke(); }
		public virtual void GameStarted() { OnGameStarted.Invoke(); }
		public virtual void LevelStarting() { OnLevelStarting.Invoke(); }
		public virtual void LevelStarted() { OnLevelStarted.Invoke(); }
		public virtual void LevelEnding() { OnLevelEnding.Invoke(); }
		public virtual void LevelEnded() { OnLevelEnded.Invoke(); }
		public virtual void GameEnding() { OnGameEnding.Invoke(); }
		public virtual void GameEnded() { OnGameEnded.Invoke(); }
		public virtual void GamePause() { OnGamePause.Invoke(); }
		public virtual void GameUnPause() { OnGameUnPause.Invoke(); }
		public virtual void ShowLevelResults() { OnShowLevelResults.Invoke(); }
		public virtual void ShowGameResults() { OnShowGameResults.Invoke(); }
		public virtual void RestartLevel() { OnRestartLevel.Invoke(); }
		public virtual void RestartGame() { OnRestartGame.Invoke(); }

		public virtual void UpdateTargetState()
		{
			// we will never need to run target state functions if we're already in this state, so we check for that and drop out if needed
			if (targetGameState == currentGameState)
				return;

			switch (targetGameState)
			{
				case Game.State.idle:
					break;

				case Game.State.loading:
					break;

				case Game.State.loaded:
					Loaded();
					break;

				case Game.State.gameStarting:
					GameStarting();
					break;

				case Game.State.gameStarted:
					GameStarted();
					break;

				case Game.State.levelStarting:
					LevelStarting();
					break;

				case Game.State.levelStarted:
					LevelStarted();
					break;

				case Game.State.gamePlaying:
					break;

				case Game.State.levelEnding:
					LevelEnding();
					break;

				case Game.State.levelEnded:
					LevelEnded();
					break;

				case Game.State.gameEnding:
					GameEnding();
					break;

				case Game.State.gameEnded:
					GameEnded();
					break;

				case Game.State.gamePausing:
					GamePause();
					break;

				case Game.State.gameUnPausing:
					GameUnPause();
					break;

				case Game.State.showingLevelResults:
					ShowLevelResults();
					break;

				case Game.State.showingGameResults:
					ShowGameResults();
					break;

				case Game.State.restartingLevel:
					RestartLevel();
					break;

				case Game.State.restartingGame:
					RestartGame();
					break;
			}

			// now update the current state to reflect the change
			currentGameState = targetGameState;
		}

		public virtual void UpdateCurrentState()
		{
			switch (currentGameState)
			{
				case Game.State.idle:
					break;

				case Game.State.loading:
					break;

				case Game.State.loaded:
					break;

				case Game.State.gameStarting:
					break;

				case Game.State.gameStarted:
					break;

				case Game.State.levelStarting:
					break;

				case Game.State.levelStarted:
					break;

				case Game.State.gamePlaying:
					break;

				case Game.State.levelEnding:
					break;

				case Game.State.levelEnded:
					break;

				case Game.State.gameEnding:
					break;

				case Game.State.gameEnded:
					break;

				case Game.State.gamePausing:
					break;

				case Game.State.gameUnPausing:
					break;

				case Game.State.showingLevelResults:
					break;

				case Game.State.showingGameResults:
					break;

				case Game.State.restartingLevel:
					break;

				case Game.State.restartingGame:
					break;

			}
		}

		public virtual void GamePaused()
		{
			OnGamePause.Invoke();
			Paused = true;
		}

		public virtual void GameUnPaused()
		{
			OnGameUnPause.Invoke();
			Paused = false;
		}

		public bool Paused
		{
			get
			{
				// get paused
				return paused;
			}
			set
			{
				// set paused 
				paused = value;

				if (paused)
				{
					// pause time
					Time.timeScale = 0f;
				}
				else
				{
					// unpause Unity
					Time.timeScale = 1f;
				}
			}
		}

	}
}