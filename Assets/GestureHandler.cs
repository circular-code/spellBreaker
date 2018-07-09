using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GestureRecognizer;
using SocketIO;

public class GestureHandler : MonoBehaviour {

    [SerializeField]
    private SocketIOComponent socket;
    [SerializeField]
    private DrawDetector detector;
    [SerializeField]
    private Recognizer recognizer;

    [SerializeField]
    private bool testMode;

    // Use this for initialization
    void Start () {
        if(GameObject.FindGameObjectWithTag("Logic"))
        {
            socket = GameObject.FindGameObjectWithTag("Logic").transform.GetChild(0).GetComponent<SocketIOComponent>();
        }
        Debug.Log(GameObject.FindGameObjectWithTag("Logic").gameObject);

        Lean.Touch.LeanTouch.OnFingerDown += onFingerDown;
    }

    private void onFingerDown(Lean.Touch.LeanFinger finger)
    {
        //Debug.Log("Finger down event");
        if(Lean.Touch.LeanTouch.Fingers.Count > 1)
        {
            //Debug.Log("Multiple Fingers Down -  Clear Lines");
            detector.ClearLines();
        }
    }

    public void SendSpellToServer(RecognitionResult result, GestureData gestureData)
    {
        Debug.Log("SendSpellToServer");
        if(gestureData != null)
        {
            SpellCastToJson values = new SpellCastToJson();
            if(!testMode)
            {
                values.match_id = SessionData.Match.MatchID;
            }

            values.gestureData = gestureData;

            /*foreach (GestureLine line in gestureData.lines)
            {
                foreach (Vector2 point in line.points)
                {
                    values.x.Add(point.x);
                    values.y.Add(point.y);
                }
            }*/

            if (socket)
            {
                JSONObject dataObj = new JSONObject(values.SaveToString());
                //Debug.Log(dataObj);

                socket.Emit("player:cast:spell", dataObj);
            }
        }        
    }    
}
