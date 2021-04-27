using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using TMPro;

public class AmendmentManager : MonoBehaviour
{
    [Header("Computer UI")]
    public GameObject computer;
    public GameObject s1,s2,s3,s4a,s4b;

    [Header("Drafting Document")]
    public TextMeshProUGUI draftResult;
    public GameObject clauses;
    private Vector3 clausesRectPos;
    public GameObject s1doc, s2doc, s3doc, s4adoc, s4bdoc;
    private List<GameObject> generatedDocs = new List<GameObject>();

    [Header("Submit Draft")]
    public string amendmentName = "";

    [Header("Hamilton")]
    public GameObject hamilton;
    private bool firstDraft = true;

    [Space]
    public bool testDraft, testReDraft, s1_bool, s2_bool, s3_bool, s4a_bool, s4b_bool;

    public static AmendmentManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        clausesRectPos = clauses.GetComponent<RectTransform>().position;

        hamilton.GetComponent<HamiltonIntro>().DisplayHamiltonUI();
        hamilton.GetComponent<HamiltonTexts>().PlayNext();

        PrepRoomStatus.instance.gameObject.GetComponent<SetForecast>().SetSceneName("Craft Amendments");
    }

    void Update()
    {
        if (testDraft)
        {
            testDraft = false;
            ShowDraft();
        }

        if (testReDraft)
        {
            testReDraft = false;
            ReDraft();
        }
    }

    #region update amendments
    public void UpdateAmendmentS1()
    {
        s1_bool = !s1_bool;
        if (s1_bool)
        {
            s1.SetActive(true);
        }
        else
        {
            s1.SetActive(false);
            s3_bool = false;
            s3.SetActive(false);
            if (!s3_bool && !s2_bool)
            {
                s4a.SetActive(false);
                s4b.SetActive(false);

                s4a_bool = false;
                s4b_bool = false;
            }
        }
    }

    public void UpdateAmendmentS2()
    {
        s2_bool = !s2_bool;
        if (s2_bool)
        {
            s2.SetActive(true);
        }
        else
        {
            s2.SetActive(false);
            if (!s3_bool && !s2_bool)
            {
                s4a.SetActive(false);
                s4b.SetActive(false);

                s4a_bool = false;
                s4b_bool = false;
            }
        }
    }

    public void UpdateAmendmentS3()
    {
        s3_bool = !s3_bool;
        s3.SetActive(s3_bool);
    }

    public void UpdateAmendmentS4a()
    {
        s4a_bool = !s4a_bool;
        s4a.SetActive(s4a_bool);
    }

    public void UpdateAmendmentS4b()
    {
        s4b_bool = !s4b_bool;
        s4b.SetActive(s4b_bool);
    }
    #endregion

    public void ShowDraft()
    {
        int result = 0;
        // Rep Sato
        bool sato_pass = s4b_bool;
        string sato = sato_pass ? "yes" : "no";
        result += sato_pass ? 1 : 0;

        // Rep Lee
        bool lee_pass = s4a_bool;
        string lee = lee_pass ? "yes" : "no";
        result += lee_pass ? 1 : 0;

        // Rep Ke
        bool ke_pass = s1_bool;
        string ke = ke_pass ? "yes" : "no";
        result += ke_pass ? 1 : 0;

        // Rep He
        bool he_pass = !(s1_bool || s2_bool || s3_bool || s4a_bool || s4b_bool);
        string he = he_pass ? "yes" : "no";
        result += he_pass ? 1 : 0;

        // Check the voteing result 
        // Change the result text
        draftResult.text = (result >= 2) ? "Passed!" : "Blocked";

        // Generate the final amendment document
        GenerateDocument();

        // Let Hamilton talk if it's first time
        if (firstDraft)
        {
            firstDraft = false;
            hamilton.GetComponent<HamiltonIntro>().DisplayHamiltonUI();
            hamilton.GetComponent<HamiltonTexts>().PlayNext();
            // Send message to phone
            JArray feedbacks = new JArray(sato, lee, ke, he);
            JObject messageData = new JObject
            {
                {"topic", "feedback" },
                {"message", feedbacks }
            };

            StartCoroutine(FirstDraftFeedback(messageData));
        }
        else
        {
            // Send message to phone
            JArray feedbacks = new JArray(sato, lee, ke, he);
            JObject messageData = new JObject
            {
                {"topic", "feedback" },
                {"message", feedbacks }
            };
            AirConsole.instance.Broadcast(messageData);

            Debug.Log("Sent message is: " + messageData);

            // Hide the drafting background and ui
            computer.SetActive(false);
        }        
    }

    IEnumerator FirstDraftFeedback(JObject messageData)
    {
        yield return new WaitForSeconds(20f);
        
        AirConsole.instance.Broadcast(messageData);

        Debug.Log("Sent message is: " + messageData);

        // Hide the drafting background and ui
        computer.SetActive(false);
    }

    private void GenerateDocument()
    {
        // Normally the vertical interval is 120
        // if its 4sa then the interval is 140
        int count = 1;
        if (s1_bool)
        {
            GameObject s1_doc = Instantiate(s1doc, clauses.transform.position, Quaternion.identity);
            s1_doc.transform.parent = clauses.transform.parent;
            generatedDocs.Add(s1_doc);
            // Set the text
            s1_doc.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Section " + count;
            count++;
            // Update clauses position
            Vector3 clausePos = clauses.GetComponent<RectTransform>().position;
            clausePos.y -= 120;
            clauses.GetComponent<RectTransform>().position = clausePos;
        }

        if (s2_bool)
        {
            GameObject s2_doc = Instantiate(s2doc, clauses.transform.position, Quaternion.identity);
            s2_doc.transform.parent = clauses.transform.parent;
            generatedDocs.Add(s2_doc);
            // Set the text
            s2_doc.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Section " + count;
            count++;
            // Update clauses position
            Vector3 clausePos = clauses.GetComponent<RectTransform>().position;
            clausePos.y -= 120;
            clauses.GetComponent<RectTransform>().position = clausePos;
        }

        if (s3_bool)
        {
            GameObject s3_doc = Instantiate(s3doc, clauses.transform.position, Quaternion.identity);
            s3_doc.transform.parent = clauses.transform.parent;
            generatedDocs.Add(s3_doc);
            // Set the text
            s3_doc.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Section " + count;
            count++;
            // Update clauses position
            Vector3 clausePos = clauses.GetComponent<RectTransform>().position;
            clausePos.y -= 120;
            clauses.GetComponent<RectTransform>().position = clausePos;
        }

        if (s4a_bool)
        {
            GameObject s4a_doc = Instantiate(s4adoc, clauses.transform.position, Quaternion.identity);
            s4a_doc.transform.parent = clauses.transform.parent;
            generatedDocs.Add(s4a_doc);
            // Set the text
            s4a_doc.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Section " + count + "a";
            // Update clauses position
            Vector3 clausePos = clauses.GetComponent<RectTransform>().position;
            clausePos.y -= 140;
            clauses.GetComponent<RectTransform>().position = clausePos;
        }

        if (s4b_bool)
        {
            GameObject s4b_doc = Instantiate(s4bdoc, clauses.transform.position, Quaternion.identity);
            s4b_doc.transform.parent = clauses.transform.parent;
            generatedDocs.Add(s4b_doc);
            // Set the text
            s4b_doc.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Section " + count + "b";
        }
    }

    public void ReDraft()
    {
        if (hamilton.GetComponent<HamiltonTexts>().texts.Count <= 0)
        {
            // show the drafting ground and ui
            computer.SetActive(true);
            clauses.GetComponent<RectTransform>().position = clausesRectPos;

            foreach (GameObject doc in generatedDocs)
            {
                Destroy(doc);
            }
            generatedDocs.Clear();

            amendmentName = "";
        }        
    }

    public void SubmitDraft()
    {
        // Submit
        amendmentName = "";
        amendmentName += s1_bool ? "y" : "n";
        amendmentName += s2_bool ? "y" : "n";
        amendmentName += s3_bool ? "y" : "n";
        amendmentName += s4a_bool ? "y" : "n";
        amendmentName += s4b_bool ? "y" : "n";

        Debug.Log("Amendment Outcome Name is: " + amendmentName);

        GameManager.Instance.StoreFinalBill(amendmentName);
        StartCoroutine(GameManager.Instance.LoadNextScene());
    }
}
