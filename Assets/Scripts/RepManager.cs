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

    public CharacterSelection repCharacter;
    private List<int> readyCharacters = new List<int>();

    private Dictionary<int, List<string>> repActions = new Dictionary<int, List<string>>();

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

    private void Start()
    {
        // Fill in the repActions
        for (int i = 0; i < maxPlayer; i++)
        {
            repActions.Add(i, new List<string>());
        }
    }

    public int getRepIdx(int deviceID)
    {
        if (idToRep.ContainsKey(deviceID))
        {
            return idToRep[deviceID];
        }
        else
        {
            return -1;
        }
    }

    public bool CheckAllPlayerOnCharacterSelection()
    {
        Debug.Log("CheckAllPlayerOnCharacterSelection");
        return (readyCharacters.Count == maxPlayer);
    }

    public bool CheckAllPlayerOnPrep()
    {
        Debug.Log("Count is: " + readyReps.Count);
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

    private void UpdateRepName(string name)
    {
        PrepRoomStatus.instance.UpdateRepName(name);
        repCharacter.SetName(name);
    }

    private void AddRepName(int deviceID, string name)
    {
        //int repIdx = getRepIdx(deviceID);
        //if (!repToName.ContainsKey(repIdx))
        //{
            //repToName.Add(repIdx, name);
            UpdateRepName(name);
        //}
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

    public void ChangeRepApperance(int deviceID, string appearance)
    {
        //int repIdx = getRepIdx(deviceID);
        //if (repCharacters.Count > repIdx)
        //{
            //CharacterSelection repCharacter = repCharacters[repIdx];
            switch (appearance)
            {
                case "face":
                    repCharacter.ChangeFace();
                    break;
                case "hair":
                    repCharacter.ChangeHair();
                    break;
                case "accessories":
                    repCharacter.ChangeAccessory();
                    break;
                case "ready":
                    //if (!readyCharacters.Contains(repIdx))
                    //{
                        readyCharacters.Add(deviceID);
                    //}
                    break;
                default:
                    break;
            }
        //}
        //else
        //{
        //    Debug.LogError("repIdx out of range: " + repIdx + "/" + repCharacters.Count);
        //}
        
    }

    public void ChangeCommittee(int committee)
    {
        Debug.Log("The committe idx is: " + committee);
        switch(committee)
        {
            case 0:
                repCharacter.ChangeCommittee("Finance");
                repCharacter.ChangeBody("Finance");
                PrepRoomStatus.instance.UpdateCommittee("Finance");
                break;
            case 1:
                repCharacter.ChangeCommittee("PublicService");
                repCharacter.ChangeBody("PublicService");
                PrepRoomStatus.instance.UpdateCommittee("PublicService");
                break;
            case 2:
                repCharacter.ChangeCommittee("Law");
                repCharacter.ChangeBody("Law");
                PrepRoomStatus.instance.UpdateCommittee("Law");
                break;
            default:
                break;
        }
    }

    public void GoToDestination(int deviceID, string dest)
    {
        int repIdx = getRepIdx(deviceID);
        if (dest.Equals("rest"))
        {
            if (!readyReps.Contains(deviceID))
            {
                Debug.Log("adding to ready reps");
                readyReps.Add(deviceID);
            }
        }
        PrepRoomGuide guide = FindObjectOfType<PrepRoomGuide>();
        guide.GuidePrep(repIdx, dest);
    }

    public void RecordRepLetter(int deviceID, bool decision)
    {
        //int repIdx = getRepIdx(deviceID);
        //Debug.Log("Rep" + repIdx + "says " + decision + "to the letter"); 
        PrepRoomStatus.instance.RecordRepLetterDecision(decision);
    }

    public void RecordRepMap(int deviceID, bool decision)
    {
        //int repIdx = getRepIdx(deviceID);
        //Debug.Log("Rep" + repIdx + "says " + decision + "to the map");
        PrepRoomStatus.instance.RecordRepMapDecision(decision);
    }

    public void TakeAction(int deviceID, string action)
    {
        // Can extract this line out as a function
        int repIdx = getRepIdx(deviceID);

        if (!repActions[repIdx].Contains(action))
        {
            JToken resultJson = RepInfos.instance.resultJsons[repIdx];

            int political = resultJson[action]["political"].ToObject<int>();
            int social = resultJson[action]["social"].ToObject<int>();
            int typeIdx = resultJson[action]["type"].ToObject<int>();

            Debug.Log("Political point is: " + political);
            Debug.Log("Social point is: " + social);
            Debug.Log("type value is " + typeIdx);

            // PrepRoomStatus.instance.UpdateRepStatus(repIdx, political, social, typeIdx);

            repActions[repIdx].Add(action);
        }        
    }
}
