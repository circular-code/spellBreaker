using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MatchStatsRefresher {

    public delegate void RefreshStatsUI();
    public static event RefreshStatsUI OnStatsUIRefresh;

    /*public static void GetUIStatsRefresh(string UIClass, Text textfield, string elementName)
    {
        if (LocalizationManager.SearchByFilter(UIClass) != null)
        {
            JSONObject elObj = LocalizationManager.SearchByFilter(UIClass);
            textfield.text = LocalizationHelper.StripQuotationsFromJson(elObj[elementName]);
        }
    }*/
}
