using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnimationEvent_Handler : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bullet;
    public GameObject hiddenBladeTrigger;

    GameManager_Audio audioManager;
    AudioSource audioSource;

    Animator anim;

    void Awake()
    {
        audioManager = FindObjectOfType<GameManager_Audio>();
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }


    public void ShootProjectile()
    {
        audioSource.PlayOneShot(audioManager.shootSound);
        Instantiate(bullet, firePoint.position, transform.rotation);
    }

    public void OnAttackEnter()
    {
        print("attackenter");
        audioSource.PlayOneShot(audioManager.slashSound, 2);
        hiddenBladeTrigger.SetActive(true);
    }

    public void OnAttackExit()
    {
        print("attackexit");
        hiddenBladeTrigger.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }

    public void FootStepSound()
    {
        //        print("footstep");
        if (anim.GetFloat("SpeedPercent") >= 0.05f)
        {
            audioSource.PlayOneShot(audioManager.footstepSound, .5f);
        }

    }
    public void LandSound()
    {
        audioSource.PlayOneShot(audioManager.landSound, 0.5f);
    }

}
