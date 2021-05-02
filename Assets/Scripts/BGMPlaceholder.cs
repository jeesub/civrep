using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMPlaceholder : MonoBehaviour
{
    public List<AudioClip> bgms = new List<AudioClip>();

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SceneManager.activeSceneChanged += ChangeBGM;

        //audioSource.Stop();
        audioSource.clip = bgms[0];
        audioSource.Play();
        audioSource.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ChangeBGM(Scene current, Scene next)
    {
        if (next.buildIndex == 3)
        {
            //audioSource.Stop();
            Debug.Log("Changing clip");
            audioSource.clip = bgms[1];
            audioSource.Play();
            audioSource.loop = true;
        }
        else if (next.buildIndex == 5)
        {
            Debug.Log("Changing clip");
            audioSource.clip = bgms[0];
            audioSource.Play();
            audioSource.loop = true;
        }
    }
}
