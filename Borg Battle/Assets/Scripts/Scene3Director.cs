using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene3Director : MonoBehaviour
{
	private GameObject ussDefiant;
	private GameObject ussDefiantPath;
	private float[] ussDefiantLaserTimeStamps = new float[] {0.1f, 0.3f, 0.5f, 3.0f, 3.2f, 3.4f, 5.0f, 5.2f, 5.4f};
	private GameObject starfleetLaser;
	private GameObject borgCube;
	private Camera camera;

	private AudioSource audioSource;

	void Awake()
	{
		ussDefiant = GameObject.FindWithTag("USSDefiant");
		ussDefiantPath = GameObject.FindWithTag("USSDefiantPath");
		ussDefiant.GetComponent<ShipBehaviour>().path = ussDefiantPath.GetComponent<Path>();
		ussDefiant.GetComponent<ShipBehaviour>().followingPath = true;
		ussDefiant.GetComponent<ShipBehaviour>().maxSpeed = 10f;
		borgCube = GameObject.FindWithTag("BorgCube");
		camera = Camera.main;
		starfleetLaser = Resources.Load("Prefabs/RedLaser") as GameObject;

		audioSource = GetComponent<AudioSource>();
	}

	void Start()
	{
		/*
		StartCoroutine(StarfleetShootLaser(ussDefiant, 0.1f));
		StartCoroutine(StarfleetShootLaser(ussDefiant, 0.3f));
		StartCoroutine(StarfleetShootLaser(ussDefiant, 0.5f));
		StartCoroutine(StarfleetShootLaser(ussDefiant, 3.1f));
		StartCoroutine(StarfleetShootLaser(ussDefiant, 3.3f));
		StartCoroutine(StarfleetShootLaser(ussDefiant, 3.5f));
		StartCoroutine(StarfleetShootLaser(ussDefiant, 5.0f));
		StartCoroutine(StarfleetShootLaser(ussDefiant, 5.2f));
		StartCoroutine(StarfleetShootLaser(ussDefiant, 5.4f));*/
		for(int i = 0; i < ussDefiantLaserTimeStamps.Length; i++)
		{
			StartCoroutine(StarfleetShootLaser(ussDefiant, ussDefiantLaserTimeStamps[i]));
		}
	}

	void FixedUpdate()
	{
		camera.transform.LookAt(ussDefiant.transform);
	}

	IEnumerator StarfleetShootLaser(GameObject ship, float timeStamp)
	{
		yield return new WaitForSeconds(timeStamp);
		var l = Instantiate(starfleetLaser, ship.transform.position, ship.transform.rotation);
		l.GetComponent<ProjectileBehaviour>().target = borgCube;
	}
}