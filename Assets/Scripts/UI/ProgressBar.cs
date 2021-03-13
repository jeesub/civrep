using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Progress
{
    public float val;
    public Color color;
}

public class ProgressBar : MonoBehaviour
{
    public List<Progress> progressList;
    public GameObject progressBarPrefab;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = progressList.Count-1; i >= 0; i--)
        {
            // Instantiate as child object
            GameObject theBar = Instantiate(progressBarPrefab, new Vector3(transform.position.x, 
                    transform.position.y, transform.position.z), Quaternion.identity);
            theBar.transform.parent = transform;
            theBar.gameObject.name = "subProgress" + i.ToString();

            // Adjust the scale
            theBar.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 0f);


            // Set the width of the sub progress bar
            Vector2 sizeDelta = this.GetComponent<RectTransform>().sizeDelta;
            Debug.Log("sizeDelta is: " + sizeDelta);
            float width = sizeDelta.x * progressList[i].val / 100.0f;
            Debug.Log("Width is: " + width);
            theBar.GetComponent<RectTransform>().sizeDelta = new Vector2(width, sizeDelta.y);

            // Set the color of the sub progress bar
            theBar.GetComponent<Image>().color = progressList[i].color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
