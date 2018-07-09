using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResourceHelper {

    public static JSONObject LoadJsonFromResource(string path, string filename)
    {
        if (!path[path.Length - 1].Equals('/'))
        {
            path += "/";
        }
        string filepath = path + filename;

        TextAsset file = Resources.Load<TextAsset>(filepath);

        string data = file.text;

        JSONObject obj = new JSONObject(data);
        return obj;
    }
}
