using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour {
	private GameObject closedDoor;
	private GameObject openedDoor;
	// Use this for initialization
	void Start () {
		closedDoor = transform.FindChild("Closed Front").gameObject;
		openedDoor = transform.FindChild("Opened Front").gameObject;
	}
	
	public void openDoor () {
		closedDoor.SetActive(false);
		openedDoor.SetActive(true);
	}
	public void closeDoor () {
		closedDoor.SetActive(true);
		openedDoor.SetActive(false);
	}
}
