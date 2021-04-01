using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaledStats : MonoBehaviour
{
    public GameObject scalePrefab;
    public Color defColor;
    public Color setColor;

    public int count = 10;
    public float interval = 18f;

    public int value = 3;

    private List<GameObject> scales = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        PlacePrefabs();
        SetScale(value);
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

    private void SetScale(int val)
    {
        for (int i = 0; i < val; i++)
        {
            scales[i].GetComponent<Image>().color = setColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
