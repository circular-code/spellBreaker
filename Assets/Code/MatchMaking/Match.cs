using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Match {

    [SerializeField]
    private string matchID;
    [SerializeField]
    private string playerDraw;
    [SerializeField]
    private static string localPlayerID;
    [SerializeField]
    private List<Player> players = new List<Player>();

    public Match(string _matchID, string _playerDraw, string _localPlayerID)
    {
        Debug.Log("_playerDraw " + _playerDraw);
        Debug.Log("_localPlayerID " + _localPlayerID);
        this.matchID = _matchID;
        this.playerDraw = _playerDraw;
        localPlayerID = _localPlayerID;
        Debug.Log("playerDraw " + this.playerDraw);
        Debug.Log("localPlayerID " + localPlayerID);
    }

    #region GET/SET

    public string MatchID
    {
        get
        {
            return this.matchID;
        }        
    }

    public string PlayerDraw
    {
        get
        {
            return this.playerDraw;
        }
        set
        {
            this.playerDraw = value;
        }
    }

    public string LocalPlayerID
    {
        get
        {
            return localPlayerID;
        }        
    }

    public List<Player> Players
    {
        get
        {
            return players;
        }

        set
        {
            players = value;
        }
    }

    #endregion

    public Player GetPlayerBySocketID(string _socketId)
    {
        return players.Find(x => x.SocketID == _socketId);
    }
}
