using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // CONSTANT VARIABLES
    // max value of each city status
    private static readonly int MAX_STAT = 5;
    // initial city status: 0 confidence, 1 money, 2 safety, 3 health, 4 environment
    // In the Game Manager, we'll use Enum StatusName
    private static readonly int[] INITIAL_CITY_STAT = {1, 2, 3, 1, 2};

    // STATIC VARIABLES
    // city status: 0 confidence, 1 money, 2 safety, 3 health, 4 environment
    private static int[] cityStatus = new int[5];
    // GameManager instance for Singleton pattern
    private static GameManager instance = null;

    // ENUM
    private enum StatusName
    {
        confidence,
        money,
        safety,
        health,
        environment
    }

    // Singleton pattern
    // Create an instance if necessary.
    // Or destroy this to keep singleton pattern.
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

    // Instance to use
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    // Start Game
    // call this function using GameManager.Instance.StartGame()
    public void StartGame()
    {
        ResetCityStatus();
    }

    // Reset city status to initial values
    public void ResetCityStatus()
    {
        System.Array.Copy(INITIAL_CITY_STAT, cityStatus, cityStatus.Length);
    }

    // call this function using GameManager.Instance.GetStatus(string StatusName)
    public int GetStatus(string statusName)
    {
        int idx = getIndexFromName(statusName);
        return GetStatus(idx);
    }

    private int GetStatus(int idx)
    {
        return cityStatus[idx];
    }

    // call this function using GameManager.Instance.IncreaseStatus(int idx, int num)
    public void IncreaseStatus(string statusName, int num)
    {
        int idx = getIndexFromName(statusName);
        IncreaseStatus(idx, num);
    }

    private void IncreaseStatus(int idx, int num)
    {
        int val = cityStatus[idx];
        if (val + num >= 0 && val + num <= MAX_STAT)
        {
            cityStatus[idx] = val + num;
        }
        else
        {
            cityStatus[idx] = MAX_STAT;
        }
        Debug.Log("New value = " + cityStatus[val]);
    }


    // call this function using GameManager.Instance.DecreaseStatus(int idx, int num)
    public void DecreaseStatus(string statusName, int num)
    {
        int idx = getIndexFromName(statusName);
        DecreaseStatus(idx, num);
    }

    private void DecreaseStatus(int idx, int num)
    {
        int val = cityStatus[idx];
        if (val - num >= 0 && val - num <= MAX_STAT)
        {
            cityStatus[idx] = val - num;
        }
        else
        {
            cityStatus[idx] = 0;
        }
        Debug.Log("New value = " + cityStatus[val]);
    }

    private int getIndexFromName(string statusName)
    {
        switch (statusName)
        {
            case "confidence":
                return (int)StatusName.confidence;
            case "money":
                return (int)StatusName.money;
            case "safety":
                return (int)StatusName.safety;
            case "health":
                return (int)StatusName.health;
            case "environment":
                return (int)StatusName.environment;
            default:
                throw new System.ArgumentException("Cannot find a status name: " + statusName);
        }
    }
}
