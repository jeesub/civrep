using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using UnityEngine.AI;

public class RepController : MonoBehaviour
{
    public Vector3 start_pos;

    public GameObject face, hair, acc;

    void OnEnable()
    {
        start_pos = transform.position;
        SetAppearance();
    }

    public void RepReady()
    {
        GetComponent<NavMeshAgent>().SetDestination(start_pos);
    }

    private void SetAppearance()
    {
        
        string selectionStr = System.IO.File.ReadAllText(Application.dataPath + "/" + gameObject.name + ".json");
        Debug.Log("selection is: " + selectionStr);

        JObject selection = JObject.Parse(selectionStr);
        string faceStr = selection["face"].ToString();
        string hairStr = selection["hair"].ToString();
        string accStr = selection["accessory"].ToString();

        GameObject new_face = Resources.Load<GameObject>("Characters/eyes/" + faceStr);
        face.GetComponent<MeshFilter>().mesh = new_face.GetComponent<MeshFilter>().sharedMesh;

        GameObject new_hair = Resources.Load<GameObject>("Characters/hairs/" + hairStr);
        hair.GetComponent<MeshFilter>().mesh = new_hair.GetComponent<MeshFilter>().sharedMesh;

        GameObject new_acc = Resources.Load<GameObject>("Characters/accessories/" + accStr);
        acc.GetComponent<MeshFilter>().mesh = new_acc.GetComponent<MeshFilter>().sharedMesh;
    }
}
