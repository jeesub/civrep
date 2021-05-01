using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamiltonIntro : MonoBehaviour
{
    public GameObject hamiltonCanvas;
    public GameObject panel;
    public GameObject capture;
    public GameObject hamiltonUI;

    void Awake()
    {
        hamiltonCanvas.SetActive(false);
    }

    void Update()
    {
        
    }

    private void FlyIn()
    {

    }

    public void DisplayHamiltonUI()
    {
        Debug.Log("Display Hamilton UI");
        hamiltonCanvas.SetActive(true);
        if (panel != null) panel.SetActive(true);
        if (capture != null) capture.SetActive(true);
        if (hamiltonUI != null) hamiltonUI.SetActive(true);
    }
}
