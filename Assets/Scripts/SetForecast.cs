using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SetForecast : MonoBehaviour
{
    public TextMeshProUGUI sceneName;
    public TextMeshProUGUI timeRemain;

    private int remaintime = -1;
    private bool updatingTime = false;

    void Start()
    {

    }

    void Update()
    {
        UpdateRemainTime();
    }

    private void UpdateRemainTime()
    {
        if (!updatingTime && remaintime >= 0 )
        {
            StartCoroutine(SetRemainTime());
        }
    }

    private IEnumerator SetRemainTime()
    {
        updatingTime = true;
        while (remaintime >= 0)
        {
            int minute = remaintime / 60;
            int sec = remaintime % 60;

            string minStr = (minute < 10) ? ("0" + minute.ToString()) : minute.ToString();
            string secStr = (sec < 10) ? ("0" + sec.ToString()) : sec.ToString();
            timeRemain.text = minStr + ":" + secStr;

            yield return new WaitForSecondsRealtime(1f);
            remaintime--;
        }

        StartCoroutine(GameManager.Instance.LoadNextScene());
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
