using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class RepStatusItem
{
    public TextMeshProUGUI Name;
    public GameObject supportBar;
    public GameObject political;
    public GameObject social;
    public GameObject inventory;
}

public class PrepRoomStatus : MonoBehaviour
{
    public List<RepStatusItem> repStatusItems;

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
            item.Name.text = "Bennie";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateRepStatus(int repIdx, int political, int social, int typeIdx)
    {
        if (repIdx < repStatusItems.Count)
        {
            RepStatusItem statusItem = repStatusItems[repIdx];
            statusItem.political.GetComponent<CoinPlacer>().UpdateCoinNum(political);
            statusItem.social.GetComponent<CoinPlacer>().UpdateCoinNum(social);
            statusItem.inventory.GetComponent<InventoryPlacer>().UpdateItemNum(typeIdx);
        }
        else
        {
            Debug.Log("repIdx is too large");
            Debug.Log("repIdx: " + repIdx);
            Debug.Log("count: " + repStatusItems.Count);
        }
    }

    public void UpdateRepStatus(int repIdx, float popularity, int political, int social, int typeIdx)
    {
        if (repIdx < repStatusItems.Count)
        {
            RepStatusItem statusItem = repStatusItems[repIdx];
            //statusItem.supportBar
            statusItem.political.GetComponent<CoinPlacer>().UpdateCoinNum(political);
            statusItem.social.GetComponent<CoinPlacer>().UpdateCoinNum(social);
            statusItem.inventory.GetComponent<InventoryPlacer>().UpdateItemNum(typeIdx);
        }
    }
}
