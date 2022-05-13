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
	}

	void Update()
	{
		camera.transform.LookAt(borgCube.transform);
	}
}