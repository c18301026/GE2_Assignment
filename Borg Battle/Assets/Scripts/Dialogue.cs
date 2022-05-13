using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue : MonoBehaviour
{
	public TextMeshProUGUI textComponent;
	public string[] lines;
	public Texture2D[] portraits;
	public float[] displayTimeStamps, hideTimeStamps;
	public GameObject portraitBox;
	public GameObject canvas;

	void Start()
	{
		textComponent.text = "";

		for(int i = 0; i < lines.Length; i++)
		{
			StartCoroutine(ChangeDialogue(lines[i], portraits[i], displayTimeStamps[i]));
		}

		for(int i = 0; i < hideTimeStamps.Length; i++)
		{
			StartCoroutine(HideDialogue(hideTimeStamps[i]));
		}
	}

	IEnumerator ChangeDialogue(string line, Texture2D portrait, float displayTimeStamp)
	{
		yield return new WaitForSeconds(displayTimeStamp);
		textComponent.text = line;
		portraitBox.GetComponent<RawImage>().texture = portrait;
		canvas.GetComponent<Canvas>().enabled = true;
	}

	IEnumerator HideDialogue(float hideTimeStamp)
	{
		yield return new WaitForSeconds(hideTimeStamp);
		canvas.GetComponent<Canvas>().enabled = false;
	}
}