using UnityEngine;
namespace GPC
{
	public class RaceController : ExtendedCustomMonoBehaviour
	{
		private bool isFinished;
		private bool isLapDone;
		public float currentWaypointDist;
		public int currentWaypointNum;
		public int lastWaypointNum;
		public float waypointDistance = 20f;
		public WaypointsController _waypointsController;
		public int lapsComplete;
		public bool goingWrongWay;
		public bool oldWrongWay;
		public float timeWrongWayStarted;
		public int raceID = -1;
		private Transform _currentWaypointTransform;
		private Vector3 nodePosition;
		private float targetAngle;
		private Vector3 myPosition;
		private Vector3 diff;
		private int totalWaypoints;
		private bool doneInit;

		public void Awake()
		{
			raceID = GlobalRaceManager.instance.GetUniqueID(this);
		}

		public virtual void Init()
		{
			_TR = transform;
			doneInit = true;
		}

		public int GetID()
		{
			return raceID;
		}

		public bool IsLapDone()
		{
			return isLapDone;
		}

		public void RaceFinished()
		{
			isFinished = true;
			PlayerFinishedRace();
		}

		public virtual void PlayerFinishedRace() { }

		public void RaceStart()
		{
			isFinished = false;
		}

		public bool IsFinished()
		{
			return isFinished;
		}

		public int GetCurrentLap()
		{
			return GlobalRaceManager.instance.GetLapsDone(raceID) + 1;
		}

		public void ResetLapCounter()
		{
			GlobalRaceManager.instance.ResetLapCount(raceID);
		}

		public int GetCurrentWaypointNum()
		{
			return currentWaypointNum;
		}

		public float GetCurrentWaypointDist()
		{
			return currentWaypointDist;
		}

		public bool GetIsFinished()
		{
			return isFinished;
		}

		public bool GetIsLapDone()
		{
			return isLapDone;
		}

		public void SetWayController(WaypointsController aControl)
		{
			_waypointsController = aControl;
		}

		public Transform GetWaypointTransform()
		{
			if (_currentWaypointTransform == null)
			{
				currentWaypointNum = 0;
				_currentWaypointTransform = _waypointsController.GetWaypoint(currentWaypointNum);
			}

			return _currentWaypointTransform;
		}

		public Transform GetRespawnWaypointTransform()
		{
			_currentWaypointTransform = _waypointsController.GetWaypoint(currentWaypointNum);
			return _currentWaypointTransform;
		}

		public void UpdateWaypoints()
		{
			if (!doneInit)
				Init();

			if (_waypointsController == null)
				return;

			if (totalWaypoints == 0)
			{
				// grab total waypoints
				totalWaypoints = _waypointsController.GetTotal();
				return;
			}

			// here, we deal with making sure that we always have a waypoint set up
			if (_currentWaypointTransform == null)
			{
				currentWaypointNum = 0;
				_currentWaypointTransform = _waypointsController.GetWaypoint(currentWaypointNum);
			}

			int possibleWaypoint = _waypointsController.FindNearestWaypoint(_TR.position, 200);

			// try to find the nearest waypoint but make sure it's not TOO far away that we can cheat!
			if (possibleWaypoint > currentWaypointNum && (possibleWaypoint-currentWaypointNum)<3)
				currentWaypointNum = possibleWaypoint;

			// now we need to check to see if we are close enough to the current waypoint
			// to advance on to the next one
			myPosition = _TR.position;
			myPosition.y = 0;

			// get waypoint position and 'flatten' it
			nodePosition = _currentWaypointTransform.position;
			nodePosition.y = 0;

			// check distance from this vehicle to the waypoint
			currentWaypointDist = Vector3.Distance(nodePosition, myPosition);

			if (currentWaypointDist < waypointDistance)
			{
				// we are close to the current node, so let's move on to the next one!
				currentWaypointNum++;

				// now check to see if we have been all the way around the track and need to start again
				if (currentWaypointNum >= totalWaypoints)
				{
					// reset our current waypoint to the first one again
					currentWaypointNum = 0;
					isLapDone = true;
				}

				// grab our transform reference from the waypoint controller
				_currentWaypointTransform = _waypointsController.GetWaypoint(currentWaypointNum);

				// we finish by recalculating currentWaypointDist
				nodePosition = _currentWaypointTransform.position;
				nodePosition.y = 0;

				// check distance from this vehicle to the waypoint
				currentWaypointDist = Vector3.Distance(nodePosition, myPosition);
			}
		}


		public void CheckWrongWay()
		{
			if (_currentWaypointTransform == null)
				return;

			Vector3 relativeTarget = _TR.InverseTransformPoint(_currentWaypointTransform.position);

			// Calculate the target angle for the wheels,  
			// so they point towards the target 
			targetAngle = Mathf.Atan2(relativeTarget.x, relativeTarget.z);

			// Atan returns the angle in radians, convert to degrees 
			targetAngle *= Mathf.Rad2Deg;

			if (targetAngle < -90 || targetAngle > 90)
			{
				goingWrongWay = true;
			}
			else
			{
				goingWrongWay = false;
				timeWrongWayStarted = -1;
			}

			if (oldWrongWay != goingWrongWay)
			{
				// store the current time
				timeWrongWayStarted = Time.time;
			}

			oldWrongWay = goingWrongWay;
		}

		public virtual void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.layer == 12 && isLapDone == true)
			{
				// increase lap counter
				lapsComplete++;

				// reset our lapDone flag ready for when we finish the next lap
				isLapDone = false;

				// tell race controller we just finished a lap and which lap we are now on
				GlobalRaceManager.instance.CompletedLap(raceID);
			}
		}

	}
}