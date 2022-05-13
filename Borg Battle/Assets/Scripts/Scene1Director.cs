using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
						"FLEET COMMUNICATIONS:\nAll units open fire. ...They've broken through the defence perimeter. ... Continue to attack. ...We need reinforcements. ...Ninety-six dead and twenty-two wounded on the Lexington."
						};
	private float[] displayTimeStamps = new float[] {	0f,
								2.2f,
								3.7f,
								11.5f,
								15f,
								18.25f,
								32.4f,
								48.8f
								};
	private float[] hideTimeStamps = new float[] {	9f,
							17f};
	private GameObject canvas, dialogueBox, portraitBox;
	private Dialogue dialogueScript;
	private Texture2D crusher, data, hawk, picard, riker, troi, connOfficer, wolf;
	private Texture2D[] portraits;

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

		crusher = Resources.Load("Portraits/crusher") as Texture2D;
		data = Resources.Load("Portraits/data") as Texture2D;
		hawk = Resources.Load("Portraits/hawk") as Texture2D;
		picard = Resources.Load("Portraits/picard") as Texture2D;
		riker = Resources.Load("Portraits/riker") as Texture2D;
		troi = Resources.Load("Portraits/troi") as Texture2D;
		connOfficer = Resources.Load("Portraits/connOfficer") as Texture2D;
		wolf = Resources.Load("Portraits/wolf") as Texture2D;

		portraits = new Texture2D[] {	troi,
						picard,
						troi,
						picard,
						data,
						data,
						data,
						data
						};

		dialogueScript = dialogueBox.GetComponent<Dialogue>();
		dialogueScript.canvas = canvas;
		dialogueScript.portraitBox = portraitBox;
		dialogueScript.lines = lines;
		dialogueScript.displayTimeStamps = displayTimeStamps;
		dialogueScript.hideTimeStamps = hideTimeStamps;
		dialogueScript.portraits = portraits;
	}

	void Start()
	{
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.E))
		{
			CameraAngle(-10f, 5f, 25f);
		}
		if(Input.GetKeyDown(KeyCode.R))
		{
			CameraAngle(10f, 5f, 25f);
		}
		if(Input.GetKeyDown(KeyCode.T))
		{
			CameraAngle(0f, 10f, 50f);
		}
		if(Input.GetKeyDown(KeyCode.Space))
		{
			turningAround = true;
			Invoke("GoBack", 0.5f);
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

	void CameraAngle(float x, float y, float z)
	{
		lookAt = true;
		camera.transform.position = new Vector3(enterprise.transform.position.x + x, enterprise.transform.position.y + y, enterprise.transform.position.z + z);
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