# Star Trek Borg Battle

Name: Rianlee Gabriel Pineda

Student Number: C18301026

Class Group: TU856 (DT228)

# Description
This is a low-poly recreation of the Borg battle from Star Trek: First Contact in Unity. All models were created using Blender.

The story will mimic the following video:

[![YouTube](http://img.youtube.com/vi/D7KCb-O20Fg/0.jpg)](https://www.youtube.com/watch?v=D7KCb-O20Fg)

# Video Demonstration

# Instructions for use

# Event Summary
## Scene 1
1. Picard gets notified that Starfleet has engaged the Borg.
2. The Borg threatens to assimilate Earth.
3. The Enterprise goes to Earth at maximum warp to aid the fleet who are losing the battle.

## Scene 2
1. Borg cube is seen heading for Earth.
2. 3 shots fired by the Starfleet hit the Borg cube.

## Scene 3
1. The Starfleet ships attack the Borg cube.
2. The USS Defiant shoots at the cube, but then gets too close and gets shot down by the Borg cube.
3. The USS Defiant is heavily damaged, on fire and slows down.
4. The rest of the ships continue to struggle against the Borg cube.

7. The Enterprise arrives to aid the other ships in their fight against the Borg cube.
8. Picard takes command of the fleet and orders the ships to target their weapons to a specific part of the Borg cube.
9. Before the Borg cube blows up, a Borg sphere is released.
10. The Borg sphere heads for Earth.
11. The Enterprise pursues the Borg sphere.
12. Worf boards the Enterprise.
13. Temporal vortex is created by the Borg sphere to travel back in time.
14. The Enterprise gets caught in the temporal vortex.
15. The Borg assimilates Earth from the past and changes history.
16. To reverse the damage caused by the Borg, the Enterprise follows the Borg sphere through the vortex.
17. The Borg sphere starts attacking Earth.
18. In response, the Enterprise fires quantum torpedoes at the sphere.
19. The Borg sphere blows up.

# How it works
## Scene 1
### Movement of the Enterprise
In this scene, you can see the Enterprise move through space. It is a GameObject that has the [ShipBehaviour.cs](https://github.com/c18301026/GE2_Assignment/blob/main/Borg%20Battle/Assets/Scripts/ShipBehaviour.cs) script attached. This script contains attributes relating to a ship's states, physics and methods for how the ship will move towards a target.
#### ShipBehaviour.cs
```C#
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
```
For most of the scene, the Enterprise isn't actually following (seek/arrive/path following) anything in particular. The movement is controlled by a "scene director", an empty GameObject that has a [unique script](https://github.com/c18301026/GE2_Assignment/blob/main/Borg%20Battle/Assets/Scripts/Scene1Director.cs) attached to it for every scene that controls how the events play out in specific scene. It is only until the end of the scene (when Picard says "Engage!") where the Enterprise actually seeks a target (it is a sphere, but its Mesh Renderer component has been disabled). The purpose of the target sphere is to make the Enterprise turn around when Picard decides to help the Starfleet's battle against the Borg cube.
#### These are some attributes found at the top of the scene 1 director script. They are related to the physics and states of the Enterprise ship, e.g., boolean flags that state when the ship should turn around.
```C#
// Attributes related to the ships/physics
private GameObject enterprise;
private GameObject target;
private Camera camera;
private ShipBehaviour enterpriseBehaviour;
private float targetSpeed = 5f;
private bool lookAt = false;
private bool turningAround = false;
private bool goingBack = false;
private bool faster = false;

// ...other attributes not related to the movement of the Enterprise
```
#### The Awake() method is called to initialise some variables and get a reference to the Enterprise ship and its target.
```C#
void Awake()
{
	// Ship/physics variables
	enterprise = GameObject.FindWithTag("Enterprise");
	target = GameObject.FindWithTag("Target");
	camera = Camera.main;
	enterpriseBehaviour = enterprise.GetComponent<ShipBehaviour>();
	enterpriseBehaviour.target = target.transform;
	enterpriseBehaviour.maxSpeed = targetSpeed;

	// ...the rest of the Awake() method isn't related to the movement of the Enterprise
}
```
#### In the Start() method, a method called TurnAround() is invoked after 111.5 seconds has passed in the first scene. This is when the target sphere turns around. Once the target sphere turns around, the Enterprise is already seeking it and therefore, the Enterprise also turns around.
```C#
void Start()
{
	// ...the code above the invoke method relates to coroutines for changing the camera angles, i.e., not related to the movement of the Enterprise ship

	Invoke("TurnAround", 111.5f);
}
```
#### The else statement shows that in normal cases, i.e., most of the scene, the Enterprise ship is technically not seeking the target. Instead, its position is changed at an equal pace to the target sphere. Once the turningAround flag/state has been turned on in the first if statement, the Enterprise ship begins to actually seek the target sphere. The target sphere moves in the X-axis as a way to make the Enterprise change its direction.  The else if statement shows the target (meaning the Enterprise also) going in the opposite direction it was originally going in.
```C#
void FixedUpdate()
{
	if(turningAround)
	{
		enterpriseBehaviour.seeking = true;
		target.transform.position += new Vector3(targetSpeed * Time.deltaTime, 0, 0);
	}
	else if(goingBack || faster)
	{
		target.transform.position += new Vector3(0, 0, -targetSpeed * Time.deltaTime);
	}
	else
	{
		enterprise.transform.position += new Vector3(0, 0, targetSpeed * Time.deltaTime);
		target.transform.position += new Vector3(0, 0, targetSpeed * Time.deltaTime);
	}

	// ...below is code not related to the movement of the Enterprise
}
```
#### These methods give more context to the FixedUpdate() code above.
```C#
void TurnAround()
{
	turningAround = true;
	Invoke("GoBack", 0.5f);
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
	targetSpeed = 300f;
	enterpriseBehaviour.maxSpeed = targetSpeed;
}
```

# List of classes/assets in this project

# What I am most proud of in the assignment

# Models
## Enterprise
![enterprise model in blender](BlenderImages/Enterprise.png)
## Borg Cube
![borg cube model in blender](BlenderImages/BorgCube.png)
## Earth
![earth model in blender](BlenderImages/Earth.png)
## Generic Ship No. 1
![generic ship 1 model in blender](BlenderImages/Ship1.png)
## Generic Ship No. 2
![generic ship 2 model in blender](BlenderImages/Ship2.png)
## USS Defiant
![uss defiant model in blender](BlenderImages/USSDefiant.png)
## Borg Sphere
![borg sphere model in blender](BlenderImages/BorgSphere.png)
## Borg Earth
![borg earth model in blender](BlenderImages/BorgEarth.png)