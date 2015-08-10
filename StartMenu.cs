using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartMenu : MonoBehaviour {

	private GameObject instructions;
	private GameObject mainMenu;
	//private Toggle alwaysRunToggle;
	private Text difficultyText;
	private PlayerSettings playerSettings;
	
	
	// Use this for initialization
	void Start () {
		mainMenu = GameObject.Find("Main Menu");
		instructions = GameObject.Find("Instructions");
		//toggle = GameObject.Find("AlwaysRunToggle").GetComponent<Toggle>();
		difficultyText = GameObject.Find("DifficultyDisplay").GetComponent<Text>();
		playerSettings = GameObject.FindWithTag("Settings").GetComponent<PlayerSettings>();
		mainMenu.SetActive(true);
		instructions.SetActive(false);
	}
	public void setInstructions() {
		mainMenu.SetActive(false);
		instructions.SetActive(true);
		difficultyText.text = playerSettings.getDifficulty();
	}
	public void setMainMenu() {
		mainMenu.SetActive(true);
		instructions.SetActive(false);
	}
}
