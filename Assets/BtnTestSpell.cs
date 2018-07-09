using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class BtnTestSpell : MonoBehaviour {

    [SerializeField]
    private SocketIOComponent socket;

	// Use this for initialization
	void Start () {
        socket = GameObject.FindGameObjectWithTag("Logic").transform.Find("SocketIO").GetComponent<SocketIOComponent>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TestSpell()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        Dictionary<string, string[]> values = new Dictionary<string, string[]>();

        data["match_id"] = SessionData.Match.MatchID;
        values["x"] = new string[] { "1.234", "2.234" };
        values["y"] = new string[] { "4.343", "5.132" };
        data["values"] = values.ToString();

        JSONObject dataObj = new JSONObject(data);
        socket.Emit("match:useSpell", dataObj);
    }
}
