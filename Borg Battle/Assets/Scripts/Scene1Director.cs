using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene1Director : MonoBehaviour
{
	// Attributes related to the ships/physics
	private GameObject enterprise;
	private GameObject target;
	private Camera camera;
	private ShipBehaviour enterpriseBehaviour;
	private float targetSpeed = 5f;
	private bool lookAt = false;
	private bool turningAround = false;
	private bool goingBack = false;
	private bool faster = false;

	// Attributes related to the dialogue box
	private string[] lines = new string[] {	"TROI (on intercom):\nBridge to Captain Picard.",
						"PICARD:\nGo ahead.",
						"TROI (on intercom):\nWe've just received word from the fleet. They've engaged the Borg.",
						"PICARD:\nData, put Starfleet frequency one four eight six on audio.",
						"DATA:\nAye sir.",
						"FLEET COMMUNICATIONS:\nFlagship to Endeavor. Standby to engage at grid A-fifteen. ...Defiant and Bozeman, fall back to mobile position one. ...Acknowledge. ...We have it in visual range. A Borg cube on course zero point two one five, speed warp point nine six.",
						"BORG COMMUNICATIONS:\nWe are the Borg. Lower your shields and surrender your ships. We will add your biological and technological distinctiveness to our own. Your culture will adapt to service us. Resistance is futile.",
						"FLEET COMMUNICATIONS:\nAll units open fire. ...They've broken through the defence perimeter. ... Continue to attack. ...We need reinforcements. ...Ninety-six dead and twenty-two wounded on the Lexington.",
						"PICARD:\nLieutenant Hawk. Set a course for Earth.",
						"HAWK:\nAye sir.",
						"PICARD:\nMaximum warp. ...I am about to commit a direct violation of our orders. Any of you who wish to object should do so now. It will be noted in my log.",
						"DATA:\nCaptain, I believe I speak for everyone here, sir, when I say ...to hell with our orders.",
						"PICARD:\nRED ALERT! All hands to battle stations.",
						"PICARD:\nEngage."
						};
	private float[] displayTimeStamps = new float[] {	0f,
								2.2f,
								3.7f,
								11.5f,
								15f,
								18.25f,
								32.4f,
								48.8f,
								72f,
								75f,
								76.5f,
								92f,
								105f,
								109.5f
								};
	private float[] hideTimeStamps = new float[] {	9f,
							17f,
							90f,
							102.5f,
							109f,
							112.5f};
	private GameObject canvas, dialogueBox, portraitBox;
	private Dialogue dialogueScript;
	private Texture2D borg, crusher, data, hawk, picard, riker, starfleet, troi, connOfficer, worf;
	private Texture2D[] portraits;

	private AudioSource audioSource;

	void Awake()
	{
		// Ship/physics variables
		enterprise = GameObject.FindWithTag("Enterprise");
		target = GameObject.FindWithTag("Target");
		camera = Camera.main;
		enterpriseBehaviour = enterprise.GetComponent<ShipBehaviour>();
		enterpriseBehaviour.target = target.transform;
		enterpriseBehaviour.maxSpeed = targetSpeed;

		// Dialogue box variables
		canvas = GameObject.FindWithTag("Canvas");
		dialogueBox = GameObject.FindWithTag("DialogueBox");
		portraitBox = GameObject.FindWithTag("PortraitBox");

		borg = Resources.Load("Portraits/Borg") as Texture2D;
		crusher = Resources.Load("Portraits/Crusher") as Texture2D;
		data = Resources.Load("Portraits/Data") as Texture2D;
		hawk = Resources.Load("Portraits/Hawk") as Texture2D;
		picard = Resources.Load("Portraits/Picard") as Texture2D;
		riker = Resources.Load("Portraits/Riker") as Texture2D;
		starfleet = Resources.Load("Portraits/Starfleet") as Texture2D;
		troi = Resources.Load("Portraits/Troi") as Texture2D;
		connOfficer = Resources.Load("Portraits/USS_Defiant_Conn_Officer") as Texture2D;
		worf = Resources.Load("Portraits/Worf") as Texture2D;

		portraits = new Texture2D[] {	troi,
						picard,
						troi,
						picard,
						data,
						starfleet,
						borg,
						starfleet,
						picard,
						hawk,
						picard,
						data,
						picard,
						picard
						};

		dialogueScript = dialogueBox.GetComponent<Dialogue>();
		dialogueScript.canvas = canvas;
		dialogueScript.portraitBox = portraitBox;
		dialogueScript.lines = lines;
		dialogueScript.displayTimeStamps = displayTimeStamps;
		dialogueScript.hideTimeStamps = hideTimeStamps;
		dialogueScript.portraits = portraits;

		audioSource = GetComponent<AudioSource>();
	}

	void Start()
	{
		StartCoroutine(ChangeCameraAngle(10f, 5f, 25f, 11.5f));
		StartCoroutine(ChangeCameraAngle(0f, 10f, 50f, 32.4f));
		StartCoroutine(ChangeCameraAngle(-10f, 5f, 25f, 72f));
		StartCoroutine(ChangeCameraAngle(0f, 10f, 50f, 92f));
		StartCoroutine(ChangeCameraAngle(10f, 5f, 25f, 109.5f));
		Invoke("TurnAround", 111.5f);
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space) || !audioSource.isPlaying)
		{
			SceneManager.LoadScene("Scene2");
		}
	}

	void FixedUpdate()
	{
		if(turningAround)
		{
			enterpriseBehaviour.seeking = true;
			target.transform.position += new Vector3(targetSpeed * Time.deltaTime, 0, 0);
		}
		else if(goingBack || faster)
		{
			target.transform.position += new Vector3(0, 0, -targetSpeed * Time.deltaTime);
		}
		else
		{
			enterprise.transform.position += new Vector3(0, 0, targetSpeed * Time.deltaTime);
			target.transform.position += new Vector3(0, 0, targetSpeed * Time.deltaTime);
		}

		if(lookAt)
		{
			camera.transform.LookAt(enterprise.transform);
		}
	}

	IEnumerator ChangeCameraAngle(float x, float y, float z, float timeStamp)
	{
		yield return new WaitForSeconds(timeStamp);
		lookAt = true;
		camera.transform.position = new Vector3(enterprise.transform.position.x + x, enterprise.transform.position.y + y, enterprise.transform.position.z + z);
	}

	void TurnAround()
	{
		turningAround = true;
		Invoke("GoBack", 0.5f);
	}

	void GoBack()
	{
		turningAround = false;
		goingBack = true;
		Invoke("Engage", 3f);
	}

	void Engage()
	{
		goingBack = false;
		faster = true;
		targetSpeed = 300f;
		enterpriseBehaviour.maxSpeed = targetSpeed;
	}
}