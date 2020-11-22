using UnityEngine;
using System.Collections;

namespace GPC
{

	[AddComponentMenu("CSharpBookCode/Common/Horizontal-only Keyboard Input With Firing")]

	public class PlayerInput2WayFire : BaseInputController
	{

		public override void CheckInput()
		{
			// get input data from vertical and horizontal axis and store them internally in vert and horz so we don't
			// have to access them every time we need to relay input data out
			horz = Input.GetAxis("Horizontal");

			// set up some boolean values for up, down, left and right
			Left = (horz < 0);
			Right = (horz > 0);

			// get fire / action buttons
			Fire1 = Input.GetButton("Fire1");
		}

		public void LateUpdate()
		{
			// check inputs each LateUpdate() ready for the next tick
			CheckInput();
		}
	}
}