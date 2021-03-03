using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;

public class AirConsoleReceiver : MonoBehaviour
{
    private void Awake()
    {
        AirConsole.instance.onMessage += OnMessage;
    }

    private void OnMessage(int fromDeviceID, JToken data)
    {
        Debug.Log("Message from: " + fromDeviceID + "\n Data: " + data);
        if(data["action"] != null && data["action"].ToString().Equals("interact"))
        {
            Debug.Log("Interacted!!");
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
