using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MapInfo
{
    public string place;
    public string info;
}

[System.Serializable]
public class RepInfo
{
    public string letter0;
    public string letter1;
    public MapInfo map0;
    public MapInfo map1;
    public string computer0;
    public string computer1;
}

public enum InfoType
{
    letter,
    institution,
    research,
    news,
    docs,
    conversation
}

[System.Serializable]
public class InfoResult
{
    [Range(0,5)]
    public int political;
    [Range(0, 5)]
    public int social;
    public InfoType type;
}

[System.Serializable]
public class RepInfoResult
{
    public InfoResult letter0;
    public InfoResult letter1;
    public InfoResult map0;
    public InfoResult map1;
    public InfoResult computer0;
    public InfoResult computer1;
}

public class RepInfos : MonoBehaviour
{
    public static RepInfos instance;

    public List<RepInfo> infos;

    public List<RepInfoResult> results;

    public List<Sprite> inventoryIcons = new List<Sprite>();

    public List<JToken> resultJsons = new List<JToken>();

    void Awake()
    {
        if (instance)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (RepInfoResult result in results)
        {
            JToken resultJson = JObject.Parse(JsonUtility.ToJson(result));
            resultJsons.Add(resultJson);
            Debug.Log("The result json is: " + resultJson);
        }
    }
}
