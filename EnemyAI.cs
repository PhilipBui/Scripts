using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour {
	private Transform target;
	private Transform myTransform;
	public Vector3 originalPosition;
	public Vector3 targetPosition;
	public Vector3 lastSeenPosition;
	private float lastSeen = 0f;
	public float fieldOfView = 90f;
	public float rangeOfView = 30f;
	public bool alert = false;
	public float moveSpeed = 20f;
	public float rotationSpeed = 5f;
	private GameSettings alarmScript;
	public List<GameObject> inCollider;
	private NavMeshAgent agent;
	public bool patrol = true;
	// Use this for initialization
	void Start () {
		target = GameObject.FindWithTag("Player").transform;
		myTransform = gameObject.transform;
		if (originalPosition == new Vector3(0, 0, 0)) {
			Debug.Log("Should set an original position!");
			originalPosition = myTransform.position;
		}
		alarmScript = GameObject.FindWithTag("Alarm").GetComponent<GameSettings>();
		string difficulty = alarmScript.getDifficulty();
		if (difficulty == "Easy") {
			fieldOfView *= 0.75f;
		}
		else if (difficulty == "Medium") {
			rangeOfView *= 1.25f;
			moveSpeed *= 2.0f;
		}
		else if (difficulty == "Hard") {
			fieldOfView *= 1.1f;
			rangeOfView *= 1.5f;
			moveSpeed *= 4.0f;
		}
		else 
			Debug.Log("EnemyAI: Difficulty could not be found : " + difficulty);
		inCollider = new List<GameObject>();
		agent = GetComponent<NavMeshAgent> ();
		if (originalPosition == targetPosition || targetPosition == new Vector3(0, 0, 0))
			patrol = false;
		else {
			patrol = true;
			agent.SetDestination (targetPosition);
			agent.speed = moveSpeed/4;
		}
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 direction = target.position - myTransform.position;
		direction.Normalize();
		if ((Vector3.Angle(direction, transform.forward)) <= fieldOfView * 0.5f) {
			RaycastHit hit;
			if (Physics.Raycast(transform.position, direction, out hit, rangeOfView)) {
				if (hit.collider.CompareTag("Player")) {
					lastSeen = Time.time;
					lastSeenPosition = hit.collider.transform.position;
					if (alert == false) {
						audio.Play();
						setAlert();
					}
					if (alert == true) {
						agent.SetDestination (lastSeenPosition);
						agent.speed = moveSpeed;
						foreach (GameObject o in inCollider) {
							EnemyAI enemyAIScript = o.GetComponent<EnemyAI>();
							if (!enemyAIScript.isAlert()) {
								enemyAIScript.givePlayerPosition(lastSeenPosition);
							}
						}
					}
				}
			}
		}
		if (alert == true) {
			if (!alarmScript.isActive()) {
				alarmScript.setActive();
			}
			if (Time.time - lastSeen > 5) {
				alert = false;
				agent.speed = moveSpeed/4;
				agent.SetDestination (originalPosition);
				alarmScript.setInactive();
			}
		}
		else if (patrol && !alert && !agent.hasPath) {
			if (myTransform.position.x == originalPosition.x && myTransform.position.z == originalPosition.z)
				agent.SetDestination (targetPosition);
			else if (myTransform.position.x == targetPosition.x && myTransform.position.z == targetPosition.z)
				agent.SetDestination (originalPosition);
			else 
				agent.SetDestination (originalPosition);
		}
	}
	//When an enemy enters the trigger, remove it from the list.
	void OnTriggerEnter(Collider other){
		if (other.CompareTag("Enemy")) {
			GameObject go = other.gameObject;
			if(!inCollider.Contains(go))
				inCollider.Add(go);
		}
	}
	// When an enemy exits the trigger, remove it from the list.
	void OnTriggerExit(Collider other){
		if (other.CompareTag("Enemy")) {
		  GameObject go = other.gameObject;
		  inCollider.Remove(go);
		}
	}
	public void givePlayerPosition(Vector3 lastSeenPos) {
		lastSeenPosition = lastSeenPos;
		agent.SetDestination(lastSeenPosition);
	}
	public bool isAlert() {
		if (!alert)
			return false;
		else
			return true;
	}
	public void setAlert() {
		alert = true;
		lastSeen = Time.time;
		alarmScript.setActive();
	}
}
