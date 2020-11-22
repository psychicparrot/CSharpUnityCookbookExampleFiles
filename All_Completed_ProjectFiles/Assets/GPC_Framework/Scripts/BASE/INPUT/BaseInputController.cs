using UnityEngine;

namespace GPC
{
	[AddComponentMenu("CSharpBookCode/Base/Input Controller")]
	public class BaseInputController : MonoBehaviour
	{
		public enum InputTypes { player1, noInput };
		public InputTypes inputType;

		// directional buttons
		public bool Up;
		public bool Down;
		public bool Left;
		public bool Right;

		// fire / action buttons
		public bool Fire1;

		// weapon slots
		public bool Slot1;
		public bool Slot2;
		public bool Slot3;
		public bool Slot4;
		public bool Slot5;
		public bool Slot6;
		public bool Slot7;
		public bool Slot8;
		public bool Slot9;

		public float vert;
		public float horz;
		public bool shouldRespawn;

		public Vector3 TEMPVec3;
		private Vector3 zeroVector = new Vector3(0, 0, 0);

		public virtual void CheckInput()
		{
			switch (inputType)
			{
				case InputTypes.player1:
					vert = Input.GetAxis("Vertical");
					horz = Input.GetAxis("Horizontal");

					// set up some boolean values for up, down, left and right
					Up = (vert > 0);
					Down = (vert < 0);
					Left = (horz < 0);
					Right = (horz > 0);

					// get fire / action buttons
					Fire1 = Input.GetButton("Fire1");
					break;
				case InputTypes.noInput:
					break;
			}
		}

		public virtual float GetHorizontal()
		{
			return horz;
		}

		public virtual float GetVertical()
		{
			return vert;
		}

		public virtual bool GetFire()
		{
			return Fire1;
		}

		public bool GetRespawn()
		{
			return shouldRespawn;
		}

		public virtual Vector3 GetMovementDirectionVector()
		{
			// temp vector for movement dir gets set to the value of an otherwise unused vector that always have the value of 0,0,0
			TEMPVec3 = zeroVector;

			TEMPVec3.x = horz;
			TEMPVec3.y = vert;

			// return the movement vector
			return TEMPVec3;
		}

		public void SetInputType(InputTypes theType)
		{
			inputType = theType;
		}
	}
}