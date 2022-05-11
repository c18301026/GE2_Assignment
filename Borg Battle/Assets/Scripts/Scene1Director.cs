using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene1Director : MonoBehaviour
{
	private GameObject enterprise;
	private GameObject target;
	private Camera camera;
	private float targetSpeed = 5f;

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
		target.transform.position += new Vector3(0, 0, targetSpeed * Time.deltaTime);
	}
}
