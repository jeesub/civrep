﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AirConsoleReceiverForStart : MonoBehaviour
{
    private int maxPlayer;

    private void Awake()
    {
        AirConsole.instance.onMessage += OnMessage;
        AirConsole.instance.onConnect += OnConnect;
        AirConsole.instance.onDisconnect += OnDisconnect;
    }

    private void Start()
    {
        maxPlayer = RepManager.instance.maxPlayer;
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

    private void SelectCharacter(int fromDeviceID, JToken data)
    {
        int repIdx = data["Character"].ToObject<int>();
        Debug.Log("repIdx: " + repIdx);
        if (RepManager.instance.SelectCharacter(fromDeviceID, repIdx))
        {
            JObject messageData = new JObject
                {
                    {"topic", "character selection" },
                    {"message", true }
                };
            AirConsole.instance.Message(fromDeviceID, messageData);
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
        if (RepManager.instance.CheckAllPlayer())
        {
            SceneManager.LoadScene(1);
        }
    }

    private void OnMessage(int fromDeviceID, JToken data)
    {

        Debug.Log("Message from: " + fromDeviceID + "\n Data: " + data);

        if (data["Character"]!=null)
        {
            SelectCharacter(fromDeviceID, data);
            CheckAllPlayer();
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
        if(AirConsole.instance != null)
        {
            AirConsole.instance.onMessage -= OnMessage;
        }
    }
}