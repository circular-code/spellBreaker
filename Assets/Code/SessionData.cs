using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SessionData : MonoBehaviour {

    [SerializeField]
    private static SessionData s_Instance = null;
    [SerializeField]
    private static string username { get; set; }
    [SerializeField]
    private static int user_id { get; set; }
    [SerializeField]
    private static string socket_id { get; set; }
    public string debug_SocketId;
    [SerializeField]
    private static Match match;
    public Match debugMatch;

    public static Match Match
    {
        get
        {
            return match;
        }
        set
        {
            match = value;
        }
    }

    public static string UserName
    {
        get
        {
            return username;
        }
        set
        {
            username = value;
        }
    }

    public static int UserID
    {
        get
        {
            return user_id;
        }
        set
        {
            user_id = value;
        }
    }

    public static string SocketID
    {
        get
        {
            return socket_id;
        }
        set
        {
            socket_id = value;
        }
    }

    void OnApplicationQuit()
    {
        match = null;
    }
}
