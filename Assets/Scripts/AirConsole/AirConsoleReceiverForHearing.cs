using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;



public class AirConsoleReceiverForHearing : MonoBehaviour
{
    public CityCouncilHost host;

    public List<GameObject> yayReps;
    public List<GameObject> nahReps;

    private void Awake()
    {
        AirConsole.instance.onMessage += OnMessage;
        AirConsole.instance.onConnect += OnConnect;
        AirConsole.instance.onDisconnect += OnDisconnect;
    }

    void Start()
    {

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

    private void ProcessVote(int fromDeviceID, JToken data)
    {
        var vote = data["message"].ToString();
        int repIdx = RepManager.instance.getRepIdx(fromDeviceID);
        if (vote.Equals("Yay"))
        {
            host.VoteYea(repIdx);
        }
        else if (vote.Equals("Nah"))
        {
            host.VoteNay(repIdx);
        }
    }

    private void OnMessage(int fromDeviceID, JToken data)
    {
        Debug.Log("Message from: " + fromDeviceID + "\n Data: " + data);
        var topic = data["topic"].ToString();
        Debug.Log("topic is: " + topic);
        switch (topic)
        {
            case "vote":
                ProcessVote(fromDeviceID, data);
                break;
            default:
                break;
        }

        //if (data["Vote"] != null)
        //{
        //    int repIdx = RepManager.instance.getRepIdx(fromDeviceID);
        //    if (data["Vote"].ToString() == "Yay")
        //    {
        //        host.VoteYea(repIdx);
        //    }
        //    else if (data["Vote"].ToString() == "Nah")
        //    {
        //        host.VoteNay(repIdx);
        //    }
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
