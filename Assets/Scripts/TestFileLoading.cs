using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestFileLoading : MonoBehaviour
{
    public string fileName;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("result string is: " + fileName);
        Sprite resultSprite = Resources.Load<Sprite>("Sprites/VoteResults/" + fileName) as Sprite;
        Debug.Log(resultSprite.rect);
        GetComponent<Image>().sprite = resultSprite;
        GetComponent<Image>().preserveAspect = true;
    }
}
