using UnityEngine;
using GPC;

public class RaceInputController : BaseInputController
{
	public override void CheckInput()
	{
		switch (inputType)
		{
			case InputTypes.player1:
				Up = Input.GetKey(KeyCode.UpArrow);
				Down = Input.GetKey(KeyCode.DownArrow);
				Left = Input.GetKey(KeyCode.LeftArrow);
				Right = Input.GetKey(KeyCode.RightArrow);
				break;
			case InputTypes.noInput:
				// nothing here!
				break;
		}

		vert = 0;
		horz = 0;

		if (Up)
			vert += 1;

		if (Down)
			vert -= 1;

		if (Left)
			horz -= 1;

		if (Right)
			horz += 1;
	}

	public void Update()
	{
		CheckInput();
	}
}