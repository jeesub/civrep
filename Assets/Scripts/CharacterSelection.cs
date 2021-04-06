using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterSelection : MonoBehaviour
{
    [Header("Face")]
    public List<string> faceNames;
    public int faceIdx = 0;
    public GameObject face;
    [Header("Hair")]
    public List<string> hairNames;
    public int hairIdx = 0;
    public GameObject hair;
    [Header("Accessories")]
    public List<string> accNames;
    public int accIdx = 0;
    public GameObject accessory;

    [Space]
    public TextMeshPro repName;

    // Start is called before the first frame update
    void Start()
    {
        repName.text = "";

        ChangeFace();
        ChangeHair();
        ChangeAccessory();
    }
    public void ChangeFace()
    {        
        GameObject new_face = Resources.Load<GameObject>("Characters/eyes/" + faceNames[faceIdx]);
        face.GetComponent<MeshFilter>().mesh = new_face.GetComponent<MeshFilter>().sharedMesh;
        faceIdx = (faceIdx + 1) % faceNames.Count;
    }

    public void ChangeHair()
    {        
        if (hairIdx == hairNames.Count)
        {
            hair.GetComponent<MeshFilter>().mesh = null;
        }
        else
        {
            GameObject new_hair = Resources.Load<GameObject>("Characters/hairs/" + hairNames[hairIdx]);
            hair.GetComponent<MeshFilter>().mesh = new_hair.GetComponent<MeshFilter>().sharedMesh;
        }        
        hairIdx = (hairIdx + 1) % (hairNames.Count+1);
    }

    public void ChangeAccessory()
    {
        
        GameObject new_acc = Resources.Load<GameObject>("Characters/accessories/" + accNames[accIdx]);
        accessory.GetComponent<MeshFilter>().mesh = new_acc.GetComponent<MeshFilter>().sharedMesh;
        accIdx = (accIdx + 1) % accNames.Count;
    }

    public void SetName(string newName)
    {
        if (newName.Length > 0)
        {
            repName.text = "<size=130%>" + newName.Substring(0,1) + "</size>" + newName.Substring(1);
        }        
    }
}
