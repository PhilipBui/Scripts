using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	public float moveSpeed = 10f;
	public int health = 100;
	private float lastAttack = 0.0f;
	public int score = 0;
	private float lastScore = 0.0f;
	public float jumpSpeed = 22f;
	public float gravity = 35f;
	private float verticalSpeed = 2f;
	private CharacterController controller;
	private Vector3 moveDirection = Vector3.zero;
	private AudioSource[] audios;
	private AudioSource coinPickup;
	private AudioSource playerHurt;
	private AudioSource playerJump;
	private AudioSource playerDeath;
	private Text healthText;
	private Text scoreText;
	private Text timerText;
	private float startTime = 0.0f;
	private bool jump = false;
	private bool flip = false;
	public float minFov = 15f;
	public float maxFov = 90f;
	public float scrollSpeed = 10f;
	private bool climb = false;
	private GameObject deathScreen;
	private GameObject pauseScreen;
	private GameObject victoryScreen;
	private GameObject alarm;
	private Elevator elevator;
	public bool run = false;
	public bool alwaysRun = false;
	public bool dead = false;
	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController>();
		audios = GetComponents<AudioSource>();
		coinPickup = audios[0];
		playerHurt = audios[1];
		playerJump = audios[2];
		playerDeath = audios[3];
		healthText = GameObject.Find("HealthDisplay").GetComponent<Text>();
		healthText.text = "Health : " + health;
		scoreText = GameObject.Find("ScoreDisplay").GetComponent<Text>();
		scoreText.text = "Score : " + score + " / 14";
		timerText = GameObject.Find("TimerDisplay").GetComponent<Text>();
		startTime = Time.time;
		deathScreen = GameObject.Find("DeathScreen");
		deathScreen.SetActive(false);
		pauseScreen = GameObject.Find("PauseScreen");
		pauseScreen.SetActive(false);
		victoryScreen = GameObject.Find("VictoryScreen");
		victoryScreen.SetActive(false);
		alarm = GameObject.FindWithTag("Alarm");
		elevator = GameObject.FindWithTag("Elevator").GetComponent<Elevator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!deathScreen.activeSelf && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))) {
			setPause();
		}
		if (!pauseScreen.activeSelf && !deathScreen.activeSelf && !victoryScreen.activeSelf && Time.time - lastAttack >= 0.5f) {
			timerText.text = "Timer : " + (Time.time - startTime).ToString("#.00");
			float fov = Camera.main.fieldOfView;
			fov -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
			fov = Mathf.Clamp(fov, minFov, maxFov);
			Camera.main.fieldOfView = fov;
			if (health <= 0) {
				playerDead();
			}
			if (!alwaysRun && (Input.GetKey("left shift") || Input.GetKey("right shift"))) {
				if (run)
					run = false;
				else
					run = true;
			}
			if (climb)
				transform.position = new Vector3(transform.position.x, transform.position.y + Input.GetAxis("Vertical"), transform.position.z);
			else {			
				if (!jump && Input.GetButtonDown("Jump")) {
					jump = true;
					if (flip)
						flip = false;
					animation.Play("jump");
					verticalSpeed = jumpSpeed;
				}
				else if (controller.isGrounded == false) {
					if (Input.GetButtonDown("Jump") && flip == false) {
						playerJump.Play();
						animation.Play("flip");
						verticalSpeed += jumpSpeed/2;
						flip = true;
					}
					verticalSpeed -= gravity * Time.deltaTime;
				}
				else { 
					jump = false;
					verticalSpeed = 0f;
				}
				moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
				moveDirection *= moveSpeed * Time.deltaTime;
				moveDirection.y = verticalSpeed * Time.deltaTime;
				moveDirection = transform.TransformDirection(moveDirection);
				if (controller.isGrounded && Input.GetButton("Vertical")) {
					if (alwaysRun || run) {
						moveDirection *= 2; 
						animation.Play("run");
					}
					else
						animation.Play("walk");
				}
				else if (!animation.isPlaying)
					animation.Play("idle");
				controller.Move(moveDirection);
			}
		}
	}
	void OnControllerColliderHit(ControllerColliderHit hit) {
		GameObject collision = hit.gameObject;
		if (collision.CompareTag("Coin") && Time.time - lastScore >= 0.5f) {
			lastScore = Time.time;
			Destroy(collision.transform.parent.gameObject);
			score += 1;
			scoreText.text = "Score : " + score + " / 14";
			coinPickup.Play();
			if (score >= 14) {
				elevator.openDoor();
			}
		}
		else if (collision.CompareTag("Enemy") && !deathScreen.activeSelf) {
			if (Time.time - lastAttack >= 0.8f) {
				playerHurt.Play();
				lastAttack = Time.time;
				health -= 10;
				animation.Play("death");
				healthText.text = "Health : " + health;
				EnemyAI enemyAIScript = collision.GetComponent<EnemyAI>();
				enemyAIScript.givePlayerPosition(transform.position);
				enemyAIScript.setAlert();
			}
		}
	}
	void OnTriggerEnter (Collider hit) {
		if (hit.CompareTag("Ladder")) {
			climb = true;
		}
		else if (hit.CompareTag("Elevator") && !victoryScreen.activeSelf) {
			GameObject.FindWithTag("Alarm").SetActive(false);
			alarm.SetActive(false);
			elevator.closeDoor();
			victoryScreen.SetActive(true);
		}
	}
	void OnTriggerExit (Collider hit) {
		if (hit.CompareTag("Ladder")) {
			climb = false;
		}
	}
	public void setPause() {
		if (pauseScreen.activeSelf) {
			pauseScreen.SetActive(false);
			Time.timeScale = 1;
		}
		else if (!pauseScreen.activeSelf) {
			pauseScreen.SetActive(true);
			Time.timeScale = 0;
		}
	}
	public void playerDead() {
		if (!dead) {
			dead = true;
			playerDeath.Play();
			animation.Play("death");
		}
		alarm.SetActive(false);
		deathScreen.SetActive(true);
	}
	public void setAlwaysRun() {
		alwaysRun = true;
	}
	public void unsetAlwaysRun() {
		alwaysRun = false;
	}
	
}
