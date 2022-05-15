using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
	public GameObject target;
	public float maxSpeed = 75f;

	private Vector3 toTarget, desired;
	private GameObject explosion;

	void Start()
	{
		explosion = Resources.Load("Prefabs/Explosion") as GameObject;
		toTarget = target.transform.position - transform.position;
		desired = toTarget.normalized * maxSpeed;
		transform.LookAt(target.transform.position);
		Invoke("DestroySelf", 3f);
	}

	void Update()
	{
		transform.position += desired * Time.deltaTime;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject == target)
		{
			Instantiate(explosion, transform.position, transform.rotation);
			DestroySelf();	
		}
	}

	void DestroySelf()
	{
		Destroy(gameObject);
	}
}