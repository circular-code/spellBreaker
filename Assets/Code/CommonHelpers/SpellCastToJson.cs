using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpellCastToJson {

    public string match_id;
    public GestureRecognizer.GestureData gestureData;

    //public List<float> x = new List<float>();
    //public List<float> y = new List<float>();

    public string SaveToString()
    {
        Debug.Log("Line count: " + gestureData.lines.Count);
        return JsonUtility.ToJson(this);
    }
}
