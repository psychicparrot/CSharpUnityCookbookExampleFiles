using UnityEngine;
using System.Collections.Generic;
namespace GPC
{
	[AddComponentMenu("CSharpBookCode/Utility/Waypoints Controller")]

	public class WaypointsController : MonoBehaviour
	{
		[ExecuteInEditMode]

		private List<Transform> _transforms;
		private Vector3 firstPoint; // store our first waypoint so we can loop the path
		private Vector3 currentPoint;
		private Vector3 lastPoint;
		private float distance;
		private int totalTransforms;
		private Vector3 diff;
		private float curDistance;
		private Transform _closest;
		private Transform _pointT;

		public bool closed = true;
		public bool shouldReverse;

		void Start()
		{
			GetTransforms();
		}

		void OnDrawGizmos()
		{
			GetTransforms();
			if (totalTransforms < 2)
				return;

			_pointT = (Transform)_transforms[0];

			// draw our path
			Transform _tempTR = (Transform)_transforms[0];
			firstPoint = _tempTR.position;
			lastPoint = firstPoint;
			_pointT = _tempTR;

			for (int i = 0; i < totalTransforms; i++)
			{
				_tempTR = (Transform)_transforms[i];
				if (_tempTR == null)
				{
					GetTransforms();
					return;
				}

				// grab the current waypoint position
				currentPoint = _tempTR.position;

				Gizmos.color = Color.green;
				Gizmos.DrawSphere(currentPoint, 2);

				// draw the line between the last waypoint and this one
				Gizmos.color = Color.red;
				Gizmos.DrawLine(lastPoint, currentPoint);

				// point our last transform at the latest position
				_pointT.LookAt(currentPoint);

				// update our 'last' waypoint to become this one as we
				// move on to find the next...
				lastPoint = currentPoint;

				// update the pointing transform
				_pointT = (Transform)_transforms[i];
			}

			// close the path
			if (closed)
			{
				Gizmos.color = Color.red;
				Gizmos.DrawLine(currentPoint, firstPoint);
			}
		}

		public void GetTransforms()
		{
			// we store all of the waypoints transforms in a List
			_transforms = new List<Transform>();

			// now we go through any transforms 'under' this transform, so all of
			// the child objects that act as our waypoints get put into our list
			foreach (Transform t in transform)
			{
				// add this transform to our list
				_transforms.Add(t);
			}

			totalTransforms = (int)_transforms.Count;
		}

		public void SetReverseMode(bool rev)
		{
			shouldReverse = rev;
		}

		public int FindNearestWaypoint(Vector3 fromPos, float maxRange)
		{
			if (_transforms == null)
				GetTransforms();

			distance = Mathf.Infinity;
			int tempIndex = 0;

			// Iterate through them and find the closest one
			for (int i = 0; i < _transforms.Count; i++)
			{
				Transform _tempTR = (Transform)_transforms[i];

				// calculate the distance between the current transform and the passed in transform's position vector
				diff = (_tempTR.position - fromPos);
				curDistance = diff.sqrMagnitude;

				if (curDistance < distance)
				{
					if (Mathf.Abs(_tempTR.position.y - fromPos.y) < maxRange)
					{

						// set our current 'winner' (closest transform) to the transform we just found
						_closest = _tempTR;

						// store the index of this waypoint
						tempIndex = i;

						// set our 'winning' distance to the distance we just found
						distance = curDistance;
					}
				}
			}

			if (_closest)
			{
				// return the waypoint we found in this test
				return tempIndex;
			}
			else
			{
				// no waypoint was found, so return -1 (this should be acccounted for at the other end!)
				return -1;
			}
		}

		// this function has the addition of a check to avoid finding the same transform as one passed in. we use
		// this to make sure that when we are looking for the nearest waypoint we don't find the same one as
		// we just passed

		public int FindNearestWaypoint(Vector3 fromPos, Transform exceptThis, float maxRange)
		{
			if (_transforms == null)
				GetTransforms();

			// the distance variable is just used to hold the 'current' distance when
			// we are comparing, so that we can find the closest
			distance = Mathf.Infinity;
			int tempIndex = 0;

			// Iterate through them and find the closest one
			for (int i = 0; i < totalTransforms; i++)
			{
				// grab a reference to a transform
				Transform _tempTR = (Transform)_transforms[i];

				// calculate the distance between the current transform and the passed in transform's position vector
				diff = (_tempTR.position - fromPos);
				curDistance = diff.sqrMagnitude;

				// now compare distances - making sure that we are not 
				if (curDistance < distance && _tempTR != exceptThis)
				{
					if (Mathf.Abs(_tempTR.position.y - fromPos.y) < maxRange)
					{

						// set our current 'winner' (closest transform) to the transform we just found
						_closest = _tempTR;

						// store the index of this waypoint
						tempIndex = i;

						// set our 'winning' distance to the distance we just found
						distance = curDistance;
					}
				}
			}

			// now we make sure that we did actually find something, then return it
			if (_closest)
			{
				// return the waypoint we found in this test
				return tempIndex;
			}
			else
			{
				// no waypoint was found, so return -1 (this should be acccounted for at the other end!)
				return -1;
			}
		}

		public Transform GetWaypoint(int index)
		{
			if (shouldReverse)
			{
				// send back the reverse index'd waypoint
				index = (totalTransforms - 1) - index;

				if (index < 0)
					index = 0;
			}

			// make sure that we have populated the transforms list, if not, populate it
			if (_transforms == null)
				GetTransforms();

			if (index > totalTransforms - 1)
				return null;

			return (Transform)_transforms[index];
		}

		public int GetTotal()
		{
			return totalTransforms;
		}
	}
}