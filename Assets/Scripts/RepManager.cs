using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class RepManager : MonoBehaviour
{
    public static RepManager instance;

    public int maxPlayer = 2;

    // Map airconsole device id to rep idx
    private Dictionary<int, int> idToRep = new Dictionary<int, int>();
    // Map rep idx to airconsole device id, set this up just in case
    private Dictionary<int, int> repToId = new Dictionary<int, int>();
    // Map deviceID to player's name
    private Dictionary<int, string> idToName = new Dictionary<int, string>();
    // Map repIdx to player's name
    public Dictionary<int, string> repToName = new Dictionary<int, string>();

    private List<int> readyReps = new List<int>();

    private void Awake()
    {
        if (instance)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public bool CheckAllPlayerOnCharacterSelection()
    {
        
        return (idToName.Count == maxPlayer);
    }

    public bool CheckAllPlayerOnPrep()
    {
        return (readyReps.Count == maxPlayer);
    }

    public bool SelectCharacter(int deviceID, int repIdx)
    {
        if (idToRep.ContainsKey(deviceID) || idToRep.ContainsValue(repIdx))
        {
            return false;
        }
        else
        {
            idToRep.Add(deviceID, repIdx);
            repToId.Add(repIdx, deviceID);
            return true;
        }
    } 

    private void UpdateRepName(int repIdx, string name)
    {
        PrepRoomStatus.instance.UpdateRepName(repIdx, name);
    }

    private void AddRepName(int deviceID, string name)
    {
        int repIdx = idToRep[deviceID];
        if (!repToName.ContainsKey(repIdx))
        {
            repToName.Add(repIdx, name);
            UpdateRepName(repIdx, name);
        }
    }

    public void SetRepName(int deviceID, string name)
    {
        if (!idToName.ContainsKey(deviceID))
        {
            idToName.Add(deviceID, name);

            // Add Rep Name
            AddRepName(deviceID, name);
        }
        else
        {
            // Only for testing
            Debug.Log("Only for testing");
            idToName.Remove(deviceID);
            idToName.Add(deviceID, name);
            AddRepName(deviceID, name);
        }
    }

    public void GoToDestination(int deviceID, string dest)
    {
        int repIdx = idToRep[deviceID];
        if (dest.Equals("rest"))
        {
            if (!readyReps.Contains(deviceID))
            {
                readyReps.Add(deviceID);
            }
        }
        PrepRoomGuide guide = FindObjectOfType<PrepRoomGuide>();
        guide.GuidePrep(repIdx, dest);
    }

    public void TakeAction(int deviceID, string action)
    {
        // Can extract this line out as a function
        int repIdx = idToRep[deviceID];
        JToken resultJson = RepInfos.instance.resultJsons[repIdx];        

        int political = resultJson[action]["political"].ToObject<int>();
        int social = resultJson[action]["social"].ToObject<int>();
        int typeIdx = resultJson[action]["type"].ToObject<int>();

        Debug.Log("Political point is: " + political);
        Debug.Log("Social point is: " + social);
        Debug.Log("type value is " + typeIdx);

        PrepRoomStatus.instance.UpdateRepStatus(repIdx, political, social, typeIdx);
    }
}
