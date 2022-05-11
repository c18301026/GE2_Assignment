using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBehaviour : MonoBehaviour
{
	// Ship states
	public bool seeking;
	public bool arriving;
	public bool followingPath;

	public Transform target;
	public Path path;
	public float maxSpeed;

	private Vector3 force;
	private Vector3 acceleration;
	private float mass = 1f;
	private Vector3 velocity = new Vector3(0, 0, 0);
	private float speed;
	private float banking = 0.1f;
	private float damping = 0.1f;
	private float slowingDistance = 40f;
	private float waypointDistance = 3f;

	void Seek(Vector3 targetPos)
	{
		Vector3 toTarget = targetPos - transform.position;
		Vector3 desired = toTarget.normalized * maxSpeed;

		force = desired - velocity;
		acceleration = force / mass;
		velocity += acceleration * Time.deltaTime;
		transform.position += velocity * Time.deltaTime;
		speed = velocity.magnitude;

		if(speed > 0)
		{
			Vector3 tempUp = Vector3.Lerp(transform.up, Vector3.up + (acceleration * banking), Time.deltaTime * 3.0f);
			transform.LookAt(transform.position + velocity, tempUp);
			velocity -= (damping * velocity * Time.deltaTime);
		}
	}

	void Arrive(Vector3 targetPos)
	{
		Vector3 toTarget = targetPos - transform.position;
		float distance = toTarget.magnitude;

		if(distance == 0.0f)
		{
			force = Vector3.zero;
		}
		else
		{
			float ramped = (distance / slowingDistance) * maxSpeed;
			float clamped = Mathf.Min(ramped, maxSpeed);
			Vector3 desired = clamped * (toTarget / distance);
			force = desired - velocity;
		}

		acceleration = force / mass;
		velocity += acceleration * Time.deltaTime;
		transform.position += velocity * Time.deltaTime;
		speed = velocity.magnitude;

		if(speed > 0)
		{
			Vector3 tempUp = Vector3.Lerp(transform.up, Vector3.up + (acceleration * banking), Time.deltaTime * 3.0f);
			transform.LookAt(transform.position + velocity, tempUp);
			velocity -= (damping * velocity * Time.deltaTime);
		}
	}

	void FollowPath()
	{
		Vector3 nextWaypoint = path.Next();

		if(!path.looped && path.IsLast())
		{
			Arrive(nextWaypoint);
		}
		else
		{
			if(Vector3.Distance(transform.position, nextWaypoint) < waypointDistance)
			{
				path.ToNext();
			}

			Seek(nextWaypoint);
		}
	}

	void FixedUpdate()
	{
		if(seeking)
		{
			Seek(target.position);
		}
		else if(arriving)
		{
			Arrive(target.position);
		}
		else if(followingPath)
		{
			FollowPath();
		}
	}
}