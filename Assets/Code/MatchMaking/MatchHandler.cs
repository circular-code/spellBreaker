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
        socket.On("match:joined", (SocketIOEvent e) =>
        {
            Debug.Log("[SocketIO] LFM Request successfull.");
            StartCoroutine(InitializeMatch(e.data));
            socket.On("endMatch", (SocketIOEvent ev) =>
            {
                if (SceneManager.GetActiveScene().buildIndex != 1)
                {
                    socket.Off("spell:result", (SocketIOEvent evnt) =>
                    {
                        SpellResult(evnt.data);
                    });
                    socket.Off("spell:pending", (SocketIOEvent evnt) =>
                    {
                        SpellPending(evnt.data);
                    }); socket.Off("spell:blocked", (SocketIOEvent evnt) =>
                    {
                        SpellBlocked(evnt.data);
                    });
                    SceneManager.LoadScene(1);
                }
            });
        });
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RegisterForMatchMaking()
    {
        Debug.Log("registformatchmacking()");
        socket.Emit("register:for:match");            
    }    

    IEnumerator InitializeMatch(JSONObject data)
    {
        loadMatch = SceneManager.LoadSceneAsync(2);
        while (!loadMatch.isDone)
        {
            Debug.Log("loading scene");
            yield return loadMatch;
        }
        if(loadMatch.isDone)
        {
            StopCoroutine(InitializeMatch(data));
        }
        Match match = new Match(LocalizationHelper.StripQuotationsFromJson(data[0]), LocalizationHelper.StripQuotationsFromJson(data[1]), LocalizationHelper.StripQuotationsFromJson(data[2]["socket_id"]));
        Debug.Log(match);
        SessionData.Match = match;
        this.GetComponent<SessionData>().debugMatch = SessionData.Match;
        OverlayHandler.m_debugTextField.text = "Match ID: " + match.MatchID + "\n" + "PlayerDraw: " + match.PlayerDraw + "\n" + "Local ID: " + match.LocalPlayerID;

        //Debug.Log(data["opponentPlayer"]);        

        // Init UI Elements
        uiHandler.enabled = true;
        UIHandler.InitCombatUI(uiHandler.CombatUIElementPrefab, uiHandler);

        // Init Player objects
        InitPlayers(LocalizationHelper.StripQuotationsFromJson(data["opponentPlayer"]["socket_id"]));

        socket.On("spell:result", (SocketIOEvent e) =>
        {
            SpellResult(e.data);
        });
        socket.On("spell:blocked", (SocketIOEvent e) =>
        {
            SpellBlocked(e.data);
        });
        socket.On("spell:pending", (SocketIOEvent e) =>
        {
            SpellPending(e.data);
        });
        socket.Off("match:joined", (SocketIOEvent e) => { });
    }

    void InitPlayers(string _remotePlayerId)
    {
        GameObject localPlayerObj = Instantiate(playerPrefab);
        localPlayerObj.name = "LocalPlayer";
        Player localPlayer = localPlayerObj.AddComponent<Player>();
        localPlayer.SocketID = SocketIOComponent.sid;
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

    void SpellPending(JSONObject data)
    {
        CombatUI.AddIncomingSpell();
    }

    void SpellBlocked(JSONObject data)
    {
        CombatUI.DeleteIncomingSpell();
    }

    void SpellResult(JSONObject data)
    {
        if(data[0] == true)
        { 
            if(Convert.ToInt16(LocalizationHelper.StripQuotationsFromJson(data["defenderHealth"])) > 0)
            {
                Debug.Log(LocalizationHelper.StripQuotationsFromJson(data["defenderId"]));
                //Debug.Log(SessionData.Match.GetPlayerBySocketID(LocalizationHelper.StripQuotationsFromJson(data["result"]["socket_id"])));
                Player pl = SessionData.Match.GetPlayerBySocketID(LocalizationHelper.StripQuotationsFromJson(data["defenderId"]));


                /*
                 * NUR DEBUG LÖSUNG
                 * !!!DURCH GLOBALEN UI HANDLER UMSCHREIBEN!!!! 
                 * 
                 */
                pl.Health = Convert.ToInt16(LocalizationHelper.StripQuotationsFromJson(data["defenderHealth"]));
                if (pl.SocketID == SessionData.Match.LocalPlayerID)
                {
                    GameObject.Find("LocalHP").GetComponent<Text>().text = "Your HP: " + pl.Health;
                }
                else
                {
                    GameObject.Find("EnemyHP").GetComponent<Text>().text = "Enemy HP: " + pl.Health;
                }

                CombatUI.DeleteIncomingSpell();
            }
        } else
        {
            Debug.Log("Spell failed");
        }
    }
}
