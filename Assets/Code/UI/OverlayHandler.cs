using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayHandler : MonoBehaviour {

    [SerializeField]
    private GameObject overlayObj;
    [SerializeField]
    private Text overlayText;
    [SerializeField]
    private GameObject errorButton;

    [SerializeField]
    private Text debugTextField;
    [SerializeField]
    public static Text m_debugTextField;

    [SerializeField]
    private static GameObject m_ThisObj;
    [SerializeField]
    private static Text m_OverlayText;
    [SerializeField]
    private static GameObject m_ErrorButton;

    public static void ActivateOverlay(string filter, JSONObject obj, bool showBtn = false)
    {
        JSONObject msgObj = LocalizationManager.SearchByFilter(filter);
        
        string msgFilter = LocalizationHelper.StripQuotationsFromJson(obj);
        string msg = LocalizationHelper.StripQuotationsFromJson(msgObj[msgFilter]);
        
        //Debug.Log(msg);
        
        SetOverlay(msg, showBtn);
        m_ThisObj.SetActive(true);
    }

    public static void SetOverlay(string msg, bool showBtn)
    {
        msg.Replace("\"", "");
        m_OverlayText.text = msg;
        if (showBtn)
        {
            m_ErrorButton.SetActive(true);
        }
        if (!m_ThisObj.activeSelf)
        {
            m_ThisObj.SetActive(true);
        }
    }

    public static void CloseOverlay()
    {
        Debug.Log("Close overlay");
        m_ThisObj.SetActive(false);
        m_OverlayText.text = "";
        m_ErrorButton.SetActive(false);
    }    

	// Use this for initialization
	void Start () {
        m_ThisObj = overlayObj;
        m_OverlayText = overlayText;
        m_ErrorButton = errorButton;
        m_debugTextField = debugTextField;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
