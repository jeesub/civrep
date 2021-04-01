using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class RepStatusItem
{
    public TextMeshProUGUI Name;
    public GameObject political;
    public GameObject inventory;
    // -1: not chosen; 0: positive; 1: negative
    public int letter = -1;
    public int map = -1;
    public int changeInPolitical = 0;
}

public class PrepRoomStatus : MonoBehaviour
{
    public List<RepStatusItem> repStatusItems;

    public CityImpact cityImpact;

    public static PrepRoomStatus instance;

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

    // Start is called before the first frame update
    void Start()
    {   
        for (int repIdx = 0; repIdx < repStatusItems.Count; repIdx++)
        {
            RepStatusItem item = repStatusItems[repIdx];
            item.Name.text = "Stranger";
            ResetRepStatus(item);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ResetRepStatus(RepStatusItem rep)
    {
        rep.letter = -1;
        rep.map = -1;
        rep.changeInPolitical = 0;
    }

    public void UpdateRepName(int repIdx, string name)
    {
        if (repIdx < repStatusItems.Count)
        {
            RepStatusItem statusItem = repStatusItems[repIdx];
            statusItem.Name.text = name;
            Debug.Log("Changing rep " + repIdx + "'s name to " + name);
        }
        else
        {
            Debug.Log("repIdx is too large");
            Debug.Log("repIdx: " + repIdx);
            Debug.Log("count: " + repStatusItems.Count);
        }
    }

    public void RecordRepLetterDecision(int repIdx, bool decision)
    {
        if (repIdx < repStatusItems.Count)
        {
            RepStatusItem statusItem = repStatusItems[repIdx];
            statusItem.letter = decision ? 0 : 1;
        }
        else
        {
            Debug.Log("repIdx is too large");
            Debug.Log("repIdx: " + repIdx);
            Debug.Log("count: " + repStatusItems.Count);
        }
    }

    public void RecordRepMapDecision(int repIdx, bool decision)
    {
        if (repIdx < repStatusItems.Count)
        {
            RepStatusItem statusItem = repStatusItems[repIdx];
            statusItem.map = decision ? 0 : 1;
        }
        else
        {
            Debug.Log("repIdx is too large");
            Debug.Log("repIdx: " + repIdx);
            Debug.Log("count: " + repStatusItems.Count);
        }
    }

    public void AddRepInventory(int repIdx, int typeIdx)
    {
        if (repIdx < repStatusItems.Count)
        {
            RepStatusItem statusItem = repStatusItems[repIdx];
            statusItem.inventory.GetComponent<InventoryPlacer>().UpdateItemNum(typeIdx);
        }
        else
        {
            Debug.Log("repIdx is too large");
            Debug.Log("repIdx: " + repIdx);
            Debug.Log("count: " + repStatusItems.Count);
        }
    }

    public void UpdateRepPoliticalCap(int repIdx, int political)
    {
        if (repIdx < repStatusItems.Count)
        {
            RepStatusItem statusItem = repStatusItems[repIdx];
            statusItem.political.GetComponent<CoinPlacer>().UpdateCoinNum(political);
        }
        else
        {
            Debug.Log("repIdx is too large");
            Debug.Log("repIdx: " + repIdx);
            Debug.Log("count: " + repStatusItems.Count);
        }
    }

    private void UpdateRepImpact(RepImpact impact)
    {
        for (int repIdx = 0; repIdx < repStatusItems.Count; repIdx++)
        {
            RepStatusItem item = repStatusItems[repIdx];
            if (item.letter >= 0)
            {
                // Rep responds to the letter
                item.changeInPolitical += (item.letter == 0) ? impact.letterYes : impact.letterNo;
            }
            if (item.map >= 0)
            {
                // Rep responds to the interview
                item.changeInPolitical += (item.map == 0) ? impact.mapYes : impact.mapNo;
            }
        }
    }

    private void ResetCityImpact()
    {
        cityImpact.happiness = 0;
        cityImpact.money = 0;
        cityImpact.safety = 0;
        cityImpact.health = 0;
    }

    private void UpdateCityImpact(CityImpact impact)
    {
        cityImpact.happiness += impact.happiness;
        cityImpact.money += impact.money;
        cityImpact.safety += impact.safety;
        cityImpact.health += impact.health;
    } 

    public void UpdateImpact(AmendmentImpact voteImpact)
    {
        UpdateRepImpact(voteImpact.repImpact);
        UpdateCityImpact(voteImpact.cityImpact);        
    }

    //public void UpdateRepStatus(int repIdx, int political, int social, int typeIdx)
    //{
    //    if (repIdx < repStatusItems.Count)
    //    {
    //        RepStatusItem statusItem = repStatusItems[repIdx];
    //        statusItem.political.GetComponent<CoinPlacer>().UpdateCoinNum(political);
    //        statusItem.social.GetComponent<CoinPlacer>().UpdateCoinNum(social);
    //        statusItem.inventory.GetComponent<InventoryPlacer>().UpdateItemNum(typeIdx);
    //    }
    //    else
    //    {
    //        Debug.Log("repIdx is too large");
    //        Debug.Log("repIdx: " + repIdx);
    //        Debug.Log("count: " + repStatusItems.Count);
    //    }
    //}

    //public void UpdateRepStatus(int repIdx, float popularity, int political, int social, int typeIdx)
    //{
    //    if (repIdx < repStatusItems.Count)
    //    {
    //        RepStatusItem statusItem = repStatusItems[repIdx];
    //        //statusItem.supportBar
    //        statusItem.political.GetComponent<CoinPlacer>().UpdateCoinNum(political);
    //        statusItem.social.GetComponent<CoinPlacer>().UpdateCoinNum(social);
    //        statusItem.inventory.GetComponent<InventoryPlacer>().UpdateItemNum(typeIdx);
    //    }
    //}
}
