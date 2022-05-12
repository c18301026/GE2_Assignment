using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene1Director : MonoBehaviour
{
	private GameObject enterprise;
	private GameObject target;
	private Camera camera;
	private float targetSpeed = 5f;
	private bool lookAt;

	void Awake()
	{
		enterprise = GameObject.FindWithTag("Enterprise");
		target = GameObject.FindWithTag("Target");
		camera = Camera.main;
	}

	void Start()
	{
		ShipBehaviour enterpriseBehaviour = enterprise.GetComponent<ShipBehaviour>();
		enterpriseBehaviour.target = target.transform;
		enterpriseBehaviour.maxSpeed = targetSpeed;
		enterpriseBehaviour.seeking = true;
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.E))
		{
			CameraAngle1();
		}
		if(Input.GetKeyDown(KeyCode.R))
		{
			CameraAngle2();
		}
		if(Input.GetKeyDown(KeyCode.T))
		{
			CameraAngle3();
		}
	}

	void FixedUpdate()
	{
		target.transform.position += new Vector3(0, 0, targetSpeed * Time.deltaTime);

		if(lookAt)
		{
			camera.transform.LookAt(enterprise.transform);
		}
	}

	void CameraAngle1()
	{
		lookAt = true;
		camera.transform.position = new Vector3(enterprise.transform.position.x - 10, enterprise.transform.position.y + 5, enterprise.transform.position.z + 25);
	}

	void CameraAngle2()
	{
		lookAt = true;
		camera.transform.position = new Vector3(enterprise.transform.position.x + 10, enterprise.transform.position.y + 5, enterprise.transform.position.z + 25);
	}

	void CameraAngle3()
	{
		lookAt = true;
		camera.transform.position = new Vector3(enterprise.transform.position.x, enterprise.transform.position.y + 10, enterprise.transform.position.z + 50);
	}
}