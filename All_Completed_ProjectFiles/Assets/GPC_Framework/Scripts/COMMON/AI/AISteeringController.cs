using UnityEngine;
using AIStates;
namespace GPC
{
	public class AISteeringController : BaseAIController
	{
		public Vector3 relativeWaypointPosition;
		public float turnMultiplier = 3f;
		public float maxSpeed = 10f;
		public BaseInputController _inputController;

		public override void Start()
		{
			base.Start();
			SetAIState(AIState.move_along_waypoint_path);
		}

		public override void UpdateCurrentState()
		{
			// reset our inputs
			horz = 0;
			vert = 0;

			// look for obstacles
			obstacleFinderResult = IsObstacleAhead();

			switch (currentAIState)
			{
				case AIState.move_along_waypoint_path:
					SteerToWaypoint();
					break;

				case AIState.chasing_target:
					SteerToTarget();
					break;

				case AIState.paused_no_target:
					// do nothing
					break;

				default:
					// idle (do nothing)
					break;
			}
		}

		void SteerToWaypoint()
		{
			if (!didInit)
				return;
   
			UpdateWaypoints();
			if (_currentWaypointTransform == null)
				return;
	
			relativeWaypointPosition = _TR.InverseTransformPoint(_currentWaypointTransform.position);
			horz = (relativeWaypointPosition.x / relativeWaypointPosition.magnitude);

			if (Mathf.Abs(horz) < 0.5f)
				vert = relativeWaypointPosition.z / relativeWaypointPosition.magnitude - Mathf.Abs(horz);

			// the AvoidWalls function looks to see if there's anything in-front. If there is,
			// it will automatically change the value of moveDirection before we do the actual move
			if (obstacleFinderResult == 1)
				TurnRight();

			if (obstacleFinderResult == 2)
				TurnLeft();

			horz *= turnMultiplier;

			if (_RB.velocity.magnitude >= maxSpeed)
				vert = 0;
		}

		void SteerToTarget()
		{
			if (!didInit)
				return;
			if (_followTarget == null)
				return;

			Vector3 relativeTargetPosition = transform.InverseTransformPoint(_followTarget.position);
			horz = (relativeTargetPosition.x / relativeTargetPosition.magnitude);

			if (Vector3.Distance(_followTarget.position, _TR.position) > minChaseDistance)
				MoveForward();
			else
				NoMove();

			LookAroundFor();

			if (obstacleFinderResult == 1)
				TurnRight(); 

			if (obstacleFinderResult == 2)
				TurnLeft();

			if (obstacleFinderResult == 3)
				MoveBack();
		}

		public override void Update()
		{
			base.Update();

			if (_inputController == null)
				_inputController = GetComponent<BaseInputController>();

			// check that input is not enabled and drop out if it is
			if (_inputController.inputType != RaceInputController.InputTypes.noInput)
				return;

			_inputController.vert = vert;
			_inputController.horz = horz;
		}
	}
}