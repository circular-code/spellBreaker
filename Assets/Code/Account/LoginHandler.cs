using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using SocketIO;
using LoginHelper;

public class LoginHandler : MonoBehaviour {

    [SerializeField]
    private SocketIOComponent socket;
    public InputField regForNameText, regSurNameText, regUsernameText, regPassText, regConfPassText, regEmailText;
    public InputField usernameField, passwordField;
    public Text messageText;

    [SerializeField]
    private int timeout = 60;

    private string username, password, regForName, regSurName, regUsername, regPass, regConfPass, regEmail;

    [SerializeField]
    private LoginSocketHandler socketHandler;

    private void Awake()
    {
        socketHandler = new LoginSocketHandler(this);
        LoginSocketHandler.OnDisconnect += OnDisconnect;
    }

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update () {
		
	}


    public void Register()
    {
        regForName = regForNameText.text;
        regSurName = regSurNameText.text;

        regUsername = regUsernameText.text;
        regPass = regPassText.text;
        regConfPass = regConfPassText.text;
        regEmail = regEmailText.text;

        if (regForName == "" || regSurName == "" || regUsername == "" || regPass == "" || regConfPass == "" || regEmail == "")
        {
            JSONObject elObj = LocalizationManager.SearchByFilter("register:ui");
            messageText.text = LocalizationHelper.StripQuotationsFromJson(elObj["completation"]);
            return;
        }

        if (!regPass.Equals(regConfPass))
        {
            JSONObject elObj = LocalizationManager.SearchByFilter("register:ui");
            messageText.text = LocalizationHelper.StripQuotationsFromJson(elObj["pass-missmatch"]);
            return;
        }

        //implement registration web request
        StartCoroutine(SendRegistrationRequest(regForName, regSurName, regUsername, regEmail, regPass));
    }


    /*
     * Web Request zur Registrierung
     */
    IEnumerator SendRegistrationRequest(string forname, string surname, string username, string mail, string password)
    {
        using (UnityWebRequest request = new UnityWebRequest("http://h2778219.stratoserver.net/spellbreaker/register.php", UnityWebRequest.kHttpVerbPUT))
        {
            Dictionary<string, string> overlayData = new Dictionary<string, string>();

            overlayData["data"] = "\"sending\"";

            JSONObject overlayDataObj = new JSONObject(overlayData);
            OverlayHandler.ActivateOverlay("common", overlayDataObj[0]);

            request.timeout = timeout;
            UserData userData = new UserData(username, password, forname, surname, mail);
            Debug.Log("Forname: " + userData.forname);
            Debug.Log("Surname: " + userData.surname);
            Debug.Log("User: " + userData.username);
            Debug.Log("Pass: " + userData.password);
            Debug.Log("Mail: " + userData.mail);

            request.downloadHandler = new DownloadHandlerBuffer();
            byte[] payload = System.Text.Encoding.UTF8.GetBytes(userData.SaveToString());
            request.uploadHandler = new UploadHandlerRaw(payload);
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                overlayData = new Dictionary<string, string>();

                overlayData["data"] = "\"register-fail\"";

                overlayDataObj = new JSONObject(overlayData);
                OverlayHandler.ActivateOverlay("register:ui", overlayDataObj[0], true);
                Debug.Log(request);
                Debug.Log("HTTP: " + request.isHttpError);
                Debug.Log("NETWORK: " + request.isNetworkError);
                Debug.Log(request.uploadedBytes);
                Debug.Log(request.responseCode);
                Debug.Log(request.timeout);
                Debug.Log(request.error);
                Debug.Log(request.downloadHandler.text);
            }
            else
            {
                if (request.isDone && request.responseCode == (long)200)
                {
                    // Show results as text
                    Debug.Log(request.downloadHandler.text);
                    ReturnCode rc = ReturnCode.CreateFromJson(request.downloadHandler.text);

                    Debug.Log(rc.returnCode);

                    overlayData = new Dictionary<string, string>();
                    overlayData["data"] = rc.data;
                    overlayDataObj = new JSONObject(overlayData);

                    if (rc.returnCode != 1)
                    {
                        OverlayHandler.ActivateOverlay("register:ui", overlayDataObj[0], true);
                        request.Dispose();
                    }
                    else
                    {
                        OverlayHandler.ActivateOverlay("register:ui", overlayDataObj[0], true);
                        request.Dispose();
                    }
                }
            }
        }
    }


    /*
     * Input Felder überprüfen
     */
    public void CheckLoginFields()
    {
        Debug.Log("Login");

        messageText.text = ""; //CLEAR ANY DISPLAYED MESSAGES TO THE PLAYER

        username = usernameField.text;    //POPULATE THE PRIVATE username VARIABLE WITH THE TEXT THE PLAYER ENTERED INTO THE usernameText INPUT FIELD
        password = passwordField.text;    //POPULATE THE PRIVATE password VARIABLE WITH THE TEXT THE PLAYER ENTERED INTO THE passwordText INPUT FIELD

        if (username == "" || password == "") //IF THE PLAYER HASN'T ENTERED THE REQUIRED INFORMATION...TELL THEM TO
        {
            JSONObject elObj = LocalizationManager.SearchByFilter("login:ui");
            messageText.text = LocalizationHelper.StripQuotationsFromJson(elObj["completation"]);
            return;
        }
        else    //IF ALL INFORMATION IS ENTERED, BUILD A WWWForm AND SEND IT TO THE SERVER
        {
            WWWForm form = new WWWForm();
            form.AddField("username", username);
            form.AddField("password", password);
            SendLoginRequest(username, password);
        }
    }

    /*
     * Daten an den Server senden und Socket Events registrieren
     */
    void SendLoginRequest(string username, string password)
    {
        Debug.Log("LoginRequest");
        Dictionary<string, string> overlayData = new Dictionary<string, string>();
        overlayData["data"] = "\"connecting\"";

        JSONObject overlayDataObj = new JSONObject(overlayData);
        OverlayHandler.ActivateOverlay("common", overlayDataObj[0], false);

        if (!socket.IsConnected)
        {
            socket.Connect();
        }
        else
        {

            Dictionary<string, string> data = new Dictionary<string, string>();

            data["username"] = username;
            data["password"] = password.ToString();

            JSONObject dataObj = new JSONObject(data);
            Debug.Log("[Send Login Data] " + dataObj);
            socket.Emit("user:login", dataObj);
        }

        socket.On("connect", (SocketIOEvent e) =>
        {
            //connectionTries = 0;
            Debug.Log("Connection to Server established.");

            SessionData.SocketID = socket.sid;

            Dictionary<string, string> data = new Dictionary<string, string>();

            //RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            //byte[] bytes = new byte[12];
            //rng.GetBytes(bytes);
            //data["token"] = BitConverter.ToString(bytes);
            data["username"] = username;
            data["password"] = password.ToString();

            JSONObject dataObj = new JSONObject(data);
            Debug.Log("[Send Login Data] " + dataObj);
            
            ///
            /// Socket Emit noch schreiben!
            ///             
            socket.Emit("user:login", dataObj);
            SessionData.UserName = username;
        });

        socket.On("user:login", socketHandler.OnUserLogin);
        socket.On("user:login:notfound", socketHandler.OnError);
        socket.On("user:alreadyLoggedIn", socketHandler.OnError);
        socket.On("connection:error", socketHandler.OnError);

        ///////
        /// Socket Events noch schreiben!
        ///////
        /*
        socket.On("close", );
        */

    }

    #region Delegate Methods

    void OnDisconnect()
    {
        socket.Close();
    }

    public void OnCloseLogin()
    {
        Dictionary<string, string> overlayData = new Dictionary<string, string>();
        overlayData["data"] = "\"no connection\"";
        JSONObject overlayDataObj = new JSONObject(overlayData);
        OverlayHandler.ActivateOverlay("errors", overlayDataObj[0], true);
    }

    #endregion
}
