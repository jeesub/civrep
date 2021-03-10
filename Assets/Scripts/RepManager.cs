using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class RepManager : MonoBehaviour
{
    public static RepManager instance;

    public int maxPlayer = 2;

    // Map rep idx to airconsole device id
    private Dictionary<int, int> repToId = new Dictionary<int, int>();
    // Map airconsole device id to rep idx
    private Dictionary<int, int> idToRep = new Dictionary<int, int>();

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

    public bool CheckAllPlayer()
    {
        return (repToId.Count == maxPlayer);
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

}
