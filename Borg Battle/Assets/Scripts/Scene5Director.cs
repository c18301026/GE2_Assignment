using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene5Director : MonoBehaviour
{
	private GameObject ussDefiant, enterprise, starfleetLaser;
	private GameObject borgCube, borgLaser;
	private GameObject borgExplosion;

	private GameObject ship1, ship2, ship3, ship4;
	private GameObject ship1Path, ship2Path, ship3Path, ship4Path;

	private Camera camera;
	private AudioSource audioSource;

	private string[] lines = new string[] {	"RIKER:\nThe fleet's responded, sir. They're standing by.",
						"PICARD:\nFire."};
	private float[] displayTimeStamps = new float[] {0f, 8.5f};
	private float[] hideTimeStamps = new float[] {3f, 11.5f};
	private GameObject canvas, dialogueBox, portraitBox;
	private Dialogue dialogueScript;
	private Texture2D borg, crusher, data, hawk, picard, riker, starfleet, troi, connOfficer, worf;
	private Texture2D[] portraits;

	void Awake()
	{
		ussDefiant = GameObject.FindWithTag("USSDefiant");
		enterprise = GameObject.FindWithTag("Enterprise");
		starfleetLaser = Resources.Load("Prefabs/RedLaser") as GameObject;

		borgCube = GameObject.FindWithTag("BorgCube");
		borgLaser = Resources.Load("Prefabs/BorgLaser") as GameObject;
		borgExplosion = Resources.Load("Prefabs/BorgExplosion") as GameObject;

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

		portraits = new Texture2D[] {riker, picard};

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
		StartCoroutine(ChangeCameraAngle(75f, 20f, 70f, 30f, -90f, 0f, 0f));
		StartCoroutine(ChangeCameraAngle(-40f, 10f, -20f, 30f, 60f, 0f, 4.75f));
		StartCoroutine(ChangeCameraAngle(0f, 20f, -20f, 30f, 0f, 0f, 9.5f));
		StartCoroutine(ChangeCameraAngle(50f, 5f, 18f, 0f, -90f, 0f, 18f));
		StartCoroutine(ChangeCameraAngle(-30f, 3f, 14f, 0f, 45f, 0f, 24f));

		for(float i = 10f; i < 28; i += 0.5f)
		{
			StartCoroutine(ShootLaser(ship1, borgCube, i));
			StartCoroutine(ShootLaser(ship2, borgCube, i + 0.25f));
			StartCoroutine(ShootLaser(ship3, borgCube, i + 0.5f));
			StartCoroutine(ShootLaser(ship4, borgCube, i + 0.75f));
			StartCoroutine(ShootLaser(enterprise, borgCube, i));
		}

		Invoke("StopFollowing", 12f);
		Invoke("DestroyBorgCube", 28.5f);
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
			Debug.Log("I want out!");
		}
	}

	void StopFollowing()
	{
		ship1.GetComponent<ShipBehaviour>().followingPath = false;
		ship2.GetComponent<ShipBehaviour>().followingPath = false;
		ship3.GetComponent<ShipBehaviour>().followingPath = false;
		ship4.GetComponent<ShipBehaviour>().followingPath = false;
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

	IEnumerator ChangeCameraAngle(float xPos, float yPos, float zPos, float xRot, float yRot, float zRot, float timeStamp)
	{
		yield return new WaitForSeconds(timeStamp);
		camera.transform.position = new Vector3(xPos, yPos, zPos);
		camera.transform.rotation = Quaternion.Euler(xRot, yRot, zRot);
	}

	void DestroyBorgCube()
	{
		Instantiate(borgExplosion, borgCube.transform.position, borgCube.transform.rotation);
		Destroy(borgCube);
	}
}