using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinPlacer : MonoBehaviour
{
    // total number of coins
    public int num = 1;
    public int maxNumRow = 3;
    public float horizontalInterval = 30.0f;
    public float verticalInterval = 55f;
    public List<GameObject> coins = new List<GameObject>();
    public GameObject nextCoin;
    public GameObject coinPrefab;

    //public bool decrease;
    //public bool increase;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < num; i++)
        {
            AddCoin();
        }
    }

    private void Update()
    {
        //if (decrease)
        //{
        //    decrease = false;
        //    UpdateCoinNum(-1);
        //}
        //else if (increase)
        //{
        //    increase = false;
        //    UpdateCoinNum(1);
        //}
    }

    private void AddCoin()
    {
        GameObject coin = Instantiate(coinPrefab, new Vector3(nextCoin.transform.position.x,
                    nextCoin.transform.position.y, nextCoin.transform.position.z), Quaternion.identity);
        coin.transform.SetParent(transform);

        coin.gameObject.name = "subCoin" + coins.Count.ToString();
        coins.Add(coin);
        MoveNextCoinAhead();
    }

    private void RemoveCoin()
    {
        if (coins.Count > 0)
        {
            Destroy(coins[coins.Count - 1]);
            coins.RemoveAt(coins.Count - 1);
            MoveNextCoinBack();
        }        
    }

    private void MoveNextCoinBack()
    {
        Vector2 position = nextCoin.GetComponent<RectTransform>().anchoredPosition;
        if (coins.Count == maxNumRow-1)
        {
            position.x += maxNumRow * horizontalInterval;
            position.y += verticalInterval;
        }
        position.x -= horizontalInterval;
        nextCoin.GetComponent<RectTransform>().anchoredPosition = position;
    }

    private void MoveNextCoinAhead()
    {
        Vector2 position = nextCoin.GetComponent<RectTransform>().anchoredPosition;
        if (coins.Count == maxNumRow)
        {
            position.x -= maxNumRow * horizontalInterval;
            position.y -= verticalInterval;
        }
        position.x += horizontalInterval;
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
