using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinPlacer : MonoBehaviour
{
    // total number of coins
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
        MoveNextCoinAhead();
    }

    private void RemoveCoin()
    {
        if (coins.Count > 0)
        {
            coins.RemoveAt(coins.Count - 1);
            MoveNextCoinBack();
        }        
    }

    private void MoveNextCoinBack()
    {
        Vector2 position = nextCoin.GetComponent<RectTransform>().anchoredPosition;
        if (coins.Count == 4)
        {
            position.x += 4 * interval;
            position.y += 55;
        }
        position.x += interval;
        nextCoin.GetComponent<RectTransform>().anchoredPosition = position;
    }

    private void MoveNextCoinAhead()
    {
        Vector2 position = nextCoin.GetComponent<RectTransform>().anchoredPosition;
        if (coins.Count == 5)
        {
            position.x -= 4 * interval;
            position.y -= 55;
        }
        position.x += interval;
        nextCoin.GetComponent<RectTransform>().anchoredPosition = position;
    }

    public void UpdateCoinNum(int numChange)
    {
        if (numChange > 0)
        {
            for (int i = 0; i < numChange; i++)
            {
                AddCoin();
                num++;
            }
        }
        else if (numChange < 0)
        {
            for (int i = 0; i < Mathf.Abs(numChange); i++)
            {
                RemoveCoin();
                num--;
            } 
        }
    }
}
