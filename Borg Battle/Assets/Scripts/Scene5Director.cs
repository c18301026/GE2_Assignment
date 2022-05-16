using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene5Director : MonoBehaviour
{
	private GameObject ussDefiant, enterprise, starfleetLaser;
	private GameObject borgCube, borgLaser;

	private Camera camera;
	private AudioSource audioSource;

	void Awake()
	{
		ussDefiant = GameObject.FindWithTag("USSDefiant");
		enterprise = GameObject.FindWithTag("Enterprise");
		starfleetLaser = Resources.Load("Prefabs/RedLaser") as GameObject;

		borgCube = GameObject.FindWithTag("BorgCube");
		borgLaser = Resources.Load("Prefabs/BorgLaser") as GameObject;

		camera = Camera.main;
		audioSource = GetComponent<AudioSource>();
	}

	void Start()
	{

	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
			Debug.Log("I want out!");
		}
	}

	void FixedUpdate()
	{
		//camera.transform.LookAt(enterprise.transform);
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
}