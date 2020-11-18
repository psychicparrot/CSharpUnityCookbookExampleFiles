using UnityEngine;
using System.Collections;

// use this script in conjunction with the BaseAIController to make a bot move around
namespace GPC
{

	public class SimpleBotMover : MonoBehaviour
	{
		public BaseAIController AIController;
		public Rigidbody _RB;
		public float turnSpeed = 0.5f;
		public float moveSpeed = 0.5f;

		public Vector3 centerOfGravity;

		private Transform _TR;

		void Start()
		{
			// cache a ref to our transform
			_TR = transform;

			// if it hasn't been set in the editor, let's try and find it on this transform
			if (AIController == null)
				AIController = _TR.GetComponent<BaseAIController>();

			// set center of gravity
			if (_RB != null)
			{
				_RB.centerOfMass = centerOfGravity;
			}
		}

		void Update()
		{
			// turn the transform, if required
			_TR.Rotate(new Vector3(0, Time.deltaTime * AIController.horz * 0.1f, 0));

			// if we have a rigidbody, move it if required
			if (_RB != null)
			{
				_RB.AddForce((_TR.forward * moveSpeed * Time.deltaTime) * AIController.vert, ForceMode.VelocityChange);
			}


		}
	}
}