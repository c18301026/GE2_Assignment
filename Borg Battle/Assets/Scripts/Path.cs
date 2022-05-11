using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
	public bool looped;

	private List<Vector3> waypoints = new List<Vector3>();
	private int current = 0;

	private void PopulatePath()
	{
		waypoints.Clear();
		foreach(Transform child in transform.GetComponentsInChildren<Transform>())
		{
			if(child != transform)
			{
				waypoints.Add(child.position);
			}
		}
	}

	public void Awake()
	{
		PopulatePath();
	}

	public void OnDrawGizmos()
	{
		PopulatePath();
		Gizmos.color = Color.blue;

		for(int i = 1; i < waypoints.Count; i++)
		{
			Gizmos.DrawLine(waypoints[i - 1], waypoints[i]);
			Gizmos.DrawSphere(waypoints[i - 1], 1);
			Gizmos.DrawSphere(waypoints[i], 1);
		}

		if(looped)
		{
			Gizmos.DrawLine(waypoints[waypoints.Count - 1], waypoints[0]);
		}
	}

	public Vector3 Next()
	{
		return waypoints[current];
	}

	public bool IsLast()
	{
		return (current == waypoints.Count - 1);
	}

	public void ToNext()
	{
		if(!looped)
		{
			if(!IsLast())
			{
				current++;
			}
		}
		else
		{
			current = (current + 1) % waypoints.Count;
		}
	}
}