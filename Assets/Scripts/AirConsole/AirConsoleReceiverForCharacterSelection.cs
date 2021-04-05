using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AirConsoleReceiverForCharacterSelection : MonoBehaviour
{
    public int sessionTime = 500;

    public GameObject cityCanvas;
    public GameObject hamilton;

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
        //cityCanvas.SetActive(false);
        cityCanvas.GetComponent<Canvas>().enabled = false;

        maxPlayer = RepManager.instance.maxPlayer;
        NoticeController();

        cityCanvas.GetComponent<SetForecast>().SetSceneName("Character Selection");
        cityCanvas.GetComponent<SetForecast>().ResetRemainTime();
        cityCanvas.GetComponent<SetForecast>().SetRemainTime(sessionTime);
    }

    private void NoticeController()
    {
        JObject messageData = new JObject
                {
                    {"topic", "screen" },
                    {"message", "character" }
                };
        AirConsole.instance.Broadcast(messageData);
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

    private JObject SendRepInfo(int repIdx)
    {
        RepInfo info = RepInfos.instance.infos[repIdx];
        string infoStr = JsonUtility.ToJson(info);
        return JObject.Parse(infoStr);
    }

    private void SelectCharacter(int fromDeviceID, JToken data)
    {
        int repIdx = data["message"].ToObject<int>();
        Debug.Log("repIdx: " + repIdx);
        if (RepManager.instance.SelectCharacter(fromDeviceID, repIdx))
        {
            JObject messageData = new JObject
                {
                    {"topic", "character selection" },
                    {"message", true }
                };
            AirConsole.instance.Message(fromDeviceID, messageData);

            /*
            // Send the prep room info in a batch
            JObject repInfo = SendRepInfo(repIdx);
            JObject info = new JObject
            {
                {"topic", "info"},
                {"message", repInfo}
            };
            Debug.Log("info is: " + info);            
            AirConsole.instance.Message(fromDeviceID, info);
            */
        }
        else
        {
            // Rep was selected by other players
            JObject messageData = new JObject
                {
                    {"topic", "character selection" },
                    {"message", false }
                };
            AirConsole.instance.Message(fromDeviceID, messageData);
        }
    }

    private void CheckAllPlayer()
    {
        if (RepManager.instance.CheckAllPlayerOnCharacterSelection())
        {
            //SceneManager.LoadScene(1);
            //StartCoroutine(GameManager.Instance.LoadNextScene());
            //cityCanvas.SetActive(true);
            cityCanvas.GetComponent<Canvas>().enabled = true;
            hamilton.GetComponent<PlayableDirector>().Play();
        }
    }

    private void SetRepName(int fromDeviceID, JToken data)
    {
        string name = data["message"].ToString();
        RepManager.instance.SetRepName(fromDeviceID, name);
    }

    private void OnMessage(int fromDeviceID, JToken data)
    {
        Debug.Log("Message from: " + fromDeviceID + "\n Data: " + data);
        var topic = data["topic"].ToString();
        Debug.Log("topic is: " + topic);
        switch(topic)
        {
            case "name":
                SetRepName(fromDeviceID, data);
                CheckAllPlayer();
                break;
            case "character":
                SelectCharacter(fromDeviceID, data);
                break;
            case "appearance":                
                break;
            case "hamilton":
                break;
            default:
                break;
        }

        //if (data["Character"]!=null)
        //{
        //    SelectCharacter(fromDeviceID, data);
        //}        
        //else if (data["Connected"]!=null)
        //{
        //    SetRepName(fromDeviceID, data);
        //    CheckAllPlayer();
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
