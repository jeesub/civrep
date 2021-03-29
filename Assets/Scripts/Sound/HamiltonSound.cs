using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamiltonSound : MonoBehaviour
{
    public List<AudioClip> clips;

    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayVoice(int idx)
    {
        if (!source.isPlaying && clips.Count > 0)
        {
            source.clip = clips[idx];
            source.Play();
        }
    }

    public void PlayRandomVoice()
    {
        if (!source.isPlaying && clips.Count > 0)
        {
            System.Random rnd = new System.Random();
            int idx = rnd.Next(clips.Count);

            source.clip = clips[idx];
            source.Play();
        }
    }
}
