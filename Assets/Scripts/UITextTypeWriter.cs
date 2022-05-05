using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

// attach to UI Text component (with the full text already there)

public class UITextTypeWriter : MonoBehaviour 
{
	
	TMPro.TextMeshProUGUI txt;
	GameObject bobbingText;
	string story;
    public int textSpeed = 32;
	
	void Awake () 
	{
		txt = GetComponent<TMPro.TextMeshProUGUI> ();
		try {
			bobbingText = GameObject.Find("BobbingText");
			bobbingText.SetActive(false);
		} catch (Exception e) {
			Debug.Log("bobbingText not found");
		}
        
        // Debug.Log(txt);
		story = txt.text;
		txt.text = "";
		
		// TODO: add optional delay when to start
		StartCoroutine ("PlayText");
	}

	IEnumerator PlayText()
	{
		foreach (char c in story) 
		{
			txt.text += c;
			yield return new WaitForSeconds (1f/textSpeed);
		}

        bobbingText.SetActive(true);
	}

}