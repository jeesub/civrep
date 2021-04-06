using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestBodyChanging : MonoBehaviour
{
    public bool changeFace = false;
    public List<string> faceNames;
    public int faceIdx;

    public bool changeHair = false;
    public List<string> hairNames;
    public int hairIdx;

    public bool changAccessories = false;
    public List<string> accNames;
    public int accIdx;

    public bool changeName = false;
    public string newName = "";

    public GameObject face, hair, accessory;
    public TextMeshPro repName;

    // Start is called before the first frame update
    void Start()
    {
        
        repName.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (changeFace)
        {
            changeFace = false;
            ChangeFace();
        }
        if (changeHair)
        {
            changeHair = false;
            ChangeHair();
        }
        if (changAccessories)
        {
            changAccessories = false;
            ChangeAccessory();
        }
        if (changeName && newName.Length > 0)
        {
            changeName = false;
            ChangeName();
        }
    }

    public void ChangeFace()
    {
        faceIdx = (faceIdx + 1) % faceNames.Count;
        GameObject new_face = Resources.Load<GameObject>("Characters/eyes/" + faceNames[faceIdx]);
        face.GetComponent<MeshFilter>().mesh = new_face.GetComponent<MeshFilter>().sharedMesh;
    }

    public void ChangeHair()
    {
        hairIdx = (hairIdx + 1) % hairNames.Count;
        GameObject new_hair = Resources.Load<GameObject>("Characters/hairs/" + hairNames[hairIdx]);
        hair.GetComponent<MeshFilter>().mesh = new_hair.GetComponent<MeshFilter>().sharedMesh;
    }

    public void ChangeAccessory()
    {
        accIdx = (accIdx + 1) % accNames.Count;
        GameObject new_acc = Resources.Load<GameObject>("Characters/accessories/" + accNames[accIdx]);
        accessory.GetComponent<MeshFilter>().mesh = new_acc.GetComponent<MeshFilter>().sharedMesh;
    }

    public void ChangeName()
    {
        repName.text = "<size=130%>" + newName.Substring(0, 1) + "</size>" + newName.Substring(1);
    }
}
