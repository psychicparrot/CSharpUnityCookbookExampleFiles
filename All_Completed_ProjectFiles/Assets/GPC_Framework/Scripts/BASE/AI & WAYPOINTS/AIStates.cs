using UnityEngine;

namespace AIStates
{
	public enum AIState
	{
		idle,
		stopped_turning_left,
		stopped_turning_right,
		paused_no_target,
		paused_looking_for_target,
		move_along_waypoint_path,
		move_looking_for_target,
		chasing_target,
		backing_up_looking_for_target
	}
}
