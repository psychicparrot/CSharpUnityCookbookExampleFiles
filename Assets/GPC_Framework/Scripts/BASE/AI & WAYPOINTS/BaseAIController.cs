using UnityEngine;
using AIStates;
namespace GPC
{
	[AddComponentMenu("CSharpBookCode/Base/AI Controller")]

	public class BaseAIController : ExtendedCustomMonoBehaviour
	{
		public bool AIControlled;

		[Header("AI States")]
		public AIState currentAIState;
		public AIState targetAIState;

		[Header("Enemy input")]
		public float horz;
		public float vert;

		[Header("Enemy AI movement values")]
		public float obstacleAvoidDistance = 40f;
		public float modelRotateSpeed = 15f;
		public int _followTargetMaxTurnAngle = 120;
		public float minChaseDistance = 1f;
		public float maxChaseDistance = 10f;
		public float visionHeightOffset = 1f;

		[Header("Waypoints")]
		// waypoint following related variables
		public WaypointsController _waypointsController;

		public Transform _currentWaypointTransform;
		public Transform _followTarget;
		public Transform _rotateTransform;

		[System.NonSerialized]
		public bool reachedLastWaypoint;
		public float waypointDistance = 5f;
		public float pathSmoothing = 2f;
		public bool shouldReversePathFollowing;
		public bool loopPath;
		public bool destroyAtEndOfWaypoints;
		public bool startAtFirstWaypoint;

		[Header("Layer masks and raycasting")]
		public LayerMask obstacleAvoidLayers;
		public string potentialTargetTag = "Player";
		public LayerMask targetLayer;
		public int obstacleFinderResult;
		
		private int totalWaypoints;
		private int currentWaypointNum;
		private RaycastHit hit;
		private Vector3 tempDirVec;
		private Vector3 relativeTarget;
		private float targetAngle;
		private int obstacleHitType;
		public int lookAheadWaypoints = 0;

		public virtual void Start()
		{
			Init();
		}

		public virtual void Init()
		{
			// cache ref to gameObject
			_GO = gameObject;

			// cache ref to transform
			_TR = transform;

			// cache a ref to our rigidbody
			_RB = _TR.GetComponent<Rigidbody>();

			if (_rotateTransform == null)
				_rotateTransform = _TR;

			didInit = true;
		}

		public void SetAIControl(bool state)
		{
			AIControlled = state;
		}

		// set up AI parameters --------------------
		public void SetobstacleAvoidDistance(float aNum)
		{
			obstacleAvoidDistance = aNum;
		}

		public void SetWaypointDistance(float aNum)
		{
			waypointDistance = aNum;
		}

		public void SetMinChaseDistance(float aNum)
		{
			minChaseDistance = aNum;
		}

		public void SetMaxChaseDistance(float aNum)
		{
			maxChaseDistance = aNum;
		}

		// -----------------------------------------

		public virtual void SetAIState(AIState newState)
		{
			// update AI state
			targetAIState = newState;
			UpdateTargetState();
		}

		public virtual void SetChaseTarget(Transform theTransform)
		{
			// set a target for this AI to chase, if required
			_followTarget = theTransform;
		}

		public virtual void Update()
		{
			// make sure we have initialized before doing anything
			if (!didInit)
				Init();

			if (!AIControlled)
				return;

			// do AI updates
			UpdateCurrentState();
		}

		public virtual void UpdateCurrentState()
		{
			// reset our inputs
			horz = 0;
			vert = 0;

			switch (currentAIState)
			{
				case AIState.paused_no_target:
					// do nothing
					break;

				default:
					// idle (do nothing)
					break;
			}
		}

		public virtual void UpdateTargetState()
		{
			switch (targetAIState)
			{
				default:
					// idle (do nothing)
					break;
			}

			currentAIState = targetAIState;
		}

		public virtual void TurnLeft()
		{
			horz = -1;
		}

		public virtual void TurnRight()
		{
			horz = 1;
		}

		public virtual void MoveForward()
		{
			vert = 1;
		}

		public virtual void MoveBack()
		{
			vert = -1;
		}

		public virtual void NoMove()
		{
			vert = 0;
		}

		public virtual void LookAroundFor()
		{
			if (_followTarget != null)
			{
				float theDist = Vector3.Distance(_TR.position, _followTarget.position);
				bool canSee = CanSee(_followTarget);

				if (theDist < maxChaseDistance)
				{
					if (canSee == true)
						SetAIState(AIState.chasing_target);
				}
			}
			else
			{
				GameObject potential = GameObject.FindGameObjectWithTag(potentialTargetTag);
				_followTarget = potential.transform;
			}
		}

		private int obstacleFinding;

		public virtual int IsObstacleAhead()
		{
			// quick check to make sure that _TR has been set
			if (_TR == null)
				return 0;
		
			// draw this raycast so we can see what it is doing
			Debug.DrawRay(_TR.position, ((_TR.forward + (-_TR.right * 0.5f)) * obstacleAvoidDistance));
			Debug.DrawRay(_TR.position, ((_TR.forward + (_TR.right * 0.5f)) * obstacleAvoidDistance));

			obstacleHitType = 0;

			RaycastHit hit;
			if (Physics.Raycast(_TR.position, _TR.forward + (-_TR.right * 0.5f), out hit, obstacleAvoidDistance, obstacleAvoidLayers))
			{
				// obstacle
				// it's a left hit, so it's a type 1 (though it could change when we check on the other side)
				obstacleHitType = 1;
			}

			if (Physics.Raycast(_TR.position, _TR.forward + (_TR.right * 0.5f), out hit, obstacleAvoidDistance, obstacleAvoidLayers))
			{
				// obstacle
				if (obstacleHitType == 0)
				{
					// if we haven't hit anything yet, this is a type 2
					obstacleHitType = 2;
				}
				else
				{
					// if we have hits on both left and right raycasts, it's a type 3
					obstacleHitType = 3;
				}
			}

			return obstacleHitType;
		}

		public void TurnTowardTarget(Transform aTarget)
		{
			if (aTarget == null)
				return;

			relativeTarget = _rotateTransform.InverseTransformPoint(aTarget.position); // note we use _rotateTransform as a rotation object rather than _TR!

			// Calculate the target angle  
			targetAngle = Mathf.Atan2(relativeTarget.x, relativeTarget.z);

			// Atan returns the angle in radians, convert to degrees 
			targetAngle *= Mathf.Rad2Deg;

			targetAngle = Mathf.Clamp(targetAngle, -_followTargetMaxTurnAngle - targetAngle, _followTargetMaxTurnAngle);

			// turn towards the target at the rate of modelRotateSpeed
			_rotateTransform.Rotate(0, targetAngle * modelRotateSpeed * Time.deltaTime, 0);
		}

		public bool CanSee(Transform aTarget)
		{
			// first, let's get a vector to use for raycasting by subtracting the target position from our AI position
			tempDirVec = Vector3.Normalize(aTarget.position - _TR.position);

			// lets have a debug line to check the distance between the two manually, in case you run into trouble!
			Debug.DrawLine(_TR.position + (visionHeightOffset * _TR.up), aTarget.position, Color.red);

			if (Physics.Raycast(_TR.position + (visionHeightOffset * _TR.up), tempDirVec, out hit, Mathf.Infinity))
			{
				if (IsInLayerMask(hit.transform.gameObject.layer, targetLayer))
				{
					return true;
				}
			}

			// nothing found, so return false
			return false;
		}

		public void SetWayController(WaypointsController aControl)
		{
			_waypointsController = aControl;
			aControl = null;

			// grab total waypoints
			totalWaypoints = _waypointsController.GetTotal();

			// make sure that if you use SetReversePath to set shouldReversePathFollowing that you
			// call SetReversePath for the first time BEFORE SetWayController, otherwise it won't set the first waypoint correctly
			if (shouldReversePathFollowing)
			{
				currentWaypointNum = totalWaypoints - 1;
			}
			else
			{
				currentWaypointNum = 0;
			}

			Init();

			// get the first waypoint from the waypoint controller
			_currentWaypointTransform = _waypointsController.GetWaypoint(currentWaypointNum);

			if (startAtFirstWaypoint)
			{
				// position at the _currentWaypointTransform position
				_TR.position = _currentWaypointTransform.position;
			}
		}

		public void SetReversePath(bool shouldRev)
		{
			shouldReversePathFollowing = shouldRev;
		}

		public void SetRotateSpeed(float aRate)
		{
			modelRotateSpeed = aRate;
		}

		public void UpdateWaypoints()
		{
			// If we don't have a waypoint controller, we safely drop out
			if (_waypointsController == null)
				return;

			if (reachedLastWaypoint && destroyAtEndOfWaypoints)
			{
				// destroy myself(!)
				Destroy(gameObject);
				return;
			}
			else if (reachedLastWaypoint)
			{
				currentWaypointNum = 0;
				reachedLastWaypoint = false;
			}

			// because of the order that scripts run and are initialised, it is possible for this function
			// to be called before we have actually finished running the waypoints initialization, which
			// means we need to drop out to avoid doing anything silly or before it breaks the game.
			if (totalWaypoints == 0)
			{
				// grab total waypoints
				totalWaypoints = _waypointsController.GetTotal();
				return;
			}

			if (_currentWaypointTransform == null)
			{
				// grab our transform reference from the waypoint controller
				_currentWaypointTransform = _waypointsController.GetWaypoint(currentWaypointNum + lookAheadWaypoints);
			}

			// try to find the nearest waypoint but make sure it's not TOO far away that we can cheat!
			int possibleWaypoint = _waypointsController.FindNearestWaypoint(_TR.position, 200);
			if (possibleWaypoint > currentWaypointNum && (possibleWaypoint - currentWaypointNum) < 3)
				currentWaypointNum = possibleWaypoint;

			// now we check to see if we are close enough to the current waypoint
			// to advance on to the next one

			Vector3 myPosition = _TR.position;
			myPosition.y = 0;

			// get waypoint position and 'flatten' it
			Vector3 nodePosition = _currentWaypointTransform.position;
			nodePosition.y = 0;

			// check distance from this to the waypoint

			float currentWayDist = Vector3.Distance(nodePosition, myPosition);

			if (currentWayDist < waypointDistance)
			{
				// we are close to the current node, so let's move on to the next one!

				if (shouldReversePathFollowing)
				{
					currentWaypointNum--;
					// now check to see if we have been all the way around
					if (currentWaypointNum < 0)
					{
						// just incase it gets referenced before we are destroyed, let's keep it to a safe index number
						currentWaypointNum = 0;
						// completed the route!
						reachedLastWaypoint = true;
						// if we are set to loop, reset the currentWaypointNum to 0
						if (loopPath)
						{
							currentWaypointNum = totalWaypoints;
							reachedLastWaypoint = false;
						}
						// drop out of this function before we grab another waypoint into _currentWaypointTransform, as
						// we don't need one and the index may be invalid
						return;
					}
				} else {
					currentWaypointNum++;
					// now check to see if we have been all the way around
					if (currentWaypointNum >= totalWaypoints)
					{
						// completed the route!
						reachedLastWaypoint = true;
						// if we are set to loop, reset the currentWaypointNum to 0
						if (loopPath)
						{
							currentWaypointNum = 0;

							// the route keeps going in a loop, so we don't want reachedLastWaypoint to ever become true
							reachedLastWaypoint = false;
						}
						// drop out of this function before we grab another waypoint into _currentWaypointTransform, as
						// we don't need one and the index may be invalid
						return;
					}
				}

				// grab our transform reference from the waypoint controller
				_currentWaypointTransform = _waypointsController.GetWaypoint(currentWaypointNum);

			}
		}

		public float GetHorizontal()
		{
			return horz;
		}

		public float GetVertical()
		{
			return vert;
		}
	}
}