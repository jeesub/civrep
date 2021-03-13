using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityCouncilHost : MonoBehaviour
{
    public GameObject hearingPerson;

    public GameObject votePanel;
    public List<GameObject> repPhotos = new List<GameObject>();

    public List<int> yeaReps = new List<int>();
    public List<int> nayReps = new List<int>();
    public GameObject yea, nay;

    private void HostHearing(int duration)
    {
        hearingPerson.GetComponent<HearingManager>().StartHearing(duration);
    }

    private void HostDiscussion(int duration)
    {
        hearingPerson.GetComponent<HearingManager>().FreeDiscussion(duration);
    }

    private void UpdateYeaNay()
    {
        Vector2 naySize = nay.GetComponent<RectTransform>().sizeDelta;
        Vector2 yeaSize = yea.GetComponent<RectTransform>().sizeDelta;

        naySize.y = 140 + 20 * nayReps.Count;
        yeaSize.y = 120 + 20 * yeaReps.Count;

        nay.GetComponent<RectTransform>().sizeDelta = naySize;
        yea.GetComponent<RectTransform>().sizeDelta = yeaSize;
    }

    public void VoteYea(int repIdx)
    {
        if (repPhotos.Count > 0)
        {
            GameObject rep = repPhotos[repIdx];
            if (!rep.activeSelf) rep.SetActive(true);

            Vector3 position = rep.GetComponent<RectTransform>().localPosition;
            if (position.x > 0)
            {
                position.x *= -1;
                rep.GetComponent<RectTransform>().localPosition = position;
            }

            if (!yeaReps.Contains(repIdx))
            {
                Debug.Log("RepIdx: " + repIdx + " already in yeaReps");
                if (nayReps.Contains(repIdx)) nayReps.Remove(repIdx);
                yeaReps.Add(repIdx);
            }

            UpdateYeaNay();
        }
    }

    public void VoteNay(int repIdx)
    {
        if (repPhotos.Count > 0)
        {
            GameObject rep = repPhotos[repIdx];
            if (!rep.activeSelf) rep.SetActive(true);

            Vector3 position = rep.GetComponent<RectTransform>().localPosition;
            if (position.x < 0)
            {
                position.x *= -1;
                rep.GetComponent<RectTransform>().localPosition = position;
            }

            if (!nayReps.Contains(repIdx))
            {
                Debug.Log("RepIdx: " + repIdx + " already in nayreps");
                if (yeaReps.Contains(repIdx)) yeaReps.Remove(repIdx);
                nayReps.Add(repIdx);
            }

            UpdateYeaNay();
        }        
    }

    private void HostVote()
    {
        // Instantiate the voting panel
        GameObject canvas = GameObject.Find("Canvas");

        GameObject panel = Instantiate(votePanel, canvas.transform.position, Quaternion.identity);
        panel.transform.parent = canvas.transform;
        //panel.GetComponent<RectTransform>().position = new Vector3(0, 250, 0);

        // Set Yea and Nay
        nay = panel.transform.GetChild(1).GetChild(1).gameObject;
        yea = panel.transform.GetChild(1).GetChild(0).gameObject;        

        // Set the repPhotos
        Transform repRoot = panel.transform.GetChild(1).GetChild(2);
        for (int i = 0; i < repRoot.childCount; i++)
        {
            repPhotos.Add(repRoot.GetChild(i).gameObject);
        }
    }

    private void HostResult()
    {

    }

    public void HostEvent(EventType eventType, int duration)
    {
        switch (eventType)
        {
            case EventType.Hearing:
                HostHearing(duration);
                break;
            case EventType.Discussion:
                HostDiscussion(duration);
                break;
            case EventType.Voting:
                HostVote();
                break;
            case EventType.Result:
                HostResult();
                break;
            default:
                break;
        }
    }
}
