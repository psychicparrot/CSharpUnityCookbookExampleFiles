using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GPC;

public class PositionGraphicSetter : MonoBehaviour
{
	public RacePlayerController _racePlayerController;
	public Text posDisplay;

	private string theDisplayString;

    void LateUpdate()
    {
		switch (_racePlayerController.myRacePosition)
		{
			case 1:
				theDisplayString = "1st";
				break;
			case 2:
				theDisplayString = "2nd";
				break;
			case 3:
				theDisplayString = "3rd";
				break;
			case 4:
				theDisplayString = "4th";
				break;
			case 5:
				theDisplayString = "5th";
				break;
			case 6:
				theDisplayString = "6th";
				break;
			case 7:
				theDisplayString = "7th";
				break;
			case 8:
				theDisplayString = "8th";
				break;
			case 9:
				theDisplayString = "9th";
				break;
			case 10:
				theDisplayString = "10th";
				break;

		}

		posDisplay.text = theDisplayString;
    }
}
