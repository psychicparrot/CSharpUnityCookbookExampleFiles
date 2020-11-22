using UnityEngine;

namespace GPC
{	public class BasePlayer2DPlatformCharacter : ExtendedCustomMonoBehaviour
	{
		public float jumpPower = 2.5f;
		public float runPower = 0.6f;

		public bool canAirSteer;

		[System.NonSerialized]
		public bool allow_left;

		[System.NonSerialized]
		public bool allow_right;

		[System.NonSerialized]
		public bool allow_jump;

		public LayerMask groundLayerMask;
		public bool isOnGround;

		public BaseInputController _inputController;

		private void Awake()
		{
			Init();
		}

		public virtual void Init()
		{	
			_RB2D = GetComponent<Rigidbody2D>();
			_inputController = GetComponent<BaseInputController>();
			didInit = true;
		}

		public virtual void LateUpdate()
		{
			CheckGround();
			UpdateMovement();
		}

		public virtual void UpdateMovement()
		{
			if (!didInit)
				Init();

			_inputController.CheckInput();

			Vector2 moveVel = _RB2D.velocity;

			if (isOnGround || canAirSteer)
			{
				// direction keys
				if (_inputController.Left && allow_left)
				{
					moveVel.x = -runPower;
				}
				else if (_inputController.Right && allow_right)
				{
					moveVel.x = runPower;
				}
				else
				{
					if (allow_right || allow_left)
					{
						// stop if no left/right keys are being pressed
						moveVel.x = 0;
					}
				}
			}

			// jump key
			if (_inputController.Fire1 && allow_jump && isOnGround)
			{
				// stop if no left/right keys are being pressed
				moveVel.y = jumpPower;
				Jump();
			}

			_RB2D.velocity = moveVel;
		}

		void CheckGround()
		{
			// assume we're NOT on the ground, first..
			isOnGround = false;

			// Cast a ray straight down.
			RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 0.3f, groundLayerMask);

			// If it hits something...
			if (hit.collider != null)
			{
				isOnGround = true;
			}
		}

		public virtual void Jump()
		{
			// use this function to trigger things that happen when the player jumps
		}
	}
}