using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using UnityEngine.AI;

public class RepController : MonoBehaviour
{
    public Vector3 start_pos;

    public GameObject body, face, hair, acc;

    private NavMeshAgent navMesh;

    void OnEnable()
    {
        start_pos = transform.position;
        SetAppearance();

        if (GetComponent<NavMeshAgent>() != null)
        {
            navMesh = GetComponent<NavMeshAgent>();
        }        
    }

    public void RepReady()
    {
        navMesh.SetDestination(start_pos);
    }

    public void GoToDestination(Transform destination)
    {
        navMesh.SetDestination(destination.position);
        transform.LookAt(destination);
    }

    private void SetAppearance()
    {
        
        //string selectionStr = System.IO.File.ReadAllText(Application.dataPath + "/Player.json");
        //Debug.Log("selection is: " + selectionStr);

        JObject selection = GameManager.Instance.repAppearance;
        Debug.Log("selection is: " + selection);
        string bodyStr = selection["body"].ToString();
        string faceStr = selection["face"].ToString();
        string hairStr = selection["hair"].ToString();
        string accStr = selection["accessory"].ToString();

        GameObject new_body = Resources.Load<GameObject>("Characters/body/" + bodyStr);
        body.GetComponent<MeshFilter>().mesh = new_body.GetComponent<MeshFilter>().sharedMesh;

        GameObject new_face = Resources.Load<GameObject>("Characters/eyes/" + faceStr);
        face.GetComponent<MeshFilter>().mesh = new_face.GetComponent<MeshFilter>().sharedMesh;

        GameObject new_hair = Resources.Load<GameObject>("Characters/hairs/" + hairStr);
        hair.GetComponent<MeshFilter>().mesh = new_hair.GetComponent<MeshFilter>().sharedMesh;

        GameObject new_acc = Resources.Load<GameObject>("Characters/accessories/" + accStr);
        acc.GetComponent<MeshFilter>().mesh = new_acc.GetComponent<MeshFilter>().sharedMesh;
    }
}
