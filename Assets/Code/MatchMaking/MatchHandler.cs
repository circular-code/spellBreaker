using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SocketIO;

public class MatchHandler : MonoBehaviour {

    [SerializeField]
    private SocketIOComponent socket;
    private AsyncOperation loadMatch;
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private UIHandler uiHandler;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RegisterForMatchMaking()
    {
        
        socket.Emit("register:for:match");
        socket.On("match:joined", (SocketIOEvent e) =>
        {
            Debug.Log("[SocketIO] LFM Request successfull.");
            StartCoroutine(InitializeMatch(e.data));

        });        
    }

    IEnumerator InitializeMatch(JSONObject data)
    {
        loadMatch = SceneManager.LoadSceneAsync(1);
        while (!loadMatch.isDone)
        {
            Debug.Log("loading scene");
            yield return loadMatch;
        }
        Match match = new Match(LocalizationHelper.StripQuotationsFromJson(data[0]), LocalizationHelper.StripQuotationsFromJson(data[1]), LocalizationHelper.StripQuotationsFromJson(data[2]["socket_id"]));
        Debug.Log(match);
        SessionData.Match = match;
        this.GetComponent<SessionData>().debugMatch = SessionData.Match;
        OverlayHandler.m_debugTextField.text = "Match ID: " + match.MatchID + "\n" + "PlayerDraw: " + match.PlayerDraw + "\n" + "Local ID: " + match.LocalPlayerID;

        //Debug.Log(data["opponentPlayer"]);        

        // Init UI Elements
        uiHandler.enabled = true;
        UIHandler.InitCombatUI(uiHandler.CombatUIElement, uiHandler);

        // Init Player objects
        InitPlayers(LocalizationHelper.StripQuotationsFromJson(data["opponentPlayer"]["socket_id"]));

        socket.On("spell:result", (SocketIOEvent e) =>
        {
            SpellResult(e.data);
        });
    }

    void InitPlayers(string _remotePlayerId)
    {
        GameObject localPlayerObj = Instantiate(playerPrefab);
        localPlayerObj.name = "LocalPlayer";
        Player localPlayer = localPlayerObj.AddComponent<Player>();
        localPlayer.SocketID = SessionData.SocketID;
        localPlayer.Health = 100;
        UIHandler.UpdateLocalPlayerHealth(localPlayer.Health);
        //GameObject.Find("LocalHP").GetComponent<Text>().text = "Your HP: " + localPlayer.Health;
        SessionData.Match.Players.Add(localPlayer);

        GameObject remotePlayerObj = Instantiate(playerPrefab);
        remotePlayerObj.name = "RemotePlayer";
        Player remotePlayer = remotePlayerObj.AddComponent<Player>();
        remotePlayer.SocketID = _remotePlayerId;
        remotePlayer.Health = 100;
        UIHandler.UpdateOpponentPlayerHealth(remotePlayer.Health);
        //GameObject.Find("EnemyHP").GetComponent<Text>().text = "Enemy HP: " + remotePlayer.Health;
        SessionData.Match.Players.Add(remotePlayer);
    }

    void SpellResult(JSONObject data)
    {
        if(data[0] == true)
        {
            Debug.Log(SessionData.Match.GetPlayerBySocketID(LocalizationHelper.StripQuotationsFromJson(data["result"]["socket_id"])));
            Player pl = SessionData.Match.GetPlayerBySocketID(LocalizationHelper.StripQuotationsFromJson(data["result"]["socket_id"]));


            /*
             * NUR DEBUG LÖSUNG
             * !!!DURCH GLOBALEN UI HANDLER UMSCHREIBEN!!!! 
             * 
             */
            pl.Health = Convert.ToInt16(LocalizationHelper.StripQuotationsFromJson(data["result"]["health"]));
            if(pl.SocketID == SessionData.Match.LocalPlayerID)
            {
                GameObject.Find("LocalHP").GetComponent<Text>().text = "Your HP: " + pl.Health;
            } else
            {
                GameObject.Find("EnemyHP").GetComponent<Text>().text = "Enemy HP: " + pl.Health;
            }
        } else
        {
            Debug.Log("Spell failed");
        }
    }
}
