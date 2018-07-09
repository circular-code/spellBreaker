using System;
using System.Collections;
using System.Collections.Generic;
using SocketIO;
using SocketIO.Handler;
using UnityEngine;

namespace LoginHelper
{
    public class SessionToken
    {
        public string session_token;

        public SessionToken(string token)
        {
            this.session_token = token;
        }

        public string SaveToString()
        {
            return JsonUtility.ToJson(this);
        }
    }

    public class UserData
    {
        public string forname;
        public string surname;
        public string username;
        public string password;
        public string mail;

        public UserData(string username, string password, string forname = null, string surname = null, string mail = null)
        {
            this.forname = forname;
            this.surname = surname;
            this.username = username;
            this.password = password;
            this.mail = mail;
        }

        public string SaveToString()
        {
            return JsonUtility.ToJson(this);
        }
    }

    public class ReturnCode
    {
        public int returnCode;
        public string data;

        public static ReturnCode CreateFromJson(string jsonString)
        {
            return JsonUtility.FromJson<ReturnCode>(jsonString);
        }
    }

    public class LoginSocketHandler : SocketIOHandler
    {
        private LoginHandler handler;
         
        public LoginSocketHandler(LoginHandler _handler)
        {
            this.handler = _handler;
        }

        public override void OnSuccess(SocketIOEvent e)
        {
            OverlayHandler.CloseOverlay();
            //Debug.Log(e.data[0].ToString());
            //int user_id = JsonUtility.FromJson<int>(e.data.str);
            int user_id = Convert.ToInt32(e.data[0].ToString());

            //Debug.Log("user id: " + user_id);
            SessionData.UserID = user_id;
            //Debug.Log(SessionData.getUserID());

            GameObject.Find("LoginMask").SetActive(false);
            GameObject[] objs = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach(GameObject go in objs)
            {
                if(go.name == "MainMenuMask")
                {
                    go.SetActive(true);
                }
            }

            handler.enabled = false;
            base.OnSuccess(e);
        }
        
        public void OnUserLogin(SocketIOEvent e)
        {
            Debug.Log("[Socket OnUserLogin]: " + e);
            string data = LocalizationHelper.StripQuotationsFromJson(e.data[0]);
            if(data != "bad pass")
            {
                OnSuccess(e);
            } else
            {
                OnError(e);
            }
        }

        public delegate void Disconnect();
        public static event Disconnect OnDisconnect;

        public override void OnError(SocketIOEvent e)
        {
            base.OnError(e);
        }
    }
}
