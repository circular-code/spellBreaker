using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour {

    [SerializeField]
    private Text localPlayerHealth;
    [SerializeField]
    private Text opponentPlayerHealth;

    public Text LocalPlayerHealth
    {
        get
        {
            return localPlayerHealth;
        }

        set
        {
            localPlayerHealth = value;
        }
    }

    public Text OpponentPlayerHealth
    {
        get
        {
            return opponentPlayerHealth;
        }

        set
        {
            opponentPlayerHealth = value;
        }
    }

    public static void Init(GameObject _prefab, UIHandler _handler)
    {
        GameObject go = Instantiate(_prefab, Camera.main.transform.GetChild(0));
        go.name = "CombatUI";
        _handler.CombatUIElement = go;
        UIHandler.CombatUI = go.GetComponent<CombatUI>();
    }    
}
