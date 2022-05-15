using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene3Director : MonoBehaviour
{
	private GameObject ussDefiant, ussDefiantPath;
	private float[] ussDefiantLaserTimeStamps = new float[] {0.1f, 0.3f, 0.5f, 3.0f, 3.2f, 3.4f, 5.2f, 5.4f, 5.6f};
	private GameObject fire;
	private bool onFire;
	private GameObject starfleetLaser;

	private GameObject borgCube;
	private GameObject borgLaser;

	private GameObject ship1, ship2, ship3, ship4;
	private GameObject ship1Path, ship2Path, ship3Path, ship4Path;

	private string[] lines = new string[] {	"WORF:\nReport!",
						"CONN OFFICER:\nMain power is off-line. We've lost shields and our weapons have gone.",
						"WORF:\nPerhaps today is a good day to die. Prepare for ramming speed!",
						"CONN OFFICER:\nSir, there's another starship coming in. ...It's the Enterprise!"};
	private float[] displayTimeStamps = new float[] {15f, 16f, 23f, 28f};
	private float[] hideTimeStamps = new float[] {0f, 21f, 35f};
	private GameObject canvas, dialogueBox, portraitBox;
	private Dialogue dialogueScript;
	private Texture2D borg, crusher, data, hawk, picard, riker, starfleet, troi, connOfficer, worf;
	private Texture2D[] portraits;

	private Camera camera;
	private AudioSource audioSource;

	void Awake()
	{
		ussDefiant = GameObject.FindWithTag("USSDefiant");
		ussDefiantPath = GameObject.FindWithTag("USSDefiantPath");
		ussDefiant.GetComponent<ShipBehaviour>().path = ussDefiantPath.GetComponent<Path>();
		ussDefiant.GetComponent<ShipBehaviour>().followingPath = true;
		ussDefiant.GetComponent<ShipBehaviour>().maxSpeed = 10f;
		fire = Resources.Load("Prefabs/Fire") as GameObject;
		starfleetLaser = Resources.Load("Prefabs/RedLaser") as GameObject;

		borgCube = GameObject.FindWithTag("BorgCube");
		borgLaser = Resources.Load("Prefabs/BorgLaser") as GameObject;

		ship1 = GameObject.FindWithTag("Ship1");
		ship1Path = GameObject.FindWithTag("Ship1Path");
		ship1.GetComponent<ShipBehaviour>().path = ship1Path.GetComponent<Path>();
		ship1.GetComponent<ShipBehaviour>().followingPath = true;
		ship1.GetComponent<ShipBehaviour>().maxSpeed = 13f;

		ship2 = GameObject.FindWithTag("Ship2");
		ship2Path = GameObject.FindWithTag("Ship2Path");
		ship2.GetComponent<ShipBehaviour>().path = ship2Path.GetComponent<Path>();
		ship2.GetComponent<ShipBehaviour>().followingPath = true;
		ship2.GetComponent<ShipBehaviour>().maxSpeed = 13f;

		ship3 = GameObject.FindWithTag("Ship3");
		ship3Path = GameObject.FindWithTag("Ship3Path");
		ship3.GetComponent<ShipBehaviour>().path = ship3Path.GetComponent<Path>();
		ship3.GetComponent<ShipBehaviour>().followingPath = true;
		ship3.GetComponent<ShipBehaviour>().maxSpeed = 13f;

		ship4 = GameObject.FindWithTag("Ship4");
		ship4Path = GameObject.FindWithTag("Ship4Path");
		ship4.GetComponent<ShipBehaviour>().path = ship4Path.GetComponent<Path>();
		ship4.GetComponent<ShipBehaviour>().followingPath = true;
		ship4.GetComponent<ShipBehaviour>().maxSpeed = 13f;

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

		portraits = new Texture2D[] {worf, connOfficer, worf, connOfficer};

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
		StartCoroutine(ChangeCameraAngle(72, 18, 50, 7.5f));
		StartCoroutine(ChangeCameraAngle(0, 50, -50, 28.5f));

		for(int i = 0; i < ussDefiantLaserTimeStamps.Length; i++)
		{
			StartCoroutine(ShootLaser(ussDefiant, borgCube, ussDefiantLaserTimeStamps[i]));
		}

		for(int i = 0; i < 36; i += 4)
		{
			StartCoroutine(ShootLaser(ship1, borgCube, (float)i));
			StartCoroutine(ShootLaser(ship2, borgCube, (float)(i + 2)));
			StartCoroutine(ShootLaser(ship3, borgCube, (float)(i + 1)));
			StartCoroutine(ShootLaser(ship4, borgCube, (float)(i + 3)));

			StartCoroutine(ShootLaser(borgCube, ship1, (float)(i + 2)));
			StartCoroutine(ShootLaser(borgCube, ship2, (float)(i + 1)));
			StartCoroutine(ShootLaser(borgCube, ship3, (float)(i + 3)));
			StartCoroutine(ShootLaser(borgCube, ship4, (float)i));
		}

		StartCoroutine(ShootLaser(borgCube, ussDefiant, 7.5f));
		Invoke("setOnFire", 8f);
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space) || !audioSource.isPlaying)
		{
			SceneManager.LoadScene("Scene4");
		}

		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
			Debug.Log("I want out!");
		}
	}

	void FixedUpdate()
	{
		camera.transform.LookAt(ussDefiant.transform);
		borgCube.transform.position = new Vector3(0, 0, 60);
		borgCube.transform.rotation = Quaternion.Euler(0, 0, 0);

		if(onFire)
		{
			fire.transform.position = ussDefiant.transform.position;
		}
	}

	IEnumerator ShootLaser(GameObject shooter, GameObject target, float timeStamp)
	{
		yield return new WaitForSeconds(timeStamp);

		if(shooter.gameObject.tag == "BorgCube")
		{
			var l = Instantiate(borgLaser, shooter.transform.position, shooter.transform.rotation);
			l.GetComponent<ProjectileBehaviour>().target = target;
			l.GetComponent<ProjectileBehaviour>().maxSpeed = 150f;
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

	void setOnFire()
	{
		fire = Instantiate(fire, ussDefiant.transform.position, Quaternion.Euler(-90, 0, 0));
		onFire = true;
		ussDefiant.GetComponent<ShipBehaviour>().maxSpeed = 5f;
	}
}