using UnityEngine;
namespace GPC
{
	[AddComponentMenu("CSharpBookCode/Base/Vehicles/Wheeled Vehicle Controller")]
	[RequireComponent(typeof(Rigidbody))]

	public class BaseWheeledVehicle : ExtendedCustomMonoBehaviour
	{
		public bool isLocked;

		[Header("Wheel Colliders")]

		public WheelCollider _frontWheelLeft;
		public WheelCollider _frontWheelRight;
		public WheelCollider _rearWheelLeft;
		public WheelCollider _rearWheelRight;

		public BaseInputController _inputController;

		[Header("Steer settings")]
		public float steerMax = 30f;
		public float accelMax = 5000f;
		public float brakeMax = 5000f;

		[System.NonSerialized]
		public float steer = 0f;
		[System.NonSerialized]
		public float motor = 0f;
		[System.NonSerialized]
		public float brake = 0f;

		[Space]
		public float wheelToGroundCheckHeight = 0.5f;

		[Space]
		public bool fakeBrake;
		public float fakeBrakeDivider = 0.95f;

		[Space]
		public bool turnHelp;
		public float turnHelpAmount = 10f;

		public bool isGrounded;

		[System.NonSerialized]
		public float mySpeed;

		[System.NonSerialized]
		public Vector3 velo;

		[Header("Audio Settings")]

		public AudioSource _engineSoundSource;
		public float audioPitchOffset = 0.5f;

		public virtual void Start()
		{
			Init();
		}

		public virtual void Init()
		{
			Debug.Log("BaseWheeledVehicle Init called.");

			// cache the usual suspects
			_RB = GetComponent<Rigidbody>();
			_GO = gameObject;
			_TR = transform;

			_inputController = GetComponent<BaseInputController>();
			_RB.centerOfMass = new Vector3(0, -1f, 0);

			// see if we can find an engine sound source, if we need to
			if (_engineSoundSource == null)
			{
				_engineSoundSource = _GO.GetComponent<AudioSource>();
			}
		}

		public void SetUserInput(bool setInput)
		{
			canControl = setInput;
		}

		public void SetLock(bool lockState)
		{
			isLocked = lockState;
		}

		public virtual void LateUpdate()
		{
			if (canControl)
				GetInput();

			UpdateEngineAudio();
		}

		public virtual void FixedUpdate()
		{
			UpdatePhysics();
			DoFakeBrake();
			CheckGround();
		}

		public virtual void UpdatePhysics()
		{
			CheckLock();

			// work out a flat velocity
			velo = _RB.velocity;
			velo = transform.InverseTransformDirection(_RB.velocity);

			// work out our current forward speed
			mySpeed = velo.z;

			// if we're moving slow, we reverse motorTorque and remove brakeTorque so that the car will reverse
			if (mySpeed < 2)
			{
				// that is, if we're pressing down the brake key (making brake>0)
				if (brake > 0)
				{
					_rearWheelLeft.motorTorque = -brakeMax * brake;
					_rearWheelRight.motorTorque = -brakeMax * brake;

					_rearWheelLeft.brakeTorque = 0;
					_rearWheelRight.brakeTorque = 0;

					_frontWheelLeft.steerAngle = steerMax * steer;
					_frontWheelRight.steerAngle = steerMax * steer;

					// drop out of this function before applying the 'regular' non-reversed values to the wheels
					return;
				}
			}

			if (turnHelp)
				_RB.AddTorque(Vector3.up * steer * turnHelpAmount * _RB.mass);

			// apply regular movement values to the wheels
			_rearWheelLeft.motorTorque = accelMax * motor;
			_rearWheelRight.motorTorque = accelMax * motor;

			_rearWheelLeft.brakeTorque = brakeMax * brake;
			_rearWheelRight.brakeTorque = brakeMax * brake;

			_frontWheelLeft.steerAngle = steerMax * steer;
			_frontWheelRight.steerAngle = steerMax * steer;
		}

		public void DoFakeBrake()
		{
			if (isGrounded && brake > 0 && fakeBrake && mySpeed > 0)
			{
				tempVEC = _RB.velocity;
				tempVEC.x = tempVEC.x * fakeBrakeDivider;
				tempVEC.z = tempVEC.z * fakeBrakeDivider;
				_RB.velocity = tempVEC;
			}
		}

		public void CheckGround()
		{
			bool FLGrounded = false;
			bool FRGrounded = false;
			bool RLGrounded = false;
			bool RRGrounded = false;

			if (Physics.Raycast(_frontWheelLeft.transform.position, Vector3.down, wheelToGroundCheckHeight))
				FLGrounded = true;

			if (Physics.Raycast(_frontWheelRight.transform.position, Vector3.down, wheelToGroundCheckHeight))
				FRGrounded = true;

			if (Physics.Raycast(_rearWheelLeft.transform.position, Vector3.down, wheelToGroundCheckHeight))
				RLGrounded = true;

			if (Physics.Raycast(_rearWheelRight.transform.position, Vector3.down, wheelToGroundCheckHeight))
				RRGrounded = true;


			if (FLGrounded && FRGrounded && RLGrounded && RRGrounded)
			{
				isGrounded = true;
			}
			else
			{
				isGrounded = false;
			}
		}

		public void CheckLock()
		{
			if (isLocked)
			{
				// control is locked out and we should be stopped
				steer = 0;
				brake = 0;
				motor = 0;

				// hold our rigidbody in place (but allow the Y to move so the car may drop to the ground if it is not exactly matched to the terrain)
				tempVEC = _RB.velocity;
				tempVEC.x = 0;
				tempVEC.z = 0;
				_RB.velocity = tempVEC;
			}
		}

		public virtual void GetInput()
		{
			// calculate steering amount
			steer = Mathf.Clamp(_inputController.GetHorizontal(), -1, 1);

			// how much accelerator?
			motor = Mathf.Clamp(_inputController.GetVertical(), 0, 1);

			// how much brake?
			brake = -1 * Mathf.Clamp(_inputController.GetVertical(), -1, 0);
		}

		public virtual void UpdateEngineAudio()
		{
			_engineSoundSource.pitch = audioPitchOffset + (Mathf.Abs(_frontWheelLeft.rpm) * 0.005f);
		}
	}
}