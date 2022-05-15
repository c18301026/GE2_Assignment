using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene4Director : MonoBehaviour
{
	private GameObject ussDefiant, ussDefiantPath;
	private GameObject starfleetLaser;

	private GameObject enterprise, enterprisePath;
	private GameObject barrier;

	private GameObject ship1, ship2, ship3, ship4;
	private GameObject ship1Path, ship2Path, ship3Path, ship4Path;

	private GameObject borgCube;
	private GameObject borgLaser;

	private Camera camera;
	private AudioSource audioSource;

	private string[] lines = new string[] {	"RIKER:\nThe Defiant's losing life support.",
						"PICARD:\nBridge to transporter room three. Beam the Defiant survivors aboard.",
						"RIKER:\nCaptain, the Admiral's ship has been destroyed.",
						"PICARD:\nWhat is the status of the Borg cube?",
						"DATA:\nIt has sustained heavy damage to its outer hull. I am reading fluctuations in their power grid.",
						"PICARD:\nOn screen.",
						"PICARD:\n...Number One, open a channel to the fleet.",
						"RIKER:\nChannel open.",
						"PICARD:\nThis is Captain Picard of the Enterprise. I am taking command of the fleet. Target all of your weapons onto the following coordinates. ...Fire on my command.",
						"DATA:\nSir, the coordinates you have indicated do not appear to be a vital system.",
						"PICARD:\nTrust me Data."};
	private float[] displayTimeStamps = new float[] {	8f,
								9.5f,
								13f,
								16.75f,
								18.75f,
								24.25f,
								33.5f,
								37.5f,
								38.75f,
								47f,
								51f
								};
	private float[] hideTimeStamps = new float[] {0f, 16f, 28f, 36f};
	private GameObject canvas, dialogueBox, portraitBox;
	private Dialogue dialogueScript;
	private Texture2D borg, crusher, data, hawk, picard, riker, starfleet, troi, connOfficer, worf;
	private Texture2D[] portraits;

	void Awake()
	{
		ussDefiant = GameObject.FindWithTag("USSDefiant");
		ussDefiantPath = GameObject.FindWithTag("USSDefiantPath");
		ussDefiant.GetComponent<ShipBehaviour>().path = ussDefiantPath.GetComponent<Path>();
		ussDefiant.GetComponent<ShipBehaviour>().followingPath = true;
		ussDefiant.GetComponent<ShipBehaviour>().maxSpeed = 5f;
		starfleetLaser = Resources.Load("Prefabs/RedLaser") as GameObject;

		enterprise = GameObject.FindWithTag("Enterprise");
		enterprisePath = GameObject.FindWithTag("EnterprisePath");
		enterprise.GetComponent<ShipBehaviour>().path = enterprisePath.GetComponent<Path>();
		enterprise.GetComponent<ShipBehaviour>().followingPath = true;
		enterprise.GetComponent<ShipBehaviour>().maxSpeed = 16f;
		barrier = Resources.Load("Prefabs/Barrier") as GameObject;

		borgCube = GameObject.FindWithTag("BorgCube");
		borgLaser = Resources.Load("Prefabs/BorgLaser") as GameObject;

		ship1 = GameObject.FindWithTag("Ship1");
		ship1Path = GameObject.FindWithTag("Ship1Path");
		ship1.GetComponent<ShipBehaviour>().path = ship1Path.GetComponent<Path>();
		ship1.GetComponent<ShipBehaviour>().followingPath = true;
		ship1.GetComponent<ShipBehaviour>().maxSpeed = 12f;

		ship2 = GameObject.FindWithTag("Ship2");
		ship2Path = GameObject.FindWithTag("Ship2Path");
		ship2.GetComponent<ShipBehaviour>().path = ship2Path.GetComponent<Path>();
		ship2.GetComponent<ShipBehaviour>().followingPath = true;
		ship2.GetComponent<ShipBehaviour>().maxSpeed = 12f;

		ship3 = GameObject.FindWithTag("Ship3");
		ship3Path = GameObject.FindWithTag("Ship3Path");
		ship3.GetComponent<ShipBehaviour>().path = ship3Path.GetComponent<Path>();
		ship3.GetComponent<ShipBehaviour>().followingPath = true;
		ship3.GetComponent<ShipBehaviour>().maxSpeed = 11f;

		ship4 = GameObject.FindWithTag("Ship4");
		ship4Path = GameObject.FindWithTag("Ship4Path");
		ship4.GetComponent<ShipBehaviour>().path = ship4Path.GetComponent<Path>();
		ship4.GetComponent<ShipBehaviour>().followingPath = true;
		ship4.GetComponent<ShipBehaviour>().maxSpeed = 11f;

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

		portraits = new Texture2D[] {riker, picard, riker, picard, data, picard, picard, riker, picard, data, picard};

		dialogueScript = dialogueBox.GetComponent<Dialogue>();
		dialogueScript.canvas = canvas;
		dialogueScript.portraitBox = portraitBox;
		dialogueScript.lines = lines;
		dialogueScript.displayTimeStamps = displayTimeStamps;
		dialogueScript.hideTimeStamps = hideTimeStamps;
		dialogueScript.portraits = portraits;

		camera = Camera.main;
		audioSource = GetComponent<AudioSource>();
	}

	void Start()
	{
		for(int i = 0; i < 53; i += 4)
		{
			StartCoroutine(ShootLaser(ship1, borgCube, (float)i));
			StartCoroutine(ShootLaser(ship2, borgCube, (float)(i + 2)));
			StartCoroutine(ShootLaser(ship3, borgCube, (float)(i + 1)));
			StartCoroutine(ShootLaser(ship4, borgCube, (float)(i + 3)));

			StartCoroutine(ShootLaser(borgCube, ship1, (float)(i + 2)));
			StartCoroutine(ShootLaser(borgCube, ship2, (float)(i + 1)));
			StartCoroutine(ShootLaser(borgCube, ship3, (float)(i + 3)));
			StartCoroutine(ShootLaser(borgCube, ship4, (float)i));
			StartCoroutine(ShootLaser(borgCube, ship1, (float)(i + 6)));
			StartCoroutine(ShootLaser(borgCube, ship2, (float)(i + 5)));
			StartCoroutine(ShootLaser(borgCube, ship3, (float)(i + 7)));
			StartCoroutine(ShootLaser(borgCube, ship4, (float)(i + 4)));
		}

		StartCoroutine(ShootLaser(borgCube, enterprise, 5f));
		StartCoroutine(ShootLaser(borgCube, enterprise, 6.3f));
		Invoke("BringUpBarrier", 5.2f);
		Invoke("BringUpBarrier", 6.5f);

		StartCoroutine(ChangeCameraAngle(75, 1, 50, 8f));
		StartCoroutine(ChangeCameraAngle(0, 100, 0, 24.5f));
		StartCoroutine(ChangeCameraAngle(0, 1, 0, 38.5f));
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space) || !audioSource.isPlaying)
		{
			//SceneManager.LoadScene("Scene4");
			Debug.Log("Test");
		}

		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
			Debug.Log("I want out!");
		}
	}

	void FixedUpdate()
	{
		camera.transform.LookAt(enterprise.transform);
		borgCube.transform.position = new Vector3(0, 0, 60);
		borgCube.transform.rotation = Quaternion.Euler(0, 0, 0);
	}

	IEnumerator ShootLaser(GameObject shooter, GameObject target, float timeStamp)
	{
		yield return new WaitForSeconds(timeStamp);

		if(shooter.gameObject.tag == "BorgCube")
		{
			var l = Instantiate(borgLaser, shooter.transform.position, shooter.transform.rotation);
			l.GetComponent<ProjectileBehaviour>().target = target;
			l.GetComponent<ProjectileBehaviour>().maxSpeed = 200f;
		}
		else
		{
			var l = Instantiate(starfleetLaser, shooter.transform.position, shooter.transform.rotation);
			l.GetComponent<ProjectileBehaviour>().target = target;
		}
	}

	IEnumerator ChangeCameraAngle(float x, float y, float z, float timeStamp)
	{
		yield return new WaitForSeconds(timeStamp);
		camera.transform.position = new Vector3(x, y, z);
	}

	void BringUpBarrier()
	{
		var b = Instantiate(barrier, enterprise.transform.position, Quaternion.Euler(0, 0, 0));
		Destroy(b, 0.25f);
	}
}