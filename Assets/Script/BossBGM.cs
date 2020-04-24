using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBGM : MonoBehaviour
{
    public bool isBossBGM;
    public bool isPrinceBGM;

    public AudioClip Boss_Intro;
    public AudioClip Boss_Main;
    public AudioClip Prince_Intro;
    public AudioClip Prince_Main;

    AudioSource AudioPlayer;
    // Start is called before the first frame update
    void Start()
    {
        AudioPlayer = GetComponent<AudioSource>();
        if (isBossBGM)
        {AudioPlayer.clip = Boss_Intro;}

        else if (isPrinceBGM)
        { AudioPlayer.clip = Prince_Intro; }
        AudioPlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (isBossBGM && AudioPlayer.isPlaying == false)
        { 
            AudioPlayer.clip = Boss_Main;
            AudioPlayer.Play();
            AudioPlayer.loop = true;
        }

        if (isBossBGM && AudioPlayer.isPlaying == false)
        { 
            AudioPlayer.clip = Boss_Main;
            AudioPlayer.Play();
            AudioPlayer.loop = true;
        }
    }
}
