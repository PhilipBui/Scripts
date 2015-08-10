using UnityEngine;
using System.Collections;

public class PlayerSettings : MonoBehaviour {

	public bool hardcoreMode = false;
	public string difficulty = "Medium";
	public bool alwaysRun = false;
	// Use this for initialization
	void Start () {
		Object.DontDestroyOnLoad(gameObject);
	}
	public bool getHardcoreMode() {
		if (hardcoreMode)
			return true;
		else
			return false;
	}
	public void setHardcoreMode(bool set) {
		if (set)
			hardcoreMode = true;
		else
			hardcoreMode = false;
	}
	public bool getAlwaysRun() {
		if (alwaysRun)
			return true;
		else
			return false;
	}
	public void setAlwaysRun(bool set) {
		if (set)
			alwaysRun = true;
		else
			alwaysRun = false;
	}
	public string getDifficulty() {
		return difficulty;
	}
	public void setDifficulty(string difficulty2) {
		if (difficulty2 == "Easy")
			difficulty = "Easy";
		else if (difficulty2 == "Medium")
			difficulty = "Medium";
		else if (difficulty2 == "Hard")
			difficulty = "Hard";
		else
			Debug.Log("PlayerSettings: Difficulty could not be found");
	}
}
