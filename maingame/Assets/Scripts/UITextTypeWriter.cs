using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// attach to UI Text component (with the full text already there)
public class UITextTypeWriter : MonoBehaviour
{
    TMPro.TextMeshProUGUI txt;

    GameObject bobbingText;

    string story;

    public int textSpeed = 32;

    string mode = "none";

    void SetPlayerMoviment(bool active)
    {
        var entities =
            FindObjectsOfType<MonoBehaviour>().OfType<EntityController>();

        foreach (EntityController e in entities)
        {
            e.allowMoviment = active;
        }
    }

    void Awake()
    {
        try
        {
            bobbingText = GameObject.Find("BobbingText");
            bobbingText.SetActive(false);
        }
        catch (Exception e)
        {
            Debug.Log("bobbingText not found");
        }
    }

    void OnEnable()
    {
        bobbingText.SetActive(false);

        mode = "none";
        SetPlayerMoviment(false);

        txt = GetComponent<TMPro.TextMeshProUGUI>();

        // Debug.Log(txt);
        story = txt.text;
        txt.text = "";

        // TODO: add optional delay when to start
        StartCoroutine("PlayText");
    }

    IEnumerator PlayText()
    {
        mode = "write";

        foreach (char c in story)
        {
            txt.text += c;
            if (mode == "write")
            {
                yield return new WaitForSeconds(1f / textSpeed);
            }
        }

        bobbingText.SetActive(true);
        mode = "close";
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (mode == "close")
            {
                GameObject.Find("GardeniaTalkScene").SetActive(false);
                SetPlayerMoviment(true);
            }
            else if (mode == "write")
            {
                mode = "close";
            }
        }
    }
}
