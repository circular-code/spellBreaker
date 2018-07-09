using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LocalizationHelper {

    public static JSONObject languageFile { get; set; }

    public static string GetFilter(string filter)
    {
        return languageFile[filter].ToString();
    }

    public static string StripQuotationsFromJson(JSONObject data)
    {
        //Debug.Log("Stip Quotations from " + data);
        return data.ToString().Replace("\"", "");
    }

    public static string StripQuotationsFromString(string text)
    {
        return text.Replace("\"", "");
    }
}
