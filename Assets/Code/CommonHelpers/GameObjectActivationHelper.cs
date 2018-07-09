using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectActivationHelper {

    public static GameObject FindChildObject(GameObject parent, string name)
    {
        Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == name)
            {
                t.gameObject.SetActive(true);
                return t.gameObject;
            }
        }
        return null;
    }
}
