﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this.gameObject);	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DestroyThis()
    {
        Destroy(this.gameObject);
    }
}