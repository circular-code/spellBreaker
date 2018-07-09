using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextFadeOut : MonoBehaviour {

    [SerializeField]
    private bool timedFadeOut;
    [SerializeField]
    private bool triggerActivation;
    [SerializeField]
    private float fadeOutSpeed;
    [SerializeField]
    private Text text;
    [SerializeField]
    private string oldText;

    void OnEnable()
    {
        if (triggerActivation)
        {

        }
    }
	
	// Update is called once per frame
	void Update () {
		if(!triggerActivation)
        {
            if(timedFadeOut)
            {
                if(text.text != oldText)
                {
                    Color col = new Color(text.color.r, text.color.g, text.color.b, 1);
                    text.color = col;
                }
                if (text.text != "")
                {
                    oldText = text.text;
                    if (text.color.a > 0)
                    {
                        Color col = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime * fadeOutSpeed));
                        text.color = col;
                    }
                    else
                    {
                        text.text = "";
                    }
                } else
                {
                    Color col = new Color(text.color.r, text.color.g, text.color.b, 1);
                    text.color = col;
                }
            }
        }
	}
}
