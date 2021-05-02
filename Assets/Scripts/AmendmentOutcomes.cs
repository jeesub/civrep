using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class RepImpact
{
    public List<string> yesTexts = new List<string>();
    public List<string> noTexts = new List<string>();
    public int yesImpact;
    public int noImpact;
}

[System.Serializable]
public class CityImpact
{
    public int happiness;
    public int money;
    public int safety;
    public int health;
    public int environment;
}

[System.Serializable]
public class AmendmentImpact
{
    public string draft;
    [TextArea]
    public List<string> hamiltonText = new List<string>();
    public CityImpact cityImpact;
}

public class AmendmentOutcomes : MonoBehaviour
{
    public GameObject hamilton;
    public List<AmendmentImpact> impactList;

    // texts[0] are for passed
    public RepImpact letterImpact;
    public RepImpact intervImpact;

    public string testFinalBill;
    public bool test;

    [Header("End")]
    public List<string> hamiltonFinal = new List<string>();
    public Sprite endSprite;
    public GameObject finalBackground;
    public TextMeshProUGUI research, crafting, drafts;

    private string finalBill;
    private bool letterChecked = false;
    private bool intervChecked = false;
    private bool hamiltonWrapUp = false;
    private AmendmentImpact finalImpact;


    // Start is called before the first frame update
    void Start()
    {
        PrepRoomStatus.instance.gameObject.GetComponent<SetForecast>().SetSceneName("Final Result");

        hamilton.GetComponent<HamiltonTexts>().outcomes = this;

        if (!test)
        {
            finalBill = GameManager.Instance.finalBill;
        }
        else
        {
            finalBill = testFinalBill;
        }

        finalImpact = impactList.Find(x => x.draft.Equals(finalBill));
        if (finalImpact != null)
        {
            // Insert text for city metric
            foreach (string impactText in finalImpact.hamiltonText)
            {
                hamilton.GetComponent<HamiltonTexts>().texts.Add(impactText);
            }

            hamilton.GetComponent<HamiltonIntro>().DisplayHamiltonUI();
            hamilton.GetComponent<HamiltonTexts>().PlayNext();
        }

        // Hide Rep metric and show city metric
        PrepRoomStatus.instance.ShowCityMetric();
        PrepRoomStatus.instance.HideRepMetricInFinal();

        // Record the change in city metric
        PrepRoomStatus.instance.UpdateImpact(finalImpact);
    }

    public bool DisplayRepChanges()
    {
        Debug.Log("DisplayRepChanges");
        // Hide the change in city metric
        PrepRoomStatus.instance.ShowOverallImpact();

        // Hide City metric and show rep metric
        PrepRoomStatus.instance.HideCityMetric();
        PrepRoomStatus.instance.ShowRepMetricInFinal();

        bool hasNextTexts = false;
        if (finalImpact != null)
        {
            // Insert text for letter
            if (!letterChecked && PrepRoomStatus.instance.repStatusItem.letter == 0)
            {
                letterChecked = true;
                hasNextTexts = true;

                // If says yes to letter
                hamilton.GetComponent<HamiltonTexts>().texts.Remove("Hide");
                List<string> impactTexts = finalBill[3].Equals('y') ? letterImpact.yesTexts : letterImpact.noTexts;
                foreach (string impactText in impactTexts)
                {
                    hamilton.GetComponent<HamiltonTexts>().texts.Add(impactText);
                }

                // Show the change in rep metric
                PrepRoomStatus.instance.UpdateRepPoliticalCap(finalBill[3].Equals('y') ? 1 : -1);
            }
            else
            {
                // Insert text for interview
                if (!intervChecked && PrepRoomStatus.instance.repStatusItem.map == 0)
                {
                    intervChecked = true;
                    hasNextTexts = true;

                    // If says yes to interview
                    hamilton.GetComponent<HamiltonTexts>().texts.Remove("Hide");
                    List<string> texts = finalBill[4].Equals('y') ? intervImpact.yesTexts : intervImpact.noTexts;
                    foreach (string impactText in texts)
                    {
                        hamilton.GetComponent<HamiltonTexts>().texts.Add(impactText);
                    }

                    // Show the change in rep metric
                    PrepRoomStatus.instance.UpdateRepPoliticalCap(finalBill[4].Equals('y') ? 1 : -1);
                }
                else if (!hamiltonWrapUp)
                {
                    hamiltonWrapUp = true;
                    hasNextTexts = true;

                    // Both letter and interview are done, hamilton come out to wrap up
                    Debug.Log("Both letter and interview are done");
                    PrepRoomStatus.instance.HideRepMetricInFinal();

                    // Add Hamilton's wrapup
                    hamilton.GetComponent<HamiltonTexts>().texts.Remove("Hide");
                    foreach (string impactText in hamiltonFinal)
                    {
                        hamilton.GetComponent<HamiltonTexts>().texts.Add(impactText);
                    }
                }
                else
                {
                    // Show the end screen
                    finalBackground.GetComponent<Image>().sprite = endSprite;
                    PrepRoomStatus.instance.HideRepMetricInFinal();
                    research.gameObject.SetActive(true);
                    crafting.gameObject.SetActive(true);
                    drafts.gameObject.SetActive(true);
                    research.text = GameManager.Instance.GetResearchTime();
                    crafting.text = GameManager.Instance.GetCraftingTime();
                    drafts.text = GameManager.Instance.GetDraftNum();
                }
            }
        }
        else if (!hamiltonWrapUp)
        {
            hamiltonWrapUp = true;
            hasNextTexts = true;

            // Both letter and interview are done, hamilton come out to wrap up
            Debug.Log("Both letter and interview are done");
            PrepRoomStatus.instance.HideRepMetricInFinal();

            // Add Hamilton's wrapup
            hamilton.GetComponent<HamiltonTexts>().texts.Remove("Hide");
            foreach (string impactText in hamiltonFinal)
            {
                hamilton.GetComponent<HamiltonTexts>().texts.Add(impactText);
            }
        }
        else
        {
            // Show the end screen
            finalBackground.GetComponent<Image>().sprite = endSprite;
            PrepRoomStatus.instance.HideRepMetricInFinal();
            research.gameObject.SetActive(true);
            crafting.gameObject.SetActive(true);
            drafts.gameObject.SetActive(true);
            research.text = GameManager.Instance.GetResearchTime();
            crafting.text = GameManager.Instance.GetCraftingTime();
            drafts.text = GameManager.Instance.GetDraftNum();
        }

        Debug.Log("Playnext");
        hamilton.GetComponent<HamiltonTexts>().PlayNext();
        return hasNextTexts;
    }
}
