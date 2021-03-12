using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;


public enum InfoSource
{
    letter0,
    letter1,
    map0,
    map1,
    computer0,
    computer1,
}

public class test : MonoBehaviour
{
    public bool testAction = false;
    public InfoSource typeIdx;

    public int id = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (testAction)
        {
            string type = System.Enum.GetName(typeof(InfoSource), typeIdx);
            JToken data = new JObject
            {
                { "Action", type}
            };
            GetComponent<AirConsoleReceiverForPrep>().TestOnMessage(id, data);
            testAction = false;
        }
    }
}
