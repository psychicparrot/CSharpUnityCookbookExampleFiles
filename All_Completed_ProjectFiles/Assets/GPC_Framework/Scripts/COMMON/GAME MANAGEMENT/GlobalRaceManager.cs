using UnityEngine;
using System.Collections;

namespace GPC
{
	public class GlobalRaceManager : MonoBehaviour
	{
		public int totalLaps;
		public bool raceAllDone;
		public int racersFinished;
		private int currentID;
		private Hashtable raceControllers;
		private Hashtable racePositions;
		private Hashtable raceLaps;
		private Hashtable raceFinished;
		private int numberOfRacers;
		private int myPos;
		private bool isAhead;
		private RaceController _tempRC;

		public static GlobalRaceManager instance;

		public void Awake()
		{
			if (instance != null)
				Destroy(instance);
			instance = this;

			// we set this to -1 so that the first returned id will be 0 (since it gets incremented on request)
			currentID = -1;
		}

		public void InitNewRace(int howManyLaps)
		{
			// initialise our hashtables
			racePositions = new Hashtable();
			raceLaps = new Hashtable();
			raceFinished = new Hashtable();
			raceControllers = new Hashtable();
			raceAllDone = false;
			totalLaps = howManyLaps;
		}

		public int GetUniqueID(RaceController theRaceController)
		{
			// whenever an id is requested, we increment the ID counter. this value never gets reset, so it should always
			// return a unique id
			currentID++;

			// now set up some default data for this new player

			// this player will be on its first lap
			raceLaps.Add(currentID, 1);

			// this player will be in last position
			racePositions.Add(currentID, racePositions.Count);

			// store a reference to the race controller, to talk to later
			raceControllers.Add(currentID, theRaceController);

			// default finished state
			raceFinished[currentID] = false;

			// increment our racer counter so that we don't have to do any counting or lookup whenever we need it
			numberOfRacers++;

			// pass this id back out for the race controller to use
			return currentID;
		}
		public int GetRacePosition(int anID)
		{
			// just returns the entry for this ID in the racePositions hashtable
			return (int)racePositions[anID];
		}
		public int GetLapsDone(int anID)
		{
			// just returns the entry for this ID in the raceLaps hashtable
			return (int)raceLaps[anID];
		}
		public void CompletedLap(int anID)
		{
			// if should already have an entry in race laps, let's just increment it
			raceLaps[anID] = (int)raceLaps[anID] + 1;

			// here, we check to see if this player has finished the race or not (by checking its entry in
			// raceLaps against our totalLaps var) and if it has finished, we set its entry in raceFinished hashtable
			// to true. note that we always have to declare the object's type when we get it from the hashtable, since
			// hashtables store objects of any type and the system doesn't know what they are unless we tell it!
			if ((int)raceLaps[anID] == totalLaps && (bool)raceFinished[anID]!=true)
			{
				raceFinished[anID] = true;
				racersFinished++;
				// tell the race controller for this ID that it is finished racing
				_tempRC = (RaceController)raceControllers[anID];
				_tempRC.RaceFinished();
			}

			// check to see if the race is all done and everyone has finished (all players)
			Debug.Log("Player "+anID+" reached the finish. " + racersFinished + " players have finished the race so far.");

			if (racersFinished == raceFinished.Count)
				raceAllDone = true;
		}
		public void ResetLapCount(int anID)
		{
			// if there's ever a need to restart the race and reset laps for this player, we reset its entry
			// in the raceLaps hashtable here
			raceLaps[anID] = 0;
		}
		public int GetPosition(RaceController focusPlayerScript)
		{ 
			// start with the assumption that the player is in last place and work up
			myPos = numberOfRacers;

			// now we step through each racer and check their positions to determine whether or not
			// our focussed player is in front of them or not
			for (int b = 0; b <= numberOfRacers; b++)
			{
				// assume that we are behind this player
				isAhead = false;

				// grab a temporary reference to the 'other' player we want to check against
				_tempRC = (RaceController)raceControllers[b];

				if (_tempRC == null)
					continue;

				if (focusPlayerScript != _tempRC)
				{ // <-- make sure we're not trying to compare same objects!

					// is the focussed player a lap ahead?
					if (focusPlayerScript.GetCurrentLap() > _tempRC.GetCurrentLap())
						isAhead = true;

					// is the focussed player on the same lap, but at a higher waypoint number?
					if (focusPlayerScript.GetCurrentLap() == _tempRC.GetCurrentLap() && focusPlayerScript.GetCurrentWaypointNum() > _tempRC.GetCurrentWaypointNum() && !_tempRC.IsLapDone())
						isAhead = true;

					// have both players finished the lap, but ours a higher waypoint number?
					if (focusPlayerScript.GetCurrentLap() == _tempRC.GetCurrentLap() && focusPlayerScript.GetCurrentWaypointNum() > _tempRC.GetCurrentWaypointNum() && focusPlayerScript.IsLapDone() && _tempRC.IsLapDone())
						isAhead = true;

					// is the focussed player on the same lap, same waypoint, but closer to it?
					if (focusPlayerScript.GetCurrentLap() == _tempRC.GetCurrentLap() && focusPlayerScript.GetCurrentWaypointNum() == _tempRC.GetCurrentWaypointNum() && focusPlayerScript.GetCurrentWaypointDist() < _tempRC.GetCurrentWaypointDist())
						isAhead = true;

					// has the player completed a lap and is getting ready to move onto the next one, with a higher waypoint?
					if (focusPlayerScript.GetCurrentLap() == _tempRC.GetCurrentLap() && focusPlayerScript.GetCurrentWaypointNum() == _tempRC.GetCurrentWaypointNum() && (focusPlayerScript.IsLapDone() == true && _tempRC.IsLapDone() == false))
						isAhead = true;

					if (focusPlayerScript.GetCurrentLap() == _tempRC.GetCurrentLap() && (focusPlayerScript.IsLapDone() == true && !_tempRC.IsLapDone()))
						isAhead = true;

					if (isAhead)
					{
						myPos--;
					}
				}
			}
			return myPos;
		}
	}
}