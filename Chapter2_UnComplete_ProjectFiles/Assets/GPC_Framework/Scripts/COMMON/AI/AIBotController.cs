using UnityEngine;
using AIStates;

namespace GPC
{
	public class AIBotController : BaseEnemyStatsController
	{
		private Vector3 moveVec;
		private Vector3 targetMoveVec;
		private float distanceToChaseTarget;

		[Header("Movement")]
		public bool isStationary;
		public float moveSpeed = 30f;
		public bool faceWaypoints;

		public override void Update()
		{
			UpdateCurrentState();
			DoMovement();
		}

		public override void UpdateCurrentState()
		{
			// reset our inputs
			horz = 0;
			vert = 0;

			obstacleFinderResult = IsObstacleAhead();

			switch (currentAIState)
			{
				// -----------------------------
				case AIState.move_looking_for_target:
					LookForTarget();
					break;
				case AIState.chasing_target:
					ChasingTarget();
					break;
				// -----------------------------

				case AIState.backing_up_looking_for_target:
					BackUpLookingForTarget();
					break;
				case AIState.stopped_turning_left:
					StoppedTurnLeft();
					break;

				case AIState.stopped_turning_right:
					StoppedTurnRight();
					break;
				case AIState.paused_looking_for_target:
					LookAroundFor();
					DoMovement();
					break;

				case AIState.move_along_waypoint_path:
					MoveAlongWaypointPath();
					break;

				case AIState.paused_no_target:
					break;

				default:
					// idle (do nothing)
					break;
			}
		}

		public virtual void DoMovement()
		{
			// move forward
			_TR.Translate(0, 0, vert * moveSpeed * Time.deltaTime);
			_TR.Rotate(0, horz * modelRotateSpeed * Time.deltaTime, 0);
		}

		void LookForTarget()
		{
			// look for chase target
			LookAroundFor();

			// if there are obstacles, this is where we'll switch to a state that can avoid hitting them
			switch(obstacleFinderResult)
			{
				case 1: // go right
					SetAIState(AIState.stopped_turning_right);
					break;

				case 2: // go left
					SetAIState(AIState.stopped_turning_left);
					break;

				case 3: // back up!
					SetAIState(AIState.backing_up_looking_for_target);
					break;

				default: // default is just keep moving forward
					MoveForward();
					break;
			}
		}

		void ChasingTarget()
		{
			if (_followTarget == null)
			{
				SetAIState(AIState.move_looking_for_target);
				return;
			}

			TurnTowardTarget(_followTarget);

			distanceToChaseTarget = Vector3.Distance(_TR.position, _followTarget.position);

			// check the range
			if (distanceToChaseTarget > minChaseDistance)
				MoveForward();

			bool canSeePlayer = CanSee(_followTarget);

			if (distanceToChaseTarget > maxChaseDistance || !canSeePlayer)
				SetAIState(AIState.move_looking_for_target); // set our state to 1 - moving_looking_for_target
		}

		void BackUpLookingForTarget()
		{
			LookAroundFor();
			MoveBack();

			if (obstacleFinderResult == 0)
			{
				if (Random.Range(0, 100) > 50)
					SetAIState(AIState.stopped_turning_left);
				else
					SetAIState(AIState.stopped_turning_right);
			}
		}

		void StoppedTurnLeft()
		{
			// look for chase target
			LookAroundFor();

			// stopped, turning left
			TurnLeft();

			if (obstacleFinderResult == 0)
				SetAIState(AIState.move_looking_for_target);
		}

		void StoppedTurnRight()
		{
			// look for chase target
			LookAroundFor();

			// stopped, turning right
			TurnRight();

			// check results from looking, to see if path ahead is clear
			if (obstacleFinderResult == 0)
				SetAIState(AIState.move_looking_for_target);
		}

		void MoveAlongWaypointPath()
		{
			// make sure we have been initialized before trying to access waypoints
			if (!didInit && !reachedLastWaypoint)
				return;

			UpdateWaypoints();

			if (!isStationary)
			{
				targetMoveVec = Vector3.Normalize(_currentWaypointTransform.position - _TR.position);
				moveVec = Vector3.Lerp(moveVec, targetMoveVec, Time.deltaTime * pathSmoothing);
				_TR.Translate(moveVec * moveSpeed * Time.deltaTime);

				if (faceWaypoints)
					TurnTowardTarget(_currentWaypointTransform);
			}
		}

		public void SetMoveSpeed(float aNum)
		{
			moveSpeed = aNum;
		}
	}
}