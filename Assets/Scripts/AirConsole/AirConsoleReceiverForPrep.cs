using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AirConsoleReceiverForPrep : MonoBehaviour
{
    private int maxPlayer;

    public void TestOnMessage(int fromDeviceID, JToken data)
    {
        OnMessage(fromDeviceID, data);
    }

    private void Awake()
    {
        AirConsole.instance.onMessage += OnMessage;
        AirConsole.instance.onConnect += OnConnect;
        AirConsole.instance.onDisconnect += OnDisconnect;
    }

    private void Start()
    {
        maxPlayer = RepManager.instance.maxPlayer;
        NoticeController();

        GameObject.Find("Canvas-City").GetComponent<SetForecast>().SetSceneName("Research on the bill");
        GameObject.Find("Canvas-City").GetComponent<SetForecast>().ResetRemainTime();
        GameObject.Find("Canvas-City").GetComponent<SetForecast>().SetRemainTime(630);
    }

    private void NoticeController()
    {
        JObject messageData = new JObject
                {
                    {"topic", "screen" },
                    {"message", "prep" }
                };
        AirConsole.instance.Broadcast(messageData);
    }

    private void OnConnect(int device_id)
    {
        if (AirConsole.instance.GetActivePlayerDeviceIds.Count == 0)
        {
            if (AirConsole.instance.GetControllerDeviceIds().Count >= 2)
            {
                AirConsole.instance.SetActivePlayers(2);
                Debug.Log("We have enough players!");
            }
            else
            {
                Debug.Log("id arrays: " + string.Join(",", AirConsole.instance.GetControllerDeviceIds().ToArray()));
                Debug.Log("We need more players");
                Debug.Log("device ids num: " + AirConsole.instance.GetControllerDeviceIds().Count);
            }
        }
    }

    private void GoToDestination(int fromDeviceID, JToken data)
    {
        string dest = data["message"].ToString();
        Debug.Log("Destination is: " + dest);
        if (dest.Equals("main"))
        {
            Debug.Log("Back to main menu");
        }
        else
        {
            RepManager.instance.GoToDestination(fromDeviceID, dest);
        }
    }

    private void TakeAction(int fromDeviceID, JToken data)
    {
        string action = data["message"].ToString();
        Debug.Log("Old Action is: " + action);
        action = action.Replace("-", "");
        Debug.Log("Action is: " + action);
        RepManager.instance.TakeAction(fromDeviceID, action);
    }

    private void CheckAllPlayer()
    {
        if (RepManager.instance.CheckAllPlayerOnPrep())
        {
            StartCoroutine(GameManager.Instance.LoadNextScene());            
        }
    }

    private void RecordLetter(int fromDeviceID, JToken data)
    {
        string decisionStr = data["message"].ToString();
        Debug.Log("Decision on letter is: " + decisionStr);
        bool decision = (decisionStr.Equals("yes"));
        Debug.Log("Convert to bool is: " + decision);
        RepManager.instance.RecordRepLetter(fromDeviceID, decision);
    }

    private void RecordMap(int fromDeviceID, JToken data)
    {
        string decisionStr = data["message"].ToString();
        Debug.Log("Decision on map is: " + decisionStr);
        bool decision = (decisionStr.Equals("yes"));
        Debug.Log("Convert to bool is: " + decision);
        RepManager.instance.RecordRepMap(fromDeviceID, decision);
    }

    private void OnMessage(int fromDeviceID, JToken data)
    {
        Debug.Log("Message from: " + fromDeviceID + "\n Data: " + data);
        var topic = data["topic"].ToString();
        Debug.Log("topic is: " + topic);
        switch (topic)
        {
            case "destination":
                GoToDestination(fromDeviceID, data);
                CheckAllPlayer();
                break;
            case "action":
                //TakeAction(fromDeviceID, data);
                break;
            case "letter":
                RecordLetter(fromDeviceID, data);
                break;
            case "map":
                RecordMap(fromDeviceID, data);
                break;
            case "hamilton":
                break;
            default:
                break;
        }


        //if (data["Destination"]!=null)
        //{
        //    GoToDestination(fromDeviceID, data);
        //    CheckAllPlayer();
        //}
        //else if (data["Action"] != null)
        //{
        //    TakeAction(fromDeviceID, data);
        //}
    }


    private void OnDisconnect(int device_id)
    {
        int active_player = AirConsole.instance.ConvertDeviceIdToPlayerNumber(device_id);
        if (active_player != -1)
        {
            if (AirConsole.instance.GetControllerDeviceIds().Count >= 5)
            {
                AirConsole.instance.SetActivePlayers(5);
            }
            else
            {
                AirConsole.instance.SetActivePlayers(0);
            }
        }
    }

    private void OnDestroy()
    {
        if(AirConsole.instance != null)
        {
            AirConsole.instance.onMessage -= OnMessage;
        }
    }
}
