using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPC
{
	public class Game
	{
		public enum State { idle, loading, loaded, gameStarting, gameStarted, levelStarting, levelStarted, gamePlaying, levelEnding, levelEnded, gameEnding, gameEnded, gamePausing, gameUnPausing, showingLevelResults, showingGameResults, restartingLevel, restartingGame };
	}
}