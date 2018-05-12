using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animation : MonoBehaviour
{

    Player playerScript;
    float lerpTime = 0.1f;

    // Use this for initialization
    void Awake()
    {
        playerScript = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        playerScript.anim.SetBool("Grounded", playerScript.IsGrounded());
        playerScript.anim.SetBool("Swinging", playerScript.isSwinging);
        playerScript.anim.SetBool("Bursting", playerScript.isBursting);
        playerScript.anim.SetBool("WallWalking", playerScript.isWallWalking);
        playerScript.anim.SetBool("Falling", playerScript.isFalling);
    }


    public void Anim_Movement(float _inputValue)
    {

        playerScript.anim.SetFloat("SpeedPercent", _inputValue * ((playerScript.isRunning) ? 1.0f : 0.5f), lerpTime, Time.deltaTime);

    }
    public void Anim_Jump(bool jump)
    {
        playerScript.anim.SetBool("Jumping", jump);
    }
}
