using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using TMPro;

[System.Serializable]
public class HamiltonChoiceTexts
{
    public string name;
    public string first;
    public string second;
}

public class HamiltonChoices : MonoBehaviour
{
    public GameObject choices;
    public List<HamiltonChoiceTexts> choiceTexts = new List<HamiltonChoiceTexts>();
    public GameObject indicatorPrefab;
    public List<Sprite> indicators = new List<Sprite>();

    private TextMeshProUGUI first;
    private TextMeshProUGUI second;

    private List<GameObject> firstIndicators = new List<GameObject>();
    private List<GameObject> secondIndicators = new List<GameObject>();

    void Start()
    {
        choices.SetActive(false);
        first = choices.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        second = choices.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void HideChoices()
    {
        choices.SetActive(false);
    }

    public void ShowChoices()
    {
        choices.SetActive(true);

        // Set to correct texts
        HamiltonChoiceTexts choiceText = choiceTexts[0];
        choiceTexts.RemoveAt(0);
        if (choiceText != null)
        {
            first.text = choiceText.first;
            second.text = choiceText.second;

            // Ask Airconsole to send message here
            JObject messageData = new JObject
                {
                    {"topic", "screen" },
                    {"message", choiceText.name }
                };
            AirConsole.instance.Broadcast(messageData);
        }        
    }

    public void RecordAnswer(int fromDeviceID, int answer)
    {
        if (answer == 0)
        {
            // choosing the first
            GameObject indicator = Instantiate(indicatorPrefab,
                new Vector3(first.transform.position.x, first.transform.position.y, first.transform.position.z),
                Quaternion.identity);
            indicator.transform.parent = first.transform;
            indicator.GetComponent<RectTransform>().localScale = Vector3.one;
            indicator.GetComponent<RectTransform>().anchoredPosition = new Vector2(-24 - firstIndicators.Count * 30, -2);
            firstIndicators.Add(indicator);

            int repIdx = RepManager.instance.getRepIdx(fromDeviceID);
            if (repIdx >= 0)
            {
                // idToReps has been set up
                indicator.GetComponent<Image>().sprite = indicators[repIdx];
            }
        }
        else if (answer == 1)
        {
            // choosing the second
            GameObject indicator = Instantiate(indicatorPrefab,
                new Vector3(second.transform.position.x, second.transform.position.y, second.transform.position.z),
                Quaternion.identity);
            indicator.transform.parent = second.transform;
            indicator.GetComponent<RectTransform>().localScale = Vector3.one;
            indicator.GetComponent<RectTransform>().anchoredPosition = new Vector2(-24 - secondIndicators.Count * 30, -2);
            secondIndicators.Add(indicator);

            int repIdx = RepManager.instance.getRepIdx(fromDeviceID);
            if (repIdx >= 0)
            {
                // idToReps has been set up
                indicator.GetComponent<Image>().sprite = indicators[repIdx];
            }
        }        

        if (firstIndicators.Count + secondIndicators.Count >= RepManager.instance.maxPlayer)
        {
            

            StartCoroutine(HideHamilton());            
        }        
    }

    IEnumerator HideHamilton()
    {
        yield return new WaitForSecondsRealtime(3f);
        // Clear the list first
        foreach (GameObject indicator in firstIndicators)
        {
            Destroy(indicator);
        }
        foreach (GameObject indicator in secondIndicators)
        {
            Destroy(indicator);
        }

        firstIndicators.Clear();
        secondIndicators.Clear();

        // Set the result
        GetComponent<HamiltonIntro>().DisplayHamiltonUI();
        GetComponent<HamiltonTexts>().PlayNext();
    }
}
