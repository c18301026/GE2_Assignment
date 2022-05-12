using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene1Director : MonoBehaviour
{
	private GameObject enterprise;
	private GameObject target;
	private Camera camera;
	private ShipBehaviour enterpriseBehaviour;
	private float targetSpeed = 5f;
	private bool lookAt = false;
	private bool turningAround = false;
	private bool goingBack = false;
	private bool faster = false;

	void Awake()
	{
		enterprise = GameObject.FindWithTag("Enterprise");
		target = GameObject.FindWithTag("Target");
		camera = Camera.main;
		enterpriseBehaviour = enterprise.GetComponent<ShipBehaviour>();
		enterpriseBehaviour.target = target.transform;
		enterpriseBehaviour.maxSpeed = targetSpeed;
		enterpriseBehaviour.seeking = true;
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
			target.transform.position += new Vector3(targetSpeed * Time.deltaTime, 0, 0);
		}
		else if(goingBack || faster)
		{
			target.transform.position += new Vector3(0, 0, -targetSpeed * Time.deltaTime);
		}
		else
		{
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
		targetSpeed = 200f;
		enterpriseBehaviour.maxSpeed = targetSpeed;
	}
}