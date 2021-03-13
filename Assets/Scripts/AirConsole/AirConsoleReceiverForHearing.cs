using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;

public class AirConsoleReceiverForHearing : MonoBehaviour
{
    public Dictionary<int, int> repToId = new Dictionary<int, int>();
    public Dictionary<int, int> idToReps = new Dictionary<int, int>();
    public List<GameObject> repPhotos;
    public List<GameObject> yayReps;
    public List<GameObject> nahReps;

    public Text votingScores;

    private void Awake()
    {
        AirConsole.instance.onMessage += OnMessage;
        AirConsole.instance.onConnect += OnConnect;
        AirConsole.instance.onDisconnect += OnDisconnect;
    }

    private void Start()
    {
        NoticeController("idle");

        GameObject.Find("Canvas").GetComponent<SetForecast>().SetSceneName("Voting");
        GameObject.Find("Canvas").GetComponent<SetForecast>().SetRemainTime(300);
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

    private void OnMessage(int fromDeviceID, JToken data)
    {
        int active_player = AirConsole.instance.ConvertDeviceIdToPlayerNumber(fromDeviceID);

        Debug.Log("Message from: " + fromDeviceID + "\n Data: " + data);
        Debug.Log("Control Ids length: " + AirConsole.instance.GetControllerDeviceIds().Count);        
        Debug.Log("active player is:" + active_player);
        Debug.Log(data["Character"]);
        
        if (data["Vote"] != null)
        {
            GameObject curRepPhoto = repPhotos[idToReps[fromDeviceID]];
            if (data["Vote"].ToString() == "Yay")
            {
                if (!yayReps.Contains(curRepPhoto))
                {
                    nahReps.Remove(curRepPhoto);
                    yayReps.Add(curRepPhoto);
                }
                curRepPhoto.SetActive(true);
                curRepPhoto.transform.localPosition =
                    new Vector3(-400, curRepPhoto.transform.localPosition.y, 0);
            }
            else if (data["Vote"].ToString() == "Nah")
            {
                if (!nahReps.Contains(curRepPhoto))
                {
                    yayReps.Remove(curRepPhoto);
                    nahReps.Add(curRepPhoto);
                }
                curRepPhoto.SetActive(true);
                curRepPhoto.transform.localPosition =
                    new Vector3(400, curRepPhoto.transform.localPosition.y, 0);
            }

            UpdateVotingScore();
        }
        
    }

    private void UpdateVotingScore()
    {
        votingScores.text = yayReps.Count + " : " + nahReps.Count;
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
