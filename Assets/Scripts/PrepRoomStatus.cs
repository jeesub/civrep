﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class RepStatusItem
{
    public TextMeshProUGUI Name;
    public Image background;
    public GameObject political;
    public GameObject inventory;
    // -1: not chosen; 0: positive; 1: negative
    public int letter = -1;
    public int map = -1;
    public int changeInPolitical = 0;
}

public class PrepRoomStatus : MonoBehaviour
{
    //public List<RepStatusItem> repStatusItems;
    public RepStatusItem repStatusItem;
    public List<ScaledStats> cityMetrics;

    // Accumulate the impact on city metrics
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
        //for (int repIdx = 0; repIdx < repStatusItems.Count; repIdx++)
        //{
        //    RepStatusItem item = repStatusItems[repIdx];
        //    item.Name.text = "Stranger";
        //    ResetRepStatus(item);
        //}
        repStatusItem.Name.text = "Stranger";
        ResetRepStatus(repStatusItem);
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

    public void UpdateRepName(string name)
    {
        repStatusItem.Name.text = name;
        Debug.Log("Changing rep's name to " + name);

        //if (repIdx < repStatusItems.Count)
        //{
        //    RepStatusItem statusItem = repStatusItems[repIdx];
        //    statusItem.Name.text = name;
        //    Debug.Log("Changing rep " + repIdx + "'s name to " + name);
        //}
        //else
        //{
        //    Debug.Log("repIdx is too large");
        //    Debug.Log("repIdx: " + repIdx);
        //    Debug.Log("count: " + repStatusItems.Count);
        //}
    }

    public void UpdateCommittee(string committee)
    {
        repStatusItem.background.sprite = Resources.Load<Sprite>("Sprites/RepMetric/" + committee);
    }

    public void RecordRepLetterDecision(bool decision)
    {
        repStatusItem.letter = decision ? 0 : 1;

        //if (repIdx < repStatusItems.Count)
        //{
        //    RepStatusItem statusItem = repStatusItems[repIdx];
        //    statusItem.letter = decision ? 0 : 1;
        //}
        //else
        //{
        //    Debug.Log("repIdx is too large");
        //    Debug.Log("repIdx: " + repIdx);
        //    Debug.Log("count: " + repStatusItems.Count);
        //}
    }

    public void RecordRepMapDecision(bool decision)
    {
        repStatusItem.map = decision ? 0 : 1;

        //if (repIdx < repStatusItems.Count)
        //{
        //    RepStatusItem statusItem = repStatusItems[repIdx];
        //    statusItem.map = decision ? 0 : 1;
        //}
        //else
        //{
        //    Debug.Log("repIdx is too large");
        //    Debug.Log("repIdx: " + repIdx);
        //    Debug.Log("count: " + repStatusItems.Count);
        //}
    }

    /*
    public void AddRepInventory(int typeIdx)
    {
        repStatusItem.inventory.GetComponent<InventoryPlacer>().UpdateItemNum(typeIdx);

        //if (repIdx < repStatusItems.Count)
        //{
        //    RepStatusItem statusItem = repStatusItems[repIdx];
        //    statusItem.inventory.GetComponent<InventoryPlacer>().UpdateItemNum(typeIdx);
        //}
        //else
        //{
        //    Debug.Log("repIdx is too large");
        //    Debug.Log("repIdx: " + repIdx);
        //    Debug.Log("count: " + repStatusItems.Count);
        //}
    }
    */

    public void UpdateRepPoliticalCap(int political)
    {
        repStatusItem.political.GetComponent<CoinPlacer>().UpdateCoinNum(political);

        //if (repIdx < repStatusItems.Count)
        //{
        //    RepStatusItem statusItem = repStatusItems[repIdx];
        //    statusItem.political.GetComponent<CoinPlacer>().UpdateCoinNum(political);
        //}
        //else
        //{
        //    Debug.Log("repIdx is too large");
        //    Debug.Log("repIdx: " + repIdx);
        //    Debug.Log("count: " + repStatusItems.Count);
        //}
    }

    //private void UpdateRepImpact(RepImpact impact)
    //{
        //for (int repIdx = 0; repIdx < repStatusItems.Count; repIdx++)
        //{
            
            //RepStatusItem item = repStatusItems[repIdx];
            //int totalChange = 0;
            //if (repStatusItem.letter >= 0)
            //{
            //    // Rep responds to the letter
            //    int capitalChange = (repStatusItem.letter == 0) ? impact.letterYes : impact.letterNo;
            //    totalChange += capitalChange;
            //}
            //if (repStatusItem.map >= 0)
            //{
            //    // Rep responds to the interview
            //    int capitalChange = (repStatusItem.map == 0) ? impact.mapYes : impact.mapNo;                
            //    totalChange += capitalChange;
            //}

            //// Update the accumulate change on Rep
            //Debug.Log("Rep has total PC change of " + totalChange);
            //repStatusItem.changeInPolitical += totalChange;
            //// Update the UI
            //Debug.Log("Updating PC coins");
            //repStatusItem.political.GetComponent<CoinPlacer>().UpdateCoinNum(totalChange);
        //}
    //}

    private void ResetCityImpact()
    {
        cityImpact.happiness = 0;
        cityImpact.money = 0;
        cityImpact.safety = 0;
        cityImpact.health = 0;      
        cityImpact.environment = 0;      
    }

    private void UpdateCityImpact(CityImpact impact)
    {
        cityImpact.happiness += impact.happiness;
        cityImpact.money += impact.money;
        cityImpact.safety += impact.safety;
        cityImpact.health += impact.health;
        cityImpact.environment += impact.environment;        

        cityMetrics[0].ChangeScale(impact.happiness);
        cityMetrics[1].ChangeScale(impact.money);
        cityMetrics[2].ChangeScale(impact.health);
        cityMetrics[3].ChangeScale(impact.safety);
        cityMetrics[4].ChangeScale(impact.environment);
    } 

    public void HideCityImpactChange()
    {
        cityMetrics[0].ChangeScale(0);
        cityMetrics[1].ChangeScale(0);
        cityMetrics[2].ChangeScale(0);
        cityMetrics[3].ChangeScale(0);
        cityMetrics[4].ChangeScale(0);
    }

    public void UpdateImpact(AmendmentImpact voteImpact)
    {
        //UpdateRepImpact(voteImpact.repImpact);
        UpdateCityImpact(voteImpact.cityImpact);        
    }

    public void ShowOverallImpact()
    {
        cityMetrics[0].ShowOverallChange(cityImpact.happiness);
        cityMetrics[1].ShowOverallChange(cityImpact.money);
        cityMetrics[2].ShowOverallChange(cityImpact.health);
        cityMetrics[3].ShowOverallChange(cityImpact.safety);
        cityMetrics[4].ShowOverallChange(cityImpact.environment);

        ResetCityImpact();
    }

    public void HideOverallImpact()
    {
        cityMetrics[0].HideOverallChange();
        cityMetrics[1].HideOverallChange();
        cityMetrics[2].HideOverallChange();
        cityMetrics[3].HideOverallChange();
        cityMetrics[4].HideOverallChange();
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
