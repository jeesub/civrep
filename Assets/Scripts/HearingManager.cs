using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HearingManager : MonoBehaviour
{    
    public Light spotLight;
    public Light directionalLight;

    public int walkTime = 5;
    public int speechTime = 110;

    public Transform[] positions = new Transform[3];

    private int hearingNum = 0;
    private GameObject person;

    public void StartHearing(int duration)
    {
        // Reset All Coroutines
        StopAllCoroutines();

        // Reset the speechtime
        speechTime = duration - walkTime;

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
}
