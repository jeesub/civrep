using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;

public class AirConsoleReceiverForIntro : MonoBehaviour
{
    public GameObject dialogParent;
    public float interval = 2f;

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
                StartCoroutine(PlayersReady());
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

    private IEnumerator PlayersReady()
    {
        for (int i = 0; i < dialogParent.transform.childCount; i++)
        {
            GameObject dialog = dialogParent.transform.GetChild(i).gameObject;
            Debug.Log(dialog.name);
            yield return new WaitForSeconds(interval);
            dialog.SetActive(true);
        }

        StartCoroutine(GameManager.Instance.LoadNextScene());
    }

    private void OnMessage(int fromDeviceID, JToken data)
    {
        
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
