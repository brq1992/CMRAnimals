using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioNavManager : BaseManager
{
    private AudioSource audioSource;

    public override void InitView()
    {
        base.InitView();
        audioSource = transform.Find("AudioSource").GetComponent<AudioSource>();
    }

    private void Update()
    {

    }

    private void AddAudioSource(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
