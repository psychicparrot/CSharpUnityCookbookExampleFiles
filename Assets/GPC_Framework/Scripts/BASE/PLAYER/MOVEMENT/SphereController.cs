using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPC
{
	[AddComponentMenu("CSharpBookCode/Base/Players/Sphere Controller")]

	[RequireComponent(typeof(Rigidbody))]

	public class SphereController : ExtendedCustomMonoBehaviour
	{
		public float turnSpeed = 2f;
		public float moveSpeed = 0.2f;
		public float maxSpeed = 30f;
		public float additionalGravity = 0.5f;

		private float steer;
		private float motor;

		public bool isGrounded;

		public BaseInputController _inputController;

		void Awake()
		{
			GetComponents();

			if (_RB == null)
				_RB = GetComponent<Rigidbody>();

			// we set angular drag high to counter collision forces
			_RB.angularDrag = 1f;
		}

		void FixedUpdate()
		{
			CheckGrounded();
			UpdatePhysics();
		}

		void LateUpdate()
		{
			if (canControl)
				GetInput();

		}

		void UpdatePhysics()
		{
			_RB.angularVelocity = Vector3.zero;

			Vector3 flatVel = _RB.velocity;
			flatVel.y = 0;

			if (isGrounded)
			{
				// apply steering and thrust (only on ground)
				_RB.AddForce((_TR.forward * motor) * moveSpeed, ForceMode.VelocityChange);
				_RB.AddTorque(0, steer * turnSpeed, 0, ForceMode.VelocityChange);

			}
			else
			{
				// apply additional gravity force
				_RB.AddForce(new Vector3(0, -additionalGravity, 0), ForceMode.VelocityChange);
			}

			// restrict speed
			if (flatVel.magnitude > maxSpeed)
				_RB.velocity = new Vector3(flatVel.x * 0.95f, _RB.velocity.y, flatVel.z * 0.95f);
		}

		void GetInput()
		{
			if (_inputController == null)
				return;

			steer = 0;
			motor = 0;

			steer = _inputController.horz;
			motor = _inputController.vert;
		}

		void CheckGrounded()
		{
			isGrounded = false;

			RaycastHit hit;
			if (Physics.Raycast(_TR.position, -Vector3.up, out hit, 2.0f))
			{
				isGrounded = true;
			}
		}
	}
}