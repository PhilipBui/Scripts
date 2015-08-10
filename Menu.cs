using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Menu : MonoBehaviour {
	
	private PlayerSettings playerSettings;

	void Start () {
		playerSettings = GameObject.FindWithTag("Settings").GetComponent<PlayerSettings>();
	}
	public void restartLevel() {
		Time.timeScale = 1;
		Application.LoadLevel(Application.loadedLevel);
	}
	public void startGame() {
		Time.timeScale = 1;
		playerSettings.setHardcoreMode(false);
		Application.LoadLevel ("Underground01");
	}
	public void startGameHC() {
		Time.timeScale = 1;
		playerSettings.setHardcoreMode(true);
		Application.LoadLevel ("Underground01");
	}
	public void startMenu() {
		Application.LoadLevel ("StartMenu");
	}
	public void startGameHarder() {
		string difficulty = playerSettings.getDifficulty();
		if (difficulty == "Easy")
			playerSettings.setDifficulty("Medium");
		else if (difficulty == "Medium")
			playerSettings.setDifficulty("Hard");
		else if (difficulty == "Hard") {
			if (!playerSettings.getHardcoreMode()) 
				playerSettings.setHardcoreMode(true);
		}
		else
			Debug.Log("Congratulations, you finished the hardest difficulty!");
		Application.LoadLevel(Application.loadedLevel);
	}
	public void setDifficulty(string difficulty) {
		playerSettings.setDifficulty(difficulty);
		Text difficultyText = GameObject.Find("DifficultyDisplay").GetComponent<Text>();
		difficultyText.text = difficulty;
	}
	public void setAlwaysRun() {
		Toggle toggle = GameObject.Find("AlwaysRunToggle").GetComponent<Toggle>();
		if (toggle.isOn)
			playerSettings.setAlwaysRun(true);
		else
			playerSettings.setAlwaysRun(false);
	}
}
