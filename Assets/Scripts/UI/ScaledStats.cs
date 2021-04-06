using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScaledStats : MonoBehaviour
{
    public GameObject scalePrefab;
    public TextMeshProUGUI changeText;
    public Color defColor;
    public Color setColor;
    public Color increaseColor;
    public Color decreaseColor;

    public int count = 10;
    public float interval = 18f;

    public int curValue = 3;
    public int prevValue;

    private List<GameObject> scales = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        prevValue = curValue;
        PlacePrefabs();
        SetScale();
    }

    private void PlacePrefabs()
    {
        for (int i = 0; i < count; i++)
        {
            GameObject child = Instantiate(scalePrefab, 
                new Vector3(transform.position.x + i * interval, 
                            transform.position.y, 
                            transform.position.z),
                Quaternion.identity);
            child.transform.parent = transform;
            child.name = "scale" + i;
            child.GetComponent<Image>().color = defColor;
            scales.Add(child);
        }
    }

    public void ChangeScale(int change)
    {
        curValue += change;
        SetScale();
    }

    // Recolor the scales
    private void SetScale()
    {
        for (int i = 0; i < curValue; i++)
        {
            scales[i].GetComponent<Image>().color = setColor;
        }
        if (prevValue < curValue)
        {
            // The value has been increased
            for (int i = prevValue; i < curValue; i++)
            {
                scales[i].GetComponent<Image>().color = increaseColor;
            }
            // Start a coroutine to clear the color
            //StartCoroutine(RefreshScale());

            // update the text to reflect the numerical change
            if (changeText)
            {
                changeText.gameObject.SetActive(true);
                changeText.text = "+" + (curValue - prevValue).ToString();
                changeText.color = increaseColor;
            }                       
        }
        else if (prevValue > curValue)
        {
            // The value has been decreased
            for (int i = curValue; i < prevValue; i++)
            {
                scales[i].GetComponent<Image>().color = decreaseColor;
            }
            // Start a coroutine to clear the color 
            // StartCoroutine(RefreshScale());

            // update the text to reflect the numerical change
            if (changeText)
            {
                changeText.gameObject.SetActive(true);
                changeText.text = "-" + (prevValue - curValue).ToString();
                changeText.color = decreaseColor;
            }            
        }
        else
        {
            if (changeText)
            {
                changeText.gameObject.SetActive(false);
            }            
        }

        prevValue = curValue;
    }

    // Clear the changing color and texts
    IEnumerator RefreshScale()
    {
        yield return new WaitForSecondsRealtime(5f);
        SetScale();
    }

    public void ShowOverallChange(int change)
    {
        SetScale();
        if (change > 0)
        {
            changeText.gameObject.SetActive(true);
            changeText.text = "+" + change.ToString();
            changeText.color = increaseColor;
        }
        else if (change < 0)
        {
            changeText.gameObject.SetActive(true);
            changeText.text = "-" + change.ToString();
            changeText.color = decreaseColor;
        }        
    }

    public void HideOverallChange()
    {
        changeText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
