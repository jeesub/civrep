using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HamiltonChoices : MonoBehaviour
{
    public GameObject choices;

    private TextMeshProUGUI first;
    private TextMeshProUGUI second;
    private TextMeshProUGUI thrid;

    void Start()
    {
        choices.SetActive(false);
    }

    public void HideChoices()
    {
        choices.SetActive(false);
    }

    public void ShowChoices()
    {
        choices.SetActive(true);
        // Ask Airconsole to send message here
    }
}
