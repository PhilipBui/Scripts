using UnityEngine;
using System.Collections;

public class Minimap : MonoBehaviour {
	
	private Vector3 rotation;
	// Use this for initialization
	void Start () {
		rotation = new Vector3(90, 0, 0);
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.eulerAngles = rotation;
	}
}
