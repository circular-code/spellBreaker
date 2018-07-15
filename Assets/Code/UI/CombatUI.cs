using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour {

    [SerializeField]
    private Text localPlayerHealth;
    [SerializeField]
    private Text opponentPlayerHealth;
    [SerializeField]
    private GameObject incomingSpellPrefab;
    [SerializeField]
    private GameObject incomingSpellHolder;

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

    public GameObject IncomingSpellHolder
    {
        get
        {
            return incomingSpellHolder;
        }

        set
        {
            incomingSpellHolder = value;
        }
    }

    public GameObject IncomingSpellPrefab
    {
        get
        {
            return incomingSpellPrefab;
        }

        set
        {
            incomingSpellPrefab = value;
        }
    }

    public static void Init(GameObject _prefab, UIHandler _handler)
    {
        GameObject go = Instantiate(_prefab, Camera.main.transform.GetChild(0));
        go.name = "CombatUI";
        _handler.CombatUIElement = go;
        UIHandler.CombatUI = go.GetComponent<CombatUI>();
    }    

    public static void AddIncomingSpell()
    {
        Instantiate(UIHandler.CombatUI.IncomingSpellPrefab, UIHandler.CombatUI.IncomingSpellHolder.transform);
    }

    public static void DeleteIncomingSpell()
    {
        if(UIHandler.CombatUI.IncomingSpellHolder.transform.childCount > 0)
        {
            Destroy(UIHandler.CombatUI.IncomingSpellHolder.transform.GetChild(0).gameObject);
        }
    }
}
