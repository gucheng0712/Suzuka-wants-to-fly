using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_Audio : MonoBehaviour
{
    public AudioClip footstepSound;
    public AudioClip landSound;

    public AudioClip battle_bgm;
    public AudioClip noraml_bgm;

    public AudioClip slashSound;
    public AudioClip shootSound;
    public AudioClip dashSound;

    public AudioClip weaponSwitchSound;


    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ChangeBGM(AudioClip _bgm)
    {

        audioSource.Stop();
        audioSource.clip = _bgm;
        audioSource.Play();
    }
}
