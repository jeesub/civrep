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
    public RepImpact repImpact;
    public CityImpact cityImpact;
}

[System.Serializable]
public class Amendment
{
    public string name;
    public int passNext;
    public int failNext;
    public AmendmentImpact passImpact;
    public AmendmentImpact failImpact;
}

public class AmendmentHost : MonoBehaviour
{
    public List<Amendment> amendments;     

    private Amendment curAmend;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // TODO: add one method to get amendment going

    public bool ProcessVoteResult(bool result)
    {
        int nextAmend = result ? curAmend.passNext : curAmend.failNext;
        AmendmentImpact voteImpact = result ? curAmend.passImpact : curAmend.failImpact;
        if (nextAmend < 0 )
        {
            // It is the amendment to discuss
            return false;
        }
        // Accumulate amendment impact
        PrepRoomStatus.instance.UpdateImpact(voteImpact);

        // TODO: Call hamilton out

        // Move to next amendment item
        curAmend = amendments[nextAmend];
        return true;
    }
}
