using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyNotes : MonoBehaviour
{
    public GameObject cityCanvas;

    // Start is called before the first frame update
    void Start()
    {
        cityCanvas = GameObject.Find("Canvas-City");

        PrepRoomStatus status = cityCanvas.GetComponent<PrepRoomStatus>();

        bool letterYes = status.repStatusItem.letter == 0;
        bool interviewYes = status.repStatusItem.map == 0;
        if (letterYes)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
        }

        if (interviewYes)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(true);
        }
    }
}
