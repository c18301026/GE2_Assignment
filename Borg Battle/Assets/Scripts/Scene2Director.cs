using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene2Director : MonoBehaviour
{
	private GameObject borgCube;
	private GameObject earth;
	private Camera camera;
	private ShipBehaviour borgCubeBehaviour;
	private float borgSpeed = 5f;
	private GameObject laserSpawnPos;
	private GameObject laser;

	private AudioSource audioSource;

	void Awake()
	{
		borgCube = GameObject.FindWithTag("BorgCube");
		earth = GameObject.FindWithTag("Earth");
		camera = Camera.main;
		borgCubeBehaviour = borgCube.GetComponent<ShipBehaviour>();
		borgCubeBehaviour.target = earth.transform;
		borgCubeBehaviour.maxSpeed = borgSpeed;
		borgCubeBehaviour.seeking = true;
		laserSpawnPos = GameObject.FindWithTag("LaserSpawnPos");
		laser = Resources.Load("Prefabs/RedLaser") as GameObject;

		audioSource = GetComponent<AudioSource>();
	}

	void Start()
	{
		Invoke("ShootLaser", 11f);
		Invoke("ShootLaser", 12f);
		Invoke("ShootLaser", 13.5f);
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space) || !audioSource.isPlaying)
		{
			SceneManager.LoadScene("Scene3");
		}

		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
			Debug.Log("I want out!");
		}
	}

	void FixedUpdate()
	{
		camera.transform.LookAt(borgCube.transform);
	}

	void ShootLaser()
	{
		var l = Instantiate(laser, laserSpawnPos.transform.position, laserSpawnPos.transform.rotation);
		l.GetComponent<ProjectileBehaviour>().target = borgCube;
	}
}