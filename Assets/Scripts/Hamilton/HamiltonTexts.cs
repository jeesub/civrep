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
    public AmendmentOutcomes outcomes;

    private void Awake()
    {
        if (hUI == null) hUI = GameObject.Find("Hamilton UI");
        if (hSound == null) hSound = transform.GetChild(0).GetComponent<HamiltonSound>();
    }

    public void PlayNext()
    {
        //Debug.Log("Play Next");
        RemoveStopAtFirst();
        DisplayNextText();
    }

    private void DisplayNextText()
    {
        //Debug.Log("Display Next Text");
        bool hasEnd = CheckEnd();
        bool hasHide = CheckHide();
        if (!hasEnd && !hasHide && texts.Count > 0 && !texts[0].Equals("Stop"))
        {
            hText.text = texts[0];
            texts.RemoveAt(0);
            hText.ForceMeshUpdate();
            int totalVisibleChars = hText.textInfo.characterCount;
            //Debug.Log("count: " + totalVisibleChars);
            hText.maxVisibleCharacters = 0;
            StartCoroutine(DisplayText(totalVisibleChars, hText.textInfo.characterInfo));
        }

        // Notify city council host
        else if (host!=null && host.gameObject.activeSelf)
        {
            host.HamiltonDone();
        }
    }

    private bool CheckEnd()
    {
        //Debug.Log("Check End");
        if (texts.Count > 0 && texts[0].Equals("End"))
        {
            texts.RemoveAt(0);
            Debug.Log("Going to next scene");
            StartCoroutine(GameManager.Instance.LoadNextScene());
            return true;
        }
        return false;
    }

    private bool CheckHide()
    {
        //Debug.Log("Check Hide");
        if (texts.Count > 0 && texts[0].Equals("Hide"))
        {
            texts.RemoveAt(0);
            // Notify outcome display
            if (outcomes != null && outcomes.gameObject.activeSelf)
            {                
                if (outcomes.DisplayRepChanges()) return true;
            }

            hUI.SetActive(false);
            
            if (panel != null)
            {
                panel.SetActive(false);
            }
            if (hamiltonCapture != null)
            {
                hamiltonCapture.SetActive(false);
            }
            return true;
        }
        return false;
    }

    private void RemoveStopAtFirst()
    {
        if (texts.Count > 0 && texts[0].Equals("Stop"))
        {
            texts.RemoveAt(0);
            hChoices.HideChoices();
        }
    }

    IEnumerator DisplayText(int maxChars, TMP_CharacterInfo[] curInfo)
    {
        int num = 0;
        while (num < maxChars)
        {
            
            char curChar = curInfo[num].character;
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
                else if (char.IsLetter(curChar))
                {

                    int idx = curChar % hSound.clips.Count;
                    if (hSound != null) hSound.PlayVoice(idx);
                }
                hText.maxVisibleCharacters = num;
                //hSound.PlayRandomVoice();

                yield return new WaitForSecondsRealtime(0.015f);
            }
        }

        yield return new WaitForSecondsRealtime(4f);
        DisplayNextText();
    }
}
