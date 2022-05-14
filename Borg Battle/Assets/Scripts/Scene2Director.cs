using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene2Director : MonoBehaviour
{
	private GameObject borgCube;
	private GameObject earth;
	private Camera camera;
	private ShipBehaviour borgCubeBehaviour;
	private float borgSpeed = 7.5f;
	private GameObject laserSpawnPos;
	private GameObject laser;
	private float[] laserTimeStamps;

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
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			var l = Instantiate(laser, laserSpawnPos.transform.position, laserSpawnPos.transform.rotation);
			l.GetComponent<ProjectileBehaviour>().target = borgCube;
		}
	}

	void FixedUpdate()
	{
		camera.transform.LookAt(borgCube.transform);
	}
}