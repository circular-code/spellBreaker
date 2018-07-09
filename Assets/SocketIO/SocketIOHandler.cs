using System;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using SocketIO;

namespace SocketIO.Handler
{
    public abstract class SocketIOHandler
    {
        #region Helpers
        [SerializeField]
        private SocketIOComponent socket;

        public virtual string EncryptMD5(string input)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(input));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }
            return strBuilder.ToString();
        }


        #endregion

        #region Virtual Socket Functions

        public virtual void UserData(SocketIOEvent e)
        {
            Debug.Log(e.data);
        }

        public virtual void TestBoop(SocketIOEvent e)
        {
            Debug.Log("[SocketIO] Boop received: " + e.name + " " + e.data);

            if (e.data == null) { return; }

            Debug.Log(
                "#####################################################" +
                "THIS: " + e.data.GetField("this").str +
                "#####################################################"
            );
        }

        public virtual void OnError(SocketIOEvent e)
        {
            Debug.LogError("[SocketIO] Error: " + e.data);
            //JSONObject msgObj = LocalizationManager.SearchByFilter("errors");
            //string msgFilter = LocalizationHelper.StripQuotationsFromJson(e.data[0]);
            //string errMsg = LocalizationHelper.StripQuotationsFromJson(msgObj[msgFilter]);
            OverlayHandler.ActivateOverlay("errors", e.data[0], true);
        }

        public virtual void OnFailure(SocketIOEvent e)
        {
            Debug.Log("[SocketIO] Failure: " + e.data);
        }

        public virtual void OnSuccess(SocketIOEvent e)
        {
            Debug.Log("[SocketIO] Success: " + e.data);
        }

        public virtual void OnClose(SocketIOEvent e)
        {
            Debug.Log("[SocketIO] Close: " + e.data);
            if (socket)
            {
                if (socket.IsConnected)
                {
                    socket.Close();
                }
            }
        }

        public virtual void OnConnect(SocketIOEvent e)
        {


            //throw new NotImplementedException();
        }

        public virtual void OnConnecting(SocketIOEvent e)
        {
            Debug.Log(e);
        }

        public virtual void OnConnectionFailed(SocketIOEvent e)
        {
            Debug.Log(e.data);
            Debug.Log(e.data);
            /*if (e.Message.Contains("A WebSocket connection isn't established"))
            {
                connectionTries++;
                Debug.LogError(e.Message + " [Trieout #" + connectionTries + "]");

                if (connectionTries >= 3)
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(ConnectionOverlay("Verbindung zum Server konnte nicht hergestellt werden.", true));
                    connectionTries = 0;
                    socket.CloseSocketThread();
                }
            }*/
        }

        #endregion
    }
}


