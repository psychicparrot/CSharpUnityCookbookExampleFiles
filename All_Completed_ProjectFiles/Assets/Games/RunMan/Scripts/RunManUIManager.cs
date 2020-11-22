using UnityEngine.UI;
using GPC;

public class RunManUIManager : CanvasManager
{
	// we derive from canvas manager to give us the functionality to fade canvas nicely

	public Text scoreText;
	public Text highText;
	public Text finalScoreText;

	public Text newHighScoreText;

	void Start()
    {
		scoreText.text = "Score 0";
		newHighScoreText.gameObject.SetActive(false);

		// hide all ui canvas
		HideAll(); 
	}

	public void ShowGetReadyUI()
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

	public void SetScore(int scoreAmount)
    {
		scoreText.text = "Score "+scoreAmount.ToString("D5");
	}

	public void SetHighScore(int scoreAmount)
	{
		highText.text = "High " + scoreAmount.ToString("D5");
	}

	public void SetFinalScore(int scoreAmount)
	{
		finalScoreText.text = "Final Score " + scoreAmount.ToString("D5");
	}

	public void ShowGotHighScore()
	{
		newHighScoreText.gameObject.SetActive(true);
	}
}
