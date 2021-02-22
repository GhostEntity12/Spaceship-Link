using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HighscoreData
{
	public int score;

	public HighscoreData(int s) => score = s;
}

public class GameOver : MonoBehaviour
{
	HighscoreData data;

	private bool gameOver;
	public Animatable gameOverScreen;

	bool transitionGameOver;

	public TextMeshProUGUI scoreText;
	public TextMeshProUGUI highscoreText;
	public TextMeshProUGUI newHighscoreText;

	public void TriggerGameOver(int s)
	{
		gameOver = true;
		gameOverScreen.TriggerAnimation(gameOverScreen.onscreen);

		scoreText.text = $"Final Score:<size=50>\n{s}";

		int previousHighscore;
		try
		{
			previousHighscore = (JsonUtility.FromJson(File.ReadAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + "scores.json"), typeof(HighscoreData)) as HighscoreData).score;
		}
		catch (FileNotFoundException)
		{
			previousHighscore = 0;
		}
		Debug.Log(s + "/" + previousHighscore);
		highscoreText.text = $"High Score<size=50>\n{Mathf.Max(s, previousHighscore)}";
		newHighscoreText.gameObject.SetActive(s > previousHighscore);
		File.WriteAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + "testfile.json", JsonUtility.ToJson(new HighscoreData(Mathf.Max(s, previousHighscore))));

		transitionGameOver = true;
		Spawning.instance.enabled = false;
	}

	// Update is called once per frame
	void Update()
	{
		if (gameOver)
		{
			if (gameOverScreen.animating) gameOverScreen.Animate();

			if (transitionGameOver && !gameOverScreen.animating) transitionGameOver = false;
		}
	}

	public void Restart()
	{
		SceneManager.LoadScene(2);
	}
	public void QuitToMenu() => SceneManager.LoadScene(1);
}
