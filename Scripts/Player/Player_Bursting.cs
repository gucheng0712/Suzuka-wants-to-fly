using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Player_Bursting : MonoBehaviour
{
    public GameObject burstParticle_L;
    public GameObject burstParticle_R;

    [SerializeField] float forwardBurstForce = 20f;
    [SerializeField] float horizontalBurstForce = 10f;

    Player playerScript;
    void Awake()
    {
        playerScript = GetComponent<Player>();
    }

    void Update()
    {
        BurstInput();
        ContrainVelocity();
    }

    void BurstInput()
    {
        if (GameManager.BURST_START_INPUT)
        {
            if (!playerScript.IsGrounded() && !playerScript.isSwinging && !playerScript.isWallWalking && !playerScript.isInAttackMode)
            {
                GameManager.state = GameManager.BURST_STATE;
                EnableBurst();
            }
        }
        else if (GameManager.BURSTING_INPUT && GameManager.state == GameManager.BURST_STATE)
        {
            if (playerScript.isBursting)
            {
                Bursting();
            }
        }
        else if (GameManager.BURST_END_INPUT && GameManager.state == GameManager.BURST_STATE)
        {
            DisableBurst();
        }
        else
        {
            DisableBurst();// double check to disable burst
        }
    }

    void EnableBurst()
    {
        playerScript.isBursting = true;
        burstParticle_L.SetActive(true);
        burstParticle_R.SetActive(true);
        CameraShake.isshakeCamera = true;
    }
    void DisableBurst()
    {
        playerScript.isBursting = false;
        burstParticle_L.SetActive(false);
        burstParticle_R.SetActive(false);
        CameraShake.isshakeCamera = false;
    }
    void Bursting()
    {
        float input_Horizontal = GameManager.MOVE_INPUT.x;

        // add force direction 
        Vector3 forceDir = transform.forward * forwardBurstForce + playerScript.mainCam.transform.right * input_Horizontal * horizontalBurstForce;

        if (playerScript.IsCloseToWall())// if close to the wall disable bursting
        {
            DisableBurst();
            playerScript.rb.velocity = Vector3.zero;// reset the speed
        }
        else
        {
            playerScript.rb.AddForce(forceDir);
        }
    }

    void ContrainVelocity() // clamp velocity when the velocity magnitude is too high
    {
        float maxVelocity = playerScript.maxBurstVelocity;
        if (playerScript.rb.velocity.magnitude > maxVelocity)
        {
            playerScript.rb.velocity = Vector3.ClampMagnitude(playerScript.rb.velocity, maxVelocity);
        }
    }


}


