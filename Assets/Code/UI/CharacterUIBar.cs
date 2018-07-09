using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUIBar : MonoBehaviour {

    [SerializeField]
    private Image bar;
    [SerializeField]
    private Text value;
    [SerializeField]
    private Text label;

    public Image getBar()
    {
        return this.bar;
    }

    public Text getValue()
    {
        return this.value;
    }

    public Text getLabel()
    {
        return this.label;
    }



	// Use this for initialization
	void Start () {
        if(this.tag == "HealthBar")
        {
            //UIUpdateHandler.OnHealthbarRefresh += this.UpdateBar;
        } else if(this.tag == "EnergyBar")
        {
            //UIUpdateHandler.OnEnergybarRefresh += this.UpdateBar;
        }		
	}

    public void UpdateBar(float newVal)
    {
        this.getValue().text = newVal.ToString();
        this.getBar().fillAmount = newVal / 100;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
