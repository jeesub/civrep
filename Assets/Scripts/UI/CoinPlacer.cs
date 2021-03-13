using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPlacer : MonoBehaviour
{
    public int num = 1;
    public float interval = 30.0f;
    public List<GameObject> coins = new List<GameObject>();
    public GameObject nextCoin;
    public GameObject coinPrefab;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < num; i++)
        {
            AddCoin();
        }
    }

    private void AddCoin()
    {
        GameObject coin = Instantiate(coinPrefab, new Vector3(nextCoin.transform.position.x,
                    nextCoin.transform.position.y, nextCoin.transform.position.z), Quaternion.identity);
        coin.transform.parent = transform;

        coin.gameObject.name = "subCoin" + coins.Count.ToString();
        coins.Add(coin);
        UpdateNextCoinTrans(1);
    }

    private void UpdateNextCoinTrans(int step)
    {
        Vector2 position = nextCoin.GetComponent<RectTransform>().anchoredPosition;
        position.x += step * interval;
        nextCoin.GetComponent<RectTransform>().anchoredPosition = position;
    }

    public void UpdateCoinNum(int coinNum)
    {
        if (coinNum >= 0)
        {
            for (int i = 0; i < coinNum; i++)
            {
                AddCoin();
                num++;
            }
        }
    }
}
