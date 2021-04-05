using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class HearingManager : MonoBehaviour
{
    public CityCouncilHost host;

    [Header("Texts & UI")]    
    public GameObject hearingUI;
    public TextMeshProUGUI hearingText;
    public HamiltonSound hearingSound;
    [TextArea]
    public List<string> texts;

    [Header("Lighting")]
    public Light spotLight;
    public Light directionalLight;
    
    [Header("NavMesh")]
    public int walkTime = 5;
    public Transform[] positions = new Transform[3];

    [Header("Characters")]
    private int hearingNum = 0;
    private GameObject person;

    public void StartHearing()
    {
        // Reset All Coroutines
        StopAllCoroutines();

        // Go to podium
        transform.position = positions[0].position;
        GetComponent<NavMeshAgent>().SetDestination(positions[1].position);

        // Set the corresponding character active
        person = transform.GetChild(hearingNum).gameObject;
        person.SetActive(true);

        // Wait for the walk time
        StartCoroutine("ToPodium");
    }

    IEnumerator ToPodium()
    {
        yield return new WaitForSeconds(walkTime);
        SpeechLight(true);
        yield return new WaitForSeconds(1f);
        StartSpeech();
    }

    private void StartSpeech()
    {
        hearingUI.SetActive(true);        
        DisplayNextText();
    }

    private void SpeechLight(bool status)
    {
        directionalLight.intensity = status ? 0.6f : 1.8f;
        spotLight.gameObject.SetActive(status);
    }

    private void DisplayNextText()
    {
        CheckEnd();
        CheckHide();
        if (texts.Count > 0 && !texts[0].Equals("Stop"))
        {
            hearingText.text = texts[0];
            texts.RemoveAt(0);

            hearingText.ForceMeshUpdate();
            int totalVisibleChars = hearingText.textInfo.characterCount;
            //Debug.Log("count: " + totalVisibleChars);
            hearingText.maxVisibleCharacters = 0;
            StartCoroutine(DisplayText(totalVisibleChars));
        }
    }

    private void CheckHide()
    {
        if (texts.Count > 0 && texts[0].Equals("Hide"))
        {
            hearingUI.SetActive(false);
            texts.RemoveAt(0);
        }
    }

    private void CheckEnd()
    {
        if (texts.Count > 0 && texts[0].Equals("End"))
        {
            // End of hearing
            Debug.Log("Hit the End!");
            texts.RemoveAt(0);

            // Switch off the light
            SpeechLight(false);

            // Walk down the podium
            StartCoroutine(LeavePodium());            
        }
    }

    IEnumerator DisplayText(int maxChars)
    {
        int num = 0;
        while (num < maxChars)
        {

            char curChar = hearingText.textInfo.characterInfo[num].character;
            num++;
            if (!char.IsWhiteSpace(curChar))
            {
                if (curChar.Equals('!'))
                {
                    // This is an exclaimation mark

                }
                else if (char.IsLetter(curChar))
                {
                    int idx = curChar % hearingSound.clips.Count;
                    hearingSound.PlayVoice(idx);
                }

                hearingText.maxVisibleCharacters = num;
                //hSound.PlayRandomVoice();

                yield return new WaitForSecondsRealtime(0.1f);
            }

        }

        yield return new WaitForSecondsRealtime(3f);
        DisplayNextText();
    }

    IEnumerator LeavePodium()
    {
        yield return new WaitForSeconds(1);
        hearingUI.SetActive(false);
        GetComponent<NavMeshAgent>().SetDestination(positions[2].position);

        // Deactivate hearing person after walk time
        yield return new WaitForSeconds(walkTime);
        person.SetActive(false);
        hearingNum++; 
        yield return new WaitForSeconds(1);

        if (host && host.gameObject.activeSelf)
        {
            Debug.Log("Noticing the host");
            host.HearingDone();
        }
    }

    /*
    public void StartHearing()
    {
        // Reset All Coroutines
        StopAllCoroutines();

        // Go to podium
        transform.position = positions[0].position;
        GetComponent<NavMeshAgent>().SetDestination(positions[1].position);

        // Set the corresponding character active
        person = transform.GetChild(hearingNum).gameObject;
        person.SetActive(true);

        // Wait for the walk time
        StartCoroutine("ToPodium");
    }

    IEnumerator ToPodium()
    {
        yield return new WaitForSeconds(walkTime);
        StartSpeech();
    }

    private void StartSpeech()
    {
        SpeechLight(true);
        StartCoroutine("MakeSpeech");
    }

    private void SpeechLight(bool status)
    {
        directionalLight.intensity = status ? 0.6f : 1.8f;
        spotLight.gameObject.SetActive(status);
    }

    IEnumerator MakeSpeech()
    {
        yield return new WaitForSeconds(1);

        // Pop up all bubbles at once
        for (int i = 0; i < person.transform.childCount; i++)
        {
            person.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    
    public void FreeDiscussion(int duration)
    {
        speechTime = duration - walkTime;
        SpeechLight(false);
        StartCoroutine("LeaveSpeech");
    }

    IEnumerator LeaveSpeech()
    {
        yield return new WaitForSeconds(speechTime - 1);

        // Walking down the podium & switch light off
        for (int i = 0; i < person.transform.childCount; i++)
        {
            person.transform.GetChild(i).gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(1);

        GetComponent<NavMeshAgent>().SetDestination(positions[2].position);

        // Deactivate hearing person after walk time
        yield return new WaitForSeconds(walkTime);
        person.SetActive(false);
        hearingNum++;
    }
    */
}
