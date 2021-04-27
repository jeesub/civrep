using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using UnityEngine.Playables;
using UnityEngine.UI;

public class AirconsoleReceiverForAmendments : MonoBehaviour
{
    public GameObject hamilton;
    public GameObject cityCanvas;

    private int maxPlayer;

    private AmendmentManager amendmentManager;

    private void Awake()
    {
        AirConsole.instance.onMessage += OnMessage;
        AirConsole.instance.onConnect += OnConnect;
        AirConsole.instance.onDisconnect += OnDisconnect;
    }

    private void Start()
    {
        maxPlayer = RepManager.instance.maxPlayer;

        JObject messageData = new JObject
                {
                    {"topic", "screen" },
                    {"message", "bill" }
                };
        AirConsole.instance.Broadcast(messageData);

        amendmentManager = AmendmentManager.instance;

        cityCanvas = GameObject.Find("Canvas-City");
        if (cityCanvas != null)
        {
            cityCanvas.GetComponent<SetForecast>().SetSceneName("Character Selection");
        }

        //hamilton.GetComponent<HamiltonIntro>().DisplayHamiltonUI();
        //hamilton.GetComponent<HamiltonTexts>().PlayNext();
    }

    private void OnConnect(int device_id)
    {
        if (AirConsole.instance.GetActivePlayerDeviceIds.Count == 0)
        {
            if (AirConsole.instance.GetControllerDeviceIds().Count >= maxPlayer)
            {
                AirConsole.instance.SetActivePlayers(maxPlayer);
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

    private void CraftBill(int fromDeviceID, JToken data)
    {
        string action = data["message"].ToString();
        Debug.Log("message is: " + action);
        switch (action)
        {
            case "1":
                amendmentManager.UpdateAmendmentS1();
                break;
            case "2":
                amendmentManager.UpdateAmendmentS2();
                break;
            case "3":
                amendmentManager.UpdateAmendmentS3();
                break;
            case "4a":
                amendmentManager.UpdateAmendmentS4a();
                break;
            case "4b":
                amendmentManager.UpdateAmendmentS4b();
                break;
            case "draft":
                amendmentManager.ShowDraft();
                break;
            case "redraft":
                amendmentManager.ReDraft();
                break;
            case "submit":
                amendmentManager.SubmitDraft();
                break;
            default:
                break;
        }
    }

    private void OnMessage(int fromDeviceID, JToken data)
    {
        Debug.Log("Message from: " + fromDeviceID + "\n Data: " + data);
        var topic = data["topic"].ToString();
        Debug.Log("topic is: " + topic);
        switch (topic)
        {
            case "bill":
                CraftBill(fromDeviceID, data);
                break;
            default:
                break;
        }
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
        if (AirConsole.instance != null)
        {
            AirConsole.instance.onMessage -= OnMessage;
        }
    }
}
