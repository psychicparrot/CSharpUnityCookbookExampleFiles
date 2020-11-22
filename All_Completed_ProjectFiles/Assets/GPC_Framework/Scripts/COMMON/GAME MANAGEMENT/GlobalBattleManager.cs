using UnityEngine;
using System.Collections;

namespace GPC
{
	public class GlobalBattleManager
	{
		private int currentID;

		private Hashtable battleControllers;
		private Hashtable battlePositions;
		private Hashtable battleFinished;
		private Hashtable sortedPositions;

		private int numberOfBattlers;

		private int myPos;
		private bool isAhead;
		private BattleController tempRC;
		private BattleController focusPlayerScript;
		private bool battleRunning;

		public static GlobalBattleManager instance;

		public void InitNewBattle()
		{
			instance = this;

			// initialise our hashtables ready for putting objects into
			battlePositions = new Hashtable();
			battleFinished = new Hashtable();
			battleControllers = new Hashtable();
			sortedPositions = new Hashtable();

			currentID = 0;
			numberOfBattlers = 0;
		}

		public int GetUniqueID(BattleController theBattleController)
		{
			// whenever an id is requested, we increment the ID counter. this value never gets reset, so it should always
			// return a unique id (NOTE: these are unique to each game)
			currentID++;

			// now set up some default data for this new player
			if (battlePositions == null)
				InitNewBattle();

			// this player will be in last position
			battlePositions.Add(currentID, battlePositions.Count + 1);

			// store a reference to the battle controller, to talk to later
			battleControllers.Add(currentID, theBattleController);

			// default finished state
			battleFinished[currentID] = false;

			// increment our battler counter so that we don't have to do any counting or lookup whenever we need it
			numberOfBattlers++;

			// pass this id back out for the battle controller to use
			return currentID;
		}

		public void SetBattlePosition(int anID, int aPos)
		{
			if (battlePositions.ContainsKey(anID))
			{
				// we already have an entry in the battle positions table, so let's modify it
				battlePositions[anID] = aPos;
			}
			else
			{
				// we have no data for this player yet, so let's add it to the battlePositions hashtable
				battlePositions.Add(anID, aPos);
			}
		}

		public int GetBattlePosition(int anID)
		{
			// just returns the entry for this ID in the battlePositions hashtable
			return (int)battlePositions[anID];
		}

		private string posList;
		private int whichPos;

		public string GetPositionListString()
		{
			// this function builds a string containing a list of players in order of their scoring positions
			// now we step through each battler and check their positions to determine whether or not
			for (int b = 1; b <= numberOfBattlers; b++)
			{
				whichPos = GetPosition(b);

				tempRC = (BattleController)battleControllers[b];

				sortedPositions[whichPos] = tempRC.GetID();
			}

			if (sortedPositions.Count < numberOfBattlers)
				return "";

			posList = "";

			// now we have a populated sortedPositions hash table, let's iterate through it and build the string
			for (int b = 1; b <= numberOfBattlers; b++)
			{
				whichPos = (int)sortedPositions[b];
				posList = posList + b.ToString() + ". PLAYER " + whichPos + "\n";
			}

			return posList;
		}

		public int GetPosition(int ofWhichID)
		{
			// first, let's make sure that we are ready to go (the hashtables may not have been set up yet, so it's
			// best to be safe and check this first)
			if (battleControllers == null)
			{
				Debug.Log("GetPosition battleControllers is NULL!");
				return -1;
			}

			if (battleControllers.ContainsKey(ofWhichID) == false)
			{
				Debug.Log("GetPosition says no battle controller found for id " + ofWhichID);
				return -1;
			}

			// first, we need to find the player that we're trying to calculate the position of
			focusPlayerScript = (BattleController)battleControllers[ofWhichID];

			// start with the assumption that the player is in last place and work up
			myPos = numberOfBattlers;

			// now we step through each battler and check their positions to determine whether or not
			for (int b = 1; b <= numberOfBattlers; b++)
			{
				// assume that we are behind this player
				isAhead = false;

				// grab a temporary reference to the 'other' player we want to check against
				tempRC = (BattleController)battleControllers[b];

				// if car 2 happens to be null (deleted for example) here's a little safety to skip this iteration in the loop
				if (tempRC == null)
					continue;

				if (focusPlayerScript.GetID() != tempRC.GetID())
				{ // <-- make sure we're not trying to compare same objects!

					// check to see if this player has fragged more
					//if( focusPlayerScript.howMany_fraggedOthers  > tempRC.howMany_fraggedOthers  )
					//	isAhead=true;

					// we check here to see if the frag count is the same and if so we use the id to sort them instead
					if (focusPlayerScript.howMany_fraggedOthers == tempRC.howMany_fraggedOthers && focusPlayerScript.GetID() > tempRC.GetID())
						isAhead = true;

					// alternative version just for fun.. counts fragged times too
					// check to see if this player has fragged more
					if ((focusPlayerScript.howMany_fraggedOthers - focusPlayerScript.howmany_frags) > (tempRC.howMany_fraggedOthers - focusPlayerScript.howmany_frags))
						isAhead = true;

					if (isAhead)
					{
						myPos--;
					}
				}
			}

			return myPos;
		}

		public void StartBattle()
		{
			battleRunning = true;
			UpdateBattleStates();
		}

		public void StopBattle()
		{
			// we don't want to keep calling everyone to tell them about the battle being over if we've already done it once, so check first battleRunning is true
			if (battleRunning == true)
			{
				// set a flag to stop repeat calls etc.
				battleRunning = false;

				// tell everyone about the update to the state of battleRunning
				UpdateBattleStates();

				// tell all players that we're done, by sending a message to each gameObject with a battle controller attached to it
				// to call the PlayerFinishedBattle() function in the car control script
				for (int b = 1; b <= numberOfBattlers; b++)
				{
					tempRC = (BattleController)battleControllers[b];
					tempRC.gameObject.SendMessage("PlayerFinishedBattle", SendMessageOptions.DontRequireReceiver);
				}
			}
		}

		public void RegisterFrag(int whichID)
		{
			focusPlayerScript = (BattleController)battleControllers[whichID];
			focusPlayerScript.FraggedOther();
		}

		void UpdateBattleStates()
		{
			for (int b = 1; b <= numberOfBattlers; b++)
			{
				tempRC = (BattleController)battleControllers[b];
				tempRC.UpdateBattleState(battleRunning);
			}
		}
	}
}