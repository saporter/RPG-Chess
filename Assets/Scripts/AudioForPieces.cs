using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]         //To cover for cases when a piece doesn't already have an AudioSource

public class AudioForPieces : MonoBehaviour {

    private AudioSource audioSource;

    public AudioClip onClickSE;
    public AudioClip onMoveSE;
    public AudioClip onCaptureSE;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void SE_PickUp()             // Plays the audio for picking up the piece
    {
        audioSource.clip = onClickSE;
        audioSource.Play();
        //Debug.Log("SE_PickUp");
        

    }

    public void SE_Move()               // Plays the audio for moving the piece, will not play this audio if the capture audio is to be played as the last sound effect will be "onCaptureSE"
    {
        if (audioSource.clip != onCaptureSE)
        {
            audioSource.clip = onMoveSE;
            audioSource.Play();
            //Debug.Log("SE_Move");
        }

    }

    public void SE_Capture()            // Plays the audio for capturing
    {
        audioSource.clip = onCaptureSE;
        audioSource.Play();
        //Debug.Log("SE_Capture");
    }

}
