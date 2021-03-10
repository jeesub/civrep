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

    private List<int> readyReps;

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

    public bool CheckAllPlayerOnStart()
    {
        return (repToId.Count == maxPlayer);
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

    public void GoToDestination(int deviceID, string dest)
    {
        int repIdx = idToRep[deviceID];
        if (dest.Equals("ready"))
        {
            if (!readyReps.Contains(deviceID))
            {
                readyReps.Add(deviceID);
            }
        }
        PrepRoomGuide guide = (PrepRoomGuide)FindObjectOfType(typeof(PrepRoomGuide));
        guide.GuidePrep(repIdx, dest);
    }
}
