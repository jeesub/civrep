using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityCouncilHost : MonoBehaviour
{
    public GameObject hearingPerson;

    public GameObject votePanel;
    public List<GameObject> repPhotos = new List<GameObject>();

    private List<int> yeaReps = new List<int>();
    private List<int> nayReps = new List<int>();
    private GameObject yea, nay;

    private void HostHearing(int duration)
    {
        hearingPerson.GetComponent<HearingManager>().StartHearing(duration);
    }

    private void HostDiscussion()
    {
        
    }

    private void UpdateYeaNay()
    {
        Vector2 naySize = nay.GetComponent<RectTransform>().sizeDelta;
        Vector2 yeaSize = yea.GetComponent<RectTransform>().sizeDelta;

        naySize.y = 140 + 20 * nayReps.Count;
        yeaSize.y = 140 + 20 * yeaReps.Count;
    }

    public void VoteYea(int repIdx)
    {
        if (repPhotos.Count > 0)
        {
            GameObject rep = repPhotos[repIdx];
            if (!rep.activeSelf) rep.SetActive(true);

            Vector2 sizeDelta = rep.GetComponent<RectTransform>().sizeDelta;
            if (sizeDelta.x > 0)
            {
                sizeDelta.x *= -1;
                rep.GetComponent<RectTransform>().sizeDelta = sizeDelta;
            }

            if (!yeaReps.Contains(repIdx))
            {
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

            Vector2 sizeDelta = rep.GetComponent<RectTransform>().sizeDelta;
            if (sizeDelta.x < 0)
            {
                sizeDelta.x *= -1;
                rep.GetComponent<RectTransform>().sizeDelta = sizeDelta;
            }

            if (!nayReps.Contains(repIdx))
            {
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
        yea = panel.transform.GetChild(1).GetChild(2).gameObject;        

        // Set the repPhotos
        Transform repRoot = panel.transform.GetChild(1).GetChild(0);
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
                HostDiscussion();
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
