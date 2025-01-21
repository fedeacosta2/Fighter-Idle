using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{   
    
    public AudioSource audio_source;
    public AudioClip sfx1, sfx2, sfx3;

    private void Start()
    {
        audio_source.clip = sfx1;
        audio_source.Play();
    }

    public void ButtonPlaySfx()
    {
        audio_source.clip = sfx2;
        audio_source.Play();
    }
    public void ButtonOptionsSfx()
    {
        audio_source.clip = sfx2;
        audio_source.Play();
    }
    public void ButtonExitSfx()
    {
        audio_source.clip = sfx2;
        audio_source.Play();
    }
}
