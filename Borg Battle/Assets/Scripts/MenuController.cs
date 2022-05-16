using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			SceneManager.LoadScene("Scene1");
		}

		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
			Debug.Log("I want out!");
		}
	}
}
