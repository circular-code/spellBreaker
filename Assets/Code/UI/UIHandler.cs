using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {

    [SerializeField]
    private GameObject combatUIElementPrefab;
    [SerializeField]
    private GameObject combatUIElement;
    [SerializeField]
    private static CombatUI combatUI;

    public static CombatUI CombatUI
    {
        get
        {
            return combatUI;
        }

        set
        {
            combatUI = value;
        }
    }

    public GameObject CombatUIElement
    {
        get
        {
            return combatUIElement;
        }

        set
        {
            combatUIElement = value;
        }
    }

    public GameObject CombatUIElementPrefab
    {
        get
        {
            return combatUIElementPrefab;
        }
    }

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void InitCombatUI(GameObject _combatUIElement, UIHandler _handler)
    {
        CombatUI.Init(_combatUIElement, _handler);
    }

    public static void UpdateLocalPlayerHealth(float _newHealth)
    {
        CombatUI.LocalPlayerHealth.text = "Your Health: " + _newHealth.ToString();
    }

    public static void UpdateOpponentPlayerHealth(float _newHealth)
    {
        CombatUI.OpponentPlayerHealth.text = "Enemy Health: " + _newHealth.ToString();
    }
}
