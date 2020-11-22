using UnityEngine;
using System.Collections;

namespace GPC
{

	public class PlayerInput4WayFire : BaseInputController
	{

		public override void CheckInput()
		{
			// get input data from vertical and horizontal axis and store them internally in vert and horz so we don't
			// have to access them every time we need to relay input data out
			vert = Input.GetAxis("Vertical");
			horz = Input.GetAxis("Horizontal");

			// set up some boolean values for up, down, left and right
			Up = (vert > 0);
			Down = (vert < 0);
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