using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamiltonIntro : MonoBehaviour
{
    public GameObject hamiltonUI;

    void Start()
    {
        hamiltonUI.SetActive(false);
    }

    void Update()
    {
        
    }

    private void FlyIn()
    {

    }

    public void DisplayHamiltonUI()
    {
        hamiltonUI.SetActive(true);
    }
}
