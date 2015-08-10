using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameSettings : MonoBehaviour {

	private GameObject normalLight;
	private GameObject alarmLight;
	private AudioSource[] normalLightAudios;
	public bool hardcoreMode = false;
	public string difficulty = "Medium";
	public bool alwaysRun = false;
	// Use this for initialization
	void Awake() {
		PlayerSettings playerSettings = GameObject.FindWithTag("Settings").GetComponent<PlayerSettings>();
		difficulty = playerSettings.getDifficulty();
		hardcoreMode = playerSettings.getHardcoreMode();
		alwaysRun = playerSettings.getAlwaysRun();
	}
	void Start() {
		normalLight = transform.FindChild("Normal Light").gameObject;
		alarmLight = transform.FindChild("Alarm Light").gameObject;
		normalLightAudios = normalLight.GetComponents<AudioSource>();
		PlayerController controller = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
		if (alwaysRun)
			controller.setAlwaysRun();
		else
			controller.unsetAlwaysRun();
		Text difficultyText = GameObject.Find("DifficultyDisplay").GetComponent<Text>();
		difficultyText.text = difficulty;
		if (hardcoreMode) {
			normalLightAudios[0].enabled = false;
			normalLightAudios[1].enabled = true;
			difficultyText.text += " Hardcore";
		}
		else {
			normalLightAudios[0].enabled = true;
			normalLightAudios[1].enabled = false;
		}
		
		
	}
	public bool isActive() {
		if (normalLight.activeSelf)
			return false;
		else if (alarmLight.activeSelf)
			return true;
		else 
			Debug.Log("GameSettings: Both lights are not active.");
		return false;
	}
	public void setActive() {
		if (hardcoreMode) {
			PlayerController controller = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
			controller.playerDead();
		}
		normalLight.SetActive(false);
		alarmLight.SetActive(true);
	}
	public void setInactive() {
		normalLight.SetActive(true);
		alarmLight.SetActive(false);		
	}
	public bool getHardcoreMode() {
		if (hardcoreMode)
			return true;
		else
			return false;
	}
	public string getDifficulty() {
		return difficulty;
	}
}
