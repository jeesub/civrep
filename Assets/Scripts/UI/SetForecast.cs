using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SetForecast : MonoBehaviour
{
    public TextMeshProUGUI sceneName;
    public TextMeshProUGUI timeRemain;

    public AirConsoleReceiverForHearing hearing;

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
            StartCoroutine(GameManager.Instance.LoadNextScene());
        }
        else
        {
            // In City Council Scene, proceed to the next event in sequence
            if (hearing)
            {
                hearing.NextEventInSequence();
            }
        }
        
    }

    public void SetSceneName(string name)
    {
        sceneName.text = name;
    }

    public void SetRemainTime(int seconds)
    {
        remaintime = seconds;
    }
}
