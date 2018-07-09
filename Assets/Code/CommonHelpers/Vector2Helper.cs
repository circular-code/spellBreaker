using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Vector2Helper {

    public List<float> x = new List<float>();
    public List<float> y = new List<float>();
    //public float x;
    //public float y;

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }

    /*public Vector2Helper(float _x, float _y)
    {
        this.x = _x;
        this.y = _y;
    }*/
}
