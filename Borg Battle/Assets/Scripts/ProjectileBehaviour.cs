using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
	public GameObject target;

	private Vector3 toTarget, desired;
	private float maxSpeed = 75f;

	void Start()
	{
		toTarget = target.transform.position - transform.position;
		desired = toTarget.normalized * maxSpeed;
		transform.LookAt(target.transform.position);
		Invoke("DestroySelf", 1f);
	}

	void Update()
	{
		transform.position += desired * Time.deltaTime;
	}

	/*
	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject == target)
		{
			
		}
	}
	*/

	void DestroySelf()
	{
		Destroy(gameObject);
	}
}