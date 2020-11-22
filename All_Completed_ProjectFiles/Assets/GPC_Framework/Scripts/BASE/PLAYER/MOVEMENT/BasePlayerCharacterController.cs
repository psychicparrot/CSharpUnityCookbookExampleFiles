using UnityEngine;

namespace GPC
{
	[AddComponentMenu("CSharpBookCode/Base/Character/Base Player Character Controller")]

	public class BasePlayerCharacterController : ExtendedCustomMonoBehaviour
	{
		[Header("Movement")]
		// The speed when walking
		public float walkSpeed = 7.0f;

		// after runAfterSeconds of walking we run with runSpeed
		public float runSpeed = 12.0f;

		public float speedSmoothing = 10.0f;
		public float rotateSpeed = 500.0f;
		public float runAfterSeconds = 3.0f;

		public bool moveDirectionally;

		// The current x-z move speed
		private float moveSpeed = 0.0f;

		// The current move direction in x-z
		private Vector3 moveDirection = Vector3.zero;

		// When did the user start walking (Used for going into run after a while)
		private float walkTimeStart = 0.0f;

		// The last collision flags returned from controller.Move
		private CollisionFlags collisionFlags;

		[Space]
		public float horz;
		public float vert;
		public bool isRespawning;
		public bool isFinished;	
		public BaseInputController _inputController;

		private CharacterController _charController;	
		private Vector3 targetDirection;
		private float curSmooth;
		private float targetSpeed;
		private float curSpeed;
		private Vector3 forward;
		private Vector3 right;

		// -------------------------------------------------------------------------

		public virtual void Start()
		{
			Init();
		}

		public virtual void Init()
		{
			moveDirection = transform.TransformDirection(Vector3.forward);

			_charController = GetComponent<CharacterController>();
			GetComponents();
			_RB = GetComponent<Rigidbody>();
			_inputController = GetComponent<BaseInputController>();
		}

		public void SetUserInput(bool setInput)
		{
			canControl = setInput;
		}

		public virtual void GetInput()
		{
			if (isFinished || isRespawning || !canControl)
			{
				horz = 0;
				vert = 0;
				return;
			}

			if (!canControl)
				return;

			// get inputs
			horz = _inputController.GetHorizontal();
			vert = _inputController.GetVertical();
		}

		public virtual void LateUpdate()
		{
			if (canControl)
				GetInput();
		}

		void UpdateSmoothedMovementDirection()
		{
			if (moveDirectionally)
				UpdateDirectionalMovement();
			else
				UpdateRotationMovement();
		}

		void UpdateDirectionalMovement()
		{
			// find target direction
			targetDirection = horz * Vector3.right;
			targetDirection += vert * Vector3.forward;

			// We store speed and direction seperately,
			// so that when the character stands still we still have a valid forward direction
			// moveDirection is always normalized, and we only update  it if there is user input.
			if (targetDirection != Vector3.zero)
			{
				moveDirection = Vector3.RotateTowards(moveDirection, targetDirection, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);
				moveDirection = moveDirection.normalized;
			}

			// Smooth the speed based on the current target direction
			curSmooth = speedSmoothing * Time.deltaTime;

			// Choose target speed
			//* We want to support analog input but make sure you cant walk faster diagonally than just forward or sideways
			targetSpeed = Mathf.Min(targetDirection.magnitude, 1.0f);

			// adjust move speed
			if (Time.time - runAfterSeconds > walkTimeStart)
			{
				targetSpeed *= runSpeed;
			}
			else
			{
				targetSpeed *= walkSpeed;
			}

			moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth);

			// Reset walk time start when we slow down
			if (moveSpeed < walkSpeed * 0.3f)
				walkTimeStart = Time.time;

			// Calculate actual motion
			Vector3 movement = moveDirection * moveSpeed;
			movement *= Time.deltaTime;

			// Move the controller
			collisionFlags = _charController.Move(movement);

			// Set rotation to the move direction
			_TR.rotation = Quaternion.LookRotation(moveDirection);
		}

		void UpdateRotationMovement()
		{
			// this character movement is based on the code in the Unity help file for CharacterController.SimpleMove
			// http://docs.unity3d.com/Documentation/ScriptReference/CharacterController.SimpleMove.html

			_TR.Rotate(0, horz * rotateSpeed * Time.deltaTime, 0);
			curSpeed = moveSpeed * vert;
			_charController.SimpleMove(_TR.forward * curSpeed);

			// Target direction (the max we want to move, used for calculating target speed)
			targetDirection = vert * _TR.forward;

			// Smooth the speed based on the current target direction
			float curSmooth = speedSmoothing * Time.deltaTime;

			// Choose target speed
			//* We want to support analog input but make sure you cant walk faster diagonally than just forward or sideways
			targetSpeed = Mathf.Min(targetDirection.magnitude, 1.0f);

			// decide on animation state and adjust move speed
			if (Time.time - runAfterSeconds > walkTimeStart)
			{
				targetSpeed *= runSpeed;
			}
			else
			{
				targetSpeed *= walkSpeed;
			}

			moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth);

			// Reset walk time start when we slow down
			if (moveSpeed < walkSpeed * 0.3f)
				walkTimeStart = Time.time;
		}

		public void Update()
		{
			ApplyGravity();

			if (!canControl)
			{
				// kill all inputs if not controllable.
				Input.ResetInputAxes();
			}
			UpdateSmoothedMovementDirection();
		}

		void ApplyGravity()
		{
			// apply some gravity to the character controller
			float gravity = -9.81f * Time.deltaTime;
			_charController.Move(new Vector3(0, gravity, 0));
		}

		public float GetSpeed()
		{
			return moveSpeed;
		}

		public Vector3 GetDirection()
		{
			return moveDirection;
		}

		public bool IsMoving()
		{
			return (Mathf.Abs(vert) + Mathf.Abs(horz) > 0.5f);
		}
	}
}