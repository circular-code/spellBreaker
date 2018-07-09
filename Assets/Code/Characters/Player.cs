using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour{

    [SerializeField]
    protected string playerName;
    [SerializeField]
    protected int health;
    [SerializeField]
    protected string socketID;
    [SerializeField]
    protected bool isLocalPlayer;

    public Player(string _socketID, string _playerName = "Playername", int _health = 100, bool _isLocalPlayer = false)
    {
        this.playerName = _playerName;
        this.health = _health;
        this.socketID = _socketID;
        this.isLocalPlayer = _isLocalPlayer;
    }

    public string PlayerName
    {
            get
            {
                return playerName;
            }

            set
            {
                playerName = value;
            }
        }

    public int Health
    {
        get
        {
            return health;
        }

        set
        {
            health = value;
        }
    }

    public string SocketID
    {
        get
        {
            return socketID;
        }

        set
        {
            socketID = value;
        }
    }

    public bool IsLocalPlayer()
    {
        return this.isLocalPlayer;
    }

}
