using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetUIElementText : MonoBehaviour {


    [SerializeField]
    private Text textfield;
    [SerializeField]
    private string UIClass;
    [SerializeField]
    private string elementName;

	// Use this for initialization
	void Start () {
        LocalizationManager.OnTextRefresh += RefreshText;
        RefreshText();
    }

    public void RefreshText()
    {
        UITextRefresher.GetTextRefresh(UIClass, textfield, elementName);
    }

    public void SetElementName(string name)
    {
        elementName = name;
    }


}

public static class UITextRefresher
{
    public static void GetTextRefresh(string UIClass, Text textfield, string elementName)
    {
        if (LocalizationManager.SearchByFilter(UIClass) != null)
        {
            JSONObject elObj = LocalizationManager.SearchByFilter(UIClass);
            textfield.text = LocalizationHelper.StripQuotationsFromJson(elObj[elementName]);
        }
    }
}
