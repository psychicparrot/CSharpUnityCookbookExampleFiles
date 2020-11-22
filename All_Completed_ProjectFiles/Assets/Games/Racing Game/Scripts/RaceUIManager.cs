using UnityEngine;
using UnityEngine.UI;

public class RaceUIManager : MonoBehaviour
{
	public Text _lapText;

	public GameObject _count3;
	public GameObject _count2;
	public GameObject _count1;
	public GameObject _finalText;
	public GameObject _wrongWaySign;

	public void SetLaps(int aLap, int totalLaps)
	{
		_lapText.text = "Lap " + aLap.ToString("D2") + " of " + totalLaps.ToString("D2");
	}

	public void HideFinalMessage()
	{
		_finalText.SetActive(false);
	}

	public void ShowFinalMessage()
	{
		_finalText.SetActive(true);
	}

	public void HideWrongWay()
	{
		_wrongWaySign.SetActive(false);
	}

	public void ShowWrongWay()
	{
		_wrongWaySign.SetActive(true);
	}

	public void ShowCount(int whichOne)
	{
		switch(whichOne)
		{
			case 0:
				_count1.SetActive(false);
				_count2.SetActive(false);
				_count3.SetActive(false);
				break;
			case 1:
				_count1.SetActive(true);
				_count2.SetActive(false);
				_count3.SetActive(false);
				break;
			case 2:
				_count1.SetActive(false);
				_count2.SetActive(true);
				_count3.SetActive(false);
				break;
			case 3:
				_count1.SetActive(false);
				_count2.SetActive(false);
				_count3.SetActive(true);
				break;
		}
	}
}
