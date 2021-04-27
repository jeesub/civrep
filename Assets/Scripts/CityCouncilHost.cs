using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using NDream.AirConsole;
using TMPro;
using UnityEngine.UI;

public enum EventType
{
    Hamilton,
    Hearing,
    Discussion,
    Voting,
    Result
}

public enum ControllerPage
{
    idle,
    inventory,
    vote
}

[System.Serializable]
public class CityCouncilEvent
{
    public EventType eventType;
    public bool needTimer;
    public int duration;
    public bool needName;
    public string eventName;
    public ControllerPage controllerPage;
}

public class CityCouncilHost : MonoBehaviour
{
    [Header("Event Sequence")]
    // 1. Hamilton introduction to the process
    // 2. Jamie's hearing
    // 3. Hamilton talk a little to lead the discussion
    // 4. 10 min of discussion time
    // 5. Voting started
    public List<CityCouncilEvent> sequence = new List<CityCouncilEvent>();
    private CityCouncilEvent curEvent;
    private SetForecast forecast;

    [Header("Hamilton")]
    public GameObject hamilton;
    public GameObject hCanvas;

    [Header("Hearing")]
    public GameObject hearingPerson;
    public GameObject hearingCanvas;

    [Header("Discussion")]
    public bool jumpDiscussion = false;

    [Header("Voting")]
    public bool toNextVoting = false;
    private bool lastAmend;
    public GameObject votePanel;
    public TextMeshProUGUI numAmend;
    public TextMeshProUGUI topicAmend;
    public TextMeshProUGUI descriptionAmend;
    public List<GameObject> repPhotos = new List<GameObject>();
    public GameObject yea, nay;

    public List<int> yeaReps = new List<int>();
    public List<int> nayReps = new List<int>();
    [TextArea]
    public List<string> explainImapctTexts = new List<string>();
    private List<int> votedReps = new List<int>();

    [Header("Result")]
    [TextArea]
    public List<string> finalResultTexts = new List<string>();
    public Image finalResult;
    public string votingResults;

    private string[] orderNum = new string[] { "first", "second", "third", "fourth" };
    private AmendmentHost amendmentHost;

    void Start()
    {
        amendmentHost = GetComponent<AmendmentHost>();
        //votePanel.SetActive(false);
        //finalResult.gameObject.SetActive(false);
        //votingResults = "";

        forecast = GameObject.Find("Canvas-City").GetComponent<SetForecast>();
        forecast.hearing = this;

        NextEventInSequence();
    }

    void Update()
    {
        if (jumpDiscussion)
        {
            jumpDiscussion = false;
            JumpDiscussion();
        }
        if (toNextVoting)
        {
            toNextVoting = false;
            NextVoting();
        }
    }

    #region utilities
    public void EventTimeUp()
    {
        if (curEvent.eventType == EventType.Voting)
        {
            Debug.Log("Show result!");
            VoteDone();
        }
        else
        {
            NextEventInSequence();
        }
    }

    public void NextEventInSequence()
    {
        Debug.Log("Next Sequence Please!");
        Debug.Log("sequence has " + sequence.Count + " events to go");
        if (sequence.Count > 0)
        {
            curEvent = sequence[0];
            sequence.RemoveAt(0);

            
            if (curEvent.needTimer)
            {
                // Set the timer
                forecast.ResetRemainTime();
                forecast.SetRemainTime(curEvent.duration);
            }
            else
            {
                // Hide the timer text
                forecast.ResetRemainTime();
                forecast.SetRemainTime(-1);
            }
            

            if (curEvent.needName)
            {
                // Set the upcoming event name on UI
                forecast.SetSceneName(curEvent.eventName);
            }
            else
            {
                forecast.HideSceneName();
            }

            // Broadcast message to all controllers
            NoticeController(System.Enum.GetName(typeof(ControllerPage), curEvent.controllerPage));

            // Actually make the event on Unity
            HostEvent(curEvent.eventType, curEvent.duration);
        }
        else
        {
            // all sequences are out go to next scene -> drafting amendments
            StartCoroutine(GameManager.Instance.LoadNextScene());
        }
    }

    private void NoticeController(string status)
    {
        JObject messageData = new JObject
                {
                    {"topic", "screen" },
                    {"message", status }
                };
        AirConsole.instance.Broadcast(messageData);
    }

    public void HostEvent(EventType eventType, int duration)
    {
        switch (eventType)
        {
            case EventType.Hamilton:
                SummonHamilton();
                break;
            case EventType.Hearing:
                HostHearing();
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
    #endregion

    #region hamilton
    private void SummonHamilton()
    {
        hCanvas.SetActive(true);
        hamilton.GetComponent<HamiltonIntro>().DisplayHamiltonUI();
        hamilton.GetComponent<HamiltonTexts>().host = this;
        hamilton.GetComponent<HamiltonTexts>().PlayNext();
    }

    public void HamiltonDone()
    {
        hCanvas.SetActive(false);
        // Don't go to next sequence if hamilton is explain the vote impact
        if (curEvent.eventType != EventType.Voting)
        {
            NextEventInSequence();
        }        
    }
    #endregion

    #region hearing
    private void HostHearing()
    {
        // Host the hearing
        // hearingCanvas.SetActive(true);
        hearingPerson.GetComponent<HearingManager>().StartHearing();
    }

    public void HearingDone()
    {
        Debug.Log("Host knows the hearing is done");
        hearingCanvas.SetActive(false);
        NextEventInSequence();
    }
    #endregion

    #region discussion
    private void HostDiscussion(int duration)
    {
        StartCoroutine(FreeDiscussion(duration));
    }

    IEnumerator FreeDiscussion(int duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        NextEventInSequence();
    }

    private void JumpDiscussion()
    {
        StopCoroutine("FreeDiscussion");
        NextEventInSequence();
    }

    #endregion

    #region voting

    private void ResetYeaNay()
    {
        Vector2 naySize = nay.GetComponent<RectTransform>().sizeDelta;
        Vector2 yeaSize = yea.GetComponent<RectTransform>().sizeDelta;

        naySize.y = 160;
        yeaSize.y = 140;

        nay.GetComponent<RectTransform>().sizeDelta = naySize;
        yea.GetComponent<RectTransform>().sizeDelta = yeaSize;
    }


    private void UpdateYeaNay()
    {
        Vector2 naySize = nay.GetComponent<RectTransform>().sizeDelta;
        Vector2 yeaSize = yea.GetComponent<RectTransform>().sizeDelta;

        naySize.y = 160 + 20 * nayReps.Count;
        yeaSize.y = 140 + 20 * yeaReps.Count;

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

    // TODO: Summon Hamilton. add timer
    private void HostVote()
    {
        // Enable Voting UI
        votePanel.SetActive(true);

        // Disable all repPhotos
        foreach (GameObject rep in repPhotos)
        {
            rep.SetActive(false);
        }

        // Clear the voted rep list
        votedReps.Clear();

        // Set up Hamilton UI
        (string topicText, string descriptionText, int num) = amendmentHost.GetCurAmendment();
        topicAmend.text = "Should We " + topicText + "?";
        descriptionAmend.text = descriptionText;
        numAmend.text = "The " + orderNum[num] + " amendment being voted on is...";
    }

    public void CheckAllVoted(int repIdx)
    {
        if (!votedReps.Contains(repIdx))
        {
            votedReps.Add(repIdx);
            if (votedReps.Count == RepManager.instance.maxPlayer)
            {
                // All players have voted
                StartCoroutine("AllRepsVoted");
            }
        }
    }

    IEnumerator AllRepsVoted()
    {
        yield return new WaitForSeconds(2f);
        VoteDone();
    }

    public void VoteDone()
    {
        forecast.ResetRemainTime();

        Debug.Log("Vote is done!");
        // Notice Controller
        NoticeController("idle");

        // Default time to display the impact
        int displayTime = 60;

        // Calculate the result
        bool result = yeaReps.Count > nayReps.Count;

        // Update voting results
        votingResults += result ? "y" : "n";

        // Hide the vote panel, reset elements
        ResetYeaNay();
        votePanel.SetActive(false);        

        // Reflect the result impact on the city UI
        (List<string> impactTexts, bool isLastAmend) = amendmentHost.ProcessVoteResult(result);
        lastAmend = isLastAmend;

        // Setup the next event
        if (!isLastAmend)
        { 
            // There's more amendment coming
            Debug.Log("This is not the last amendment");
            sequence.Add(curEvent);
        }

        // Set the Hamilton texts
        foreach (string impactText in impactTexts)
        {
            hamilton.GetComponent<HamiltonTexts>().texts.Add(impactText);
        }

        // If the first amendment, Let hamilton explain the changes
        if (amendmentHost.numAmend == 1)
        {
            displayTime += 60;
            foreach(string explainText in explainImapctTexts)
            {
                hamilton.GetComponent<HamiltonTexts>().texts.Add(explainText);
            }
        }

        // Make Hamilton Talk now
        Debug.Log("Summonning Hamilton");
        Debug.Log("Hamilton now has " + hamilton.GetComponent<HamiltonTexts>().texts.Count
            + "pieces of text to read");
        SummonHamilton();
        hamilton.GetComponent<HamiltonTexts>().panel.SetActive(false);


        // Start timing the result display if it's not the last amendment
        Debug.Log("Start to time: " + displayTime + "seconds");
        StartCoroutine(DisplayImpact(displayTime, isLastAmend));        
    }

    private void NextVoting()
    {
        StopCoroutine("DisplayImpact");
        PrepRoomStatus.instance.HideCityImpactChange();
        if (!lastAmend)
        {
            NextEventInSequence();
        }
        else
        {
            HostResult();
        }
    }

    IEnumerator DisplayImpact(int displayTime, bool isLastAmend)
    {
        yield return new WaitForSecondsRealtime(displayTime);
        PrepRoomStatus.instance.HideCityImpactChange();
        if (!isLastAmend)
        {
            NextEventInSequence();
        }
        else
        {
            HostResult();
        }        
    }
    #endregion

    #region result
    private void HostResult()
    {
        Debug.Log("Hosting result here!");
        foreach (string impactText in finalResultTexts)
        {
            hamilton.GetComponent<HamiltonTexts>().texts.Add(impactText);
        }

        // Make Hamilton Talk now
        Debug.Log("Summonning Hamilton");
        Debug.Log("Hamilton now has " + hamilton.GetComponent<HamiltonTexts>().texts.Count
            + "pieces of text to read");
        SummonHamilton();
        hamilton.GetComponent<HamiltonTexts>().panel.SetActive(false);

        // Show overall impact on the city
        PrepRoomStatus.instance.ShowOverallImpact();

        // Bring up the summary sprite
        finalResult.gameObject.SetActive(true);
        Debug.Log("result string is: " + votingResults);
        Sprite resultSprite = Resources.Load<Sprite>("Sprites/VoteResults/" + votingResults);
        Debug.Log(resultSprite.rect);
        finalResult.sprite = resultSprite;
        finalResult.preserveAspect = true;
    }

    private void ShowResult()
    {
        // After all amendments have been voted on, 
        // reflect the result on PC and city metric
    }
    #endregion
}
