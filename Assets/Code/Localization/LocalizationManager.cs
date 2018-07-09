using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : MonoBehaviour {

    public delegate void ChangeSelectedLanguage(string lang);
    public static event ChangeSelectedLanguage OnLangChange;

    [SerializeField]
    private string selectedLanguage;

    public delegate void RefreshText();
    public static event RefreshText OnTextRefresh;

    public static JSONObject languageFile { get; set; }    

    void Awake()
    {
        //OnLangChange += OnSelectedLangChange;
        //Init();
    }

	// Use this for initialization
	void Start () {
        OnLangChange += OnSelectedLangChange;  
        Init();
    }

    void Init()
    {
        //Debug.Log("Init");
        if (!PlayerPrefs.HasKey("lang"))
        {
            PlayerPrefs.SetString("lang", "en");
            selectedLanguage = PlayerPrefs.GetString("lang");
            languageFile = ResourceHelper.LoadJsonFromResource("language", "loc-" + selectedLanguage);
            //Debug.Log(languageFile);
        }
        else
        {
            selectedLanguage = PlayerPrefs.GetString("lang");
            languageFile = ResourceHelper.LoadJsonFromResource("language", "loc-" + selectedLanguage);
            //Debug.Log(languageFile);
        }
        //Debug.Log(languageFile);
        changeLanguage(selectedLanguage);
    }

    public static void changeLanguage(string lang)
    {
        //Debug.Log("language changed to: " + lang);
        PlayerPrefs.SetString("lang", lang);
        OnLangChange(lang);
        languageFile = ResourceHelper.LoadJsonFromResource("language", "loc-" + lang);
        Canvas.ForceUpdateCanvases();
        if (OnTextRefresh != null)
        {
            OnTextRefresh();
        }
    }

    public static JSONObject SearchByFilter(string categoryfilter)
    {
        if (languageFile != null)
        {
            return languageFile[categoryfilter];
        }
        else
        {
            return null;
        }
    }

    public static string SearchForItem(string categoryfilter, string item)
    {
        return languageFile[categoryfilter][item].ToString();
    }

    void OnSelectedLangChange(string lang)
    {
        selectedLanguage = lang;
    }
}
