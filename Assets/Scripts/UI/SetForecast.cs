using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SetForecast : MonoBehaviour
{
    public TextMeshProUGUI sceneName;
    public TextMeshProUGUI timeRemain;
    public TextMeshProUGUI staticText;

    public CityCouncilHost hearing;

    public int remaintime = -1;
    public bool updatingTime = false;

    void Start()
    {

    }

    void Update()
    {
        UpdateRemainTime();
    }

    public void ResetRemainTime()
    {
        remaintime = -1;
        updatingTime = false;
        StopCoroutine("ProcessRemainTime");
    }

    private void UpdateRemainTime()
    {
        if (!updatingTime && remaintime >= 0 )
        {
            StartCoroutine("ProcessRemainTime");
        }
    }

    private IEnumerator ProcessRemainTime()
    {
        updatingTime = true;
        while (remaintime >= 0)
        {
            int minute = remaintime / 60;
            int sec = remaintime % 60;

            string minStr = (minute < 10) ? ("0" + minute.ToString()) : minute.ToString();
            string secStr = (sec < 10) ? ("0" + sec.ToString()) : sec.ToString();
            timeRemain.text = minStr + ":" + secStr;

            yield return new WaitForSeconds(1f);
            remaintime--;
        }

        if (SceneManager.GetActiveScene().buildIndex != 3)
        {
            // Load next Scene if it's not the City Council scene
            Debug.Log("going to next scene");
            StartCoroutine(GameManager.Instance.LoadNextScene());
        }
        else
        {
            // In City Council Scene, proceed to the next event in sequence
            if (hearing)
            {
                hearing.EventTimeUp();
            }
        }
        
    }

    public void SetRemainTime(int seconds)
    {
        if (seconds > 0) 
        {
            ShowRemainTime(true);
            remaintime = seconds;            
        }
        else
        {
            ShowRemainTime(false);
        }
    }

    private void ShowRemainTime(bool state)
    {
        staticText.gameObject.SetActive(state);
        timeRemain.gameObject.SetActive(state);
    }

    public void SetSceneName(string name)
    {
        sceneName.enabled = true;
        sceneName.text = name;
    }

    public void HideSceneName()
    {
        sceneName.enabled = false;
    }
}
