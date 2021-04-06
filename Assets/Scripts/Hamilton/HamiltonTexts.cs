using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class HamiltonTexts : MonoBehaviour
{
    public GameObject hUI;
    public GameObject panel;
    public GameObject hamiltonCapture;
    public TextMeshProUGUI hText;
    public GameObject choices;

    public HamiltonSound hSound;
    public HamiltonChoices hChoices;

    public bool isTalking = false;

    [TextArea]
    public List<string> texts = new List<string>();

    public CityCouncilHost host;

    private void Start()
    {
        hUI = GameObject.Find("Hamilton UI");
    }

    public void PlayNext()
    {
        RemoveStopAtFirst();
        DisplayNextText();
    }

    private void DisplayNextText()
    {
        CheckEnd();
        CheckHide();
        if (texts.Count > 0 && !texts[0].Equals("Stop"))
        {
            hText.text = texts[0];
            texts.RemoveAt(0);

            hText.ForceMeshUpdate();
            int totalVisibleChars = hText.textInfo.characterCount;
            //Debug.Log("count: " + totalVisibleChars);
            hText.maxVisibleCharacters = 0;
            StartCoroutine(DisplayText(totalVisibleChars));
        }

        // Notify city council host
        else if (host!=null && host.gameObject.activeSelf)
        {
            host.HamiltonDone();
        }
    }

    private void CheckEnd()
    {
        if (texts.Count > 0 && texts[0].Equals("End"))
        {
            texts.RemoveAt(0);
            Debug.Log("Going to next scene");
            StartCoroutine(GameManager.Instance.LoadNextScene());
        }
    }

    private void CheckHide()
    {
        if (texts.Count > 0 && texts[0].Equals("Hide"))
        {
            hUI.SetActive(false);
            texts.RemoveAt(0);
            if (panel != null)
            {
                panel.SetActive(false);
            }
            if (hamiltonCapture != null)
            {
                hamiltonCapture.SetActive(false);
            }
        }
    }

    private void RemoveStopAtFirst()
    {
        if (texts.Count > 0 && texts[0].Equals("Stop"))
        {
            texts.RemoveAt(0);
            hChoices.HideChoices();
        }
    }

    IEnumerator DisplayText(int maxChars)
    {
        int num = 0;
        while (num < maxChars)
        {
            
            char curChar = hText.textInfo.characterInfo[num].character;
            num++;
            if (!char.IsWhiteSpace(curChar))
            {
                if (curChar.Equals('?'))
                {
                    // This is a question
                    if (SceneManager.GetActiveScene().buildIndex != 3)
                    {
                        hChoices.ShowChoices();
                    }                    
                }
                else if (curChar.Equals('!'))
                {
                    // This is an exclaimation mark
                    
                }
                else if (char.IsLetter(curChar))
                {
                    int idx = curChar % hSound.clips.Count;
                    hSound.PlayVoice(idx);
                }

                hText.maxVisibleCharacters = num;
                //hSound.PlayRandomVoice();

                yield return new WaitForSecondsRealtime(0.07f);
            }
            
        }

        yield return new WaitForSecondsRealtime(1f);
        DisplayNextText();
    }
}
