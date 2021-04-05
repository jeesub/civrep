using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CityImpact
{
    public int happiness;
    public int money;
    public int safety;
    public int health;
}

[System.Serializable]
public class RepImpact
{
    public int letterYes;
    public int letterNo;
    public int mapYes;
    public int mapNo;
}

[System.Serializable]
public class AmendmentImpact
{
    [TextArea]
    public List<string> hamiltonText = new List<string>();
    public RepImpact repImpact;
    public CityImpact cityImpact;
}

[System.Serializable]
public class Amendment
{
    public string name;
    [TextArea]
    public string topic;
    [TextArea]
    public string description;
    public int passNext;
    public int failNext;
    public AmendmentImpact passImpact;
    public AmendmentImpact failImpact;
}

public class AmendmentHost : MonoBehaviour
{
    public List<Amendment> amendments;     

    [Header("Hamilton")]
    public GameObject hamilton;
    public GameObject hCanvas;

    private Amendment curAmend;
    public int numAmend = 0;

    // Start is called before the first frame update
    void Start()
    {
        curAmend = amendments[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public (string, string, int) GetCurAmendment()
    {
        return (curAmend.topic, curAmend.description, numAmend);
    }

    // TODO: add one method to get amendment going

    public (List<string>, bool) ProcessVoteResult(bool result)
    {
        Debug.Log("Processing vote result");
        int nextAmend = result ? curAmend.passNext : curAmend.failNext;
        AmendmentImpact voteImpact = result ? curAmend.passImpact : curAmend.failImpact;
        
        // Accumulate amendment impact
        PrepRoomStatus.instance.UpdateImpact(voteImpact);
        List<string> impactText = voteImpact.hamiltonText;

        // Check whether it's the last amendment
        if (nextAmend < 0)
        {
            // It is the amendment to discuss
            return (impactText, true);
        }

        // Move to next amendment item
        curAmend = amendments[nextAmend];
        numAmend++;
        
        return (impactText, false);
    }
}
