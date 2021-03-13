using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPlacer : MonoBehaviour
{
    public int num = 0;
    public float interval = 40.0f;
    public List<GameObject> items = new List<GameObject>();
    public GameObject nextItem;
    public GameObject itemPrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void AddInventoryItem(int typeIdx)
    {
        GameObject item = Instantiate(itemPrefab, new Vector3(nextItem.transform.position.x,
                    nextItem.transform.position.y, nextItem.transform.position.z), Quaternion.identity);
        item.transform.parent = transform;
        item.GetComponent<Image>().sprite = RepInfos.instance.inventoryIcons[typeIdx];

        item.gameObject.name = "subItem" + items.Count.ToString();
        items.Add(item);
        UpdateNextItemTrans(1);
    }

    private void UpdateNextItemTrans(int step)
    {
        Vector2 position = nextItem.GetComponent<RectTransform>().anchoredPosition;
        position.x += step * interval;
        nextItem.GetComponent<RectTransform>().anchoredPosition = position;
    }

    public void UpdateItemNum(int typeIdx)
    {
        AddInventoryItem(typeIdx);
    }
}
