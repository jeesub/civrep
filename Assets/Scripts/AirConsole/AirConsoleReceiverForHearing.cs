using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;

public enum EventType
{
    Hearing,
    Discussion,
    Voting,
    Result
}

public enum ControllerPage
{
    idle,
    inventory,
    vote
}

[System.Serializable]
public class CityCouncilEvent
{
    public int eventDuration;
    public EventType curEventType;
    public EventType nextEventType;
    public ControllerPage controllerPage;
}

public class AirConsoleReceiverForHearing : MonoBehaviour
{
    public CityCouncilHost host;
    public List<CityCouncilEvent> sequence = new List<CityCouncilEvent>();

    public List<GameObject> yayReps;
    public List<GameObject> nahReps;

    private SetForecast forecast;

    private void Awake()
    {
        AirConsole.instance.onMessage += OnMessage;
        AirConsole.instance.onConnect += OnConnect;
        AirConsole.instance.onDisconnect += OnDisconnect;
    }

    void Start()
    {
        forecast = GameObject.Find("Canvas-City").GetComponent<SetForecast>();
        forecast.hearing = this;

        NextEventInSequence();
    }

    public void NextEventInSequence()
    {
        if (sequence.Count > 0)
        {
            CityCouncilEvent eventItem = sequence[0];
            sequence.RemoveAt(0);

            // Set the timer
            forecast.ResetRemainTime();
            forecast.SetRemainTime(eventItem.eventDuration);
            // Set the upcoming event name on UI
            forecast.SetSceneName(System.Enum.GetName(typeof(EventType), eventItem.nextEventType));
            // Broadcast message to all controllers
            NoticeController(System.Enum.GetName(typeof(ControllerPage), eventItem.controllerPage));
            // Actually make the event
            host.HostEvent(eventItem.curEventType, eventItem.eventDuration);
        }
    }

    private void NoticeController(string status)
    {
        JObject messageData = new JObject
                {
                    {"topic", "screen" },
                    {"message", status }
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
