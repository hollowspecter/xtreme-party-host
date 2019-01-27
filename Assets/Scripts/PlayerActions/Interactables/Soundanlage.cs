using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soundanlage : GeneralInteractable
{
    public AudioClip[] musicPieces;
    public AudioSource audioSource;

    private int currentMusic = 0;

    protected override void Start()
    {
        base.Start();
        PlayNextSong();
    }

    protected virtual void PlayNextSong()
    {
        audioSource.clip = musicPieces[(currentMusic++) % musicPieces.Length];
        audioSource.Play();
    }

    protected override void AlternativeFinish()
    {
        base.AlternativeFinish();

        PlayNextSong();
    }

}
