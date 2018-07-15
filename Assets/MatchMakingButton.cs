using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchMakingButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Button btn = this.GetComponent<Button>();

        btn.onClick.AddListener(disableLoginHandler);
        btn.onClick.AddListener(enableMatchMaking);
	}
	
	void disableLoginHandler()
    {
        GameObject.Find("Logic").GetComponent<LoginHandler>().enabled = false;
    }

    void enableMatchMaking()
    {
        MatchHandler mh = GameObject.Find("Logic").GetComponent<MatchHandler>();
        mh.enabled = true;
        mh.RegisterForMatchMaking();
    }
}
