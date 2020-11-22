using UnityEngine.UI;
using GPC;
public class BlasterUIManager : CanvasManager
{
	public Text scoreText;
	public Text highText;
	public Text livesText;

	void Start()
	{
		// hide all ui canvas
		HideAll();
	}

	public void ShowGetReadyUI()
	{
		// here we delay showing the get ready message just so it flows nicer
		Invoke("ShowGetReady", 1f);
	}

	void ShowGetReady()
	{
		ShowCanvas(0);
	}

	public void HideGetReadyUI()
	{
		HideCanvas(0);
	}

	public void ShowGameOverUI()
	{
		ShowCanvas(1);
	}

	public void ShowGameUI()
	{
		ShowCanvas(2);
	}

	public void HideGameUI()
	{
		HideCanvas(2);
	}

	public void ShowLevelComplete()
	{
		ShowCanvas(3);
	}

	public void HideLevelComplete()
	{
		HideCanvas(3);
	}

	public void UpdateScoreUI(int anAmount)
	{
		scoreText.text = "SCORE " + anAmount.ToString("D6");
	}

	public void UpdateHighScoreUI(int anAmount)
	{
		highText.text = "HIGHSCORE " + anAmount.ToString("D6");
	}

	public void UpdateLivesUI(int anAmount)
	{
		livesText.text = "LIVES " + anAmount.ToString();
	}

}
