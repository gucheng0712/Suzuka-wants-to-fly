using System.Collections;

using UnityEngine;


public class Player_AttackMode : MonoBehaviour
{
    Player playerScript;
    Player_Animation playerScript_Animation;
    Vector3 moveDir;
    [Range(0, 1)] public float airControlPct;
    [Header("Player Settings")]
    float moveSpeed = 5f;
    float lastDashTime;
    [SerializeField] float dashsCoolDown = 1f;
    float speedMultiplier;

    float currentSpeed;
    /// <summary>
    /// ref for The turn smooth velocity.
    /// </summary>
    float turnSmoothVelocity;
    float moveSmoothVelocity;


    void Awake()
    {
        playerScript = GetComponent<Player>();
        playerScript_Animation = GetComponent<Player_Animation>();
    }

    void Update()
    {
        if (GameManager.state == GameManager.BATTLE_STATE)
        {
            PlayerInput();
        }

    }
    public void PlayerInput()
    {
        Vector2 moveInputDir = GameManager.MOVE_INPUT;  // movedir input
        bool dash = GameManager.DASH_INPUT;    // dash input
        RotatePlayer(moveInputDir);   // rotate the player
        MovePlayer(moveInputDir, dash);


        switch (GameManager.weaponIndex)
        {
            case GameManager.PISTOL_INDEX:
                if (GameManager.SHOOT_INPUT)
                {
                    if (!playerScript.isShooting)
                    {
                        if (!playerScript.CheckState("Dashing") && !playerScript.CheckInTransition() && !playerScript.CheckInTransition("Melee Layer"))
                        {
                            playerScript.anim.SetTrigger("Shoot");
                            if (GameObject.FindGameObjectWithTag("Enemy"))
                            {
                                Vector3 targetDir = GameObject.FindGameObjectWithTag("Enemy").transform.position - transform.position;
                                targetDir.y = 0;
                                transform.rotation = Quaternion.LookRotation(targetDir);
                            }
                        }
                    }
                }
                break;
            case GameManager.HIDDENBLADE_INDEX:
                if (GameManager.MELEE_INPUT)
                {
                    if (!playerScript.isAttacking)
                    {
                        if (!playerScript.CheckState("Dashing") && !playerScript.CheckInTransition() && !playerScript.CheckInTransition("Shoot Layer"))
                        {
                            playerScript.anim.SetTrigger("Attack");
                            if (GameObject.FindGameObjectWithTag("Enemy"))
                            {
                                Vector3 targetDir = GameObject.FindGameObjectWithTag("Enemy").transform.position - transform.position;
                                targetDir.y = 0;
                                transform.rotation = Quaternion.LookRotation(targetDir);
                            }
                        }
                    }
                }
                break;
        }

    }


    void RotatePlayer(Vector2 _inputDir)
    {
        if (_inputDir != Vector2.zero)
        {
            if (!playerScript.isShooting && !playerScript.isAttacking)
            {
                //float targetRotation = (!playerScript.isShooting && !playerScript.isAttacking) ?
                //Mathf.Atan2(_inputDir.x, _inputDir.y) * Mathf.Rad2Deg + playerScript.mainCam.transform.eulerAngles.y :
                //playerScript.mainCam.transform.eulerAngles.y;
                float targetRotation = Mathf.Atan2(_inputDir.x, _inputDir.y) * Mathf.Rad2Deg + playerScript.mainCam.transform.eulerAngles.y;
                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, playerScript.GetModifiedSmoothTime(.1f), 2000f, Time.deltaTime);
            }
        }
    }

    void MovePlayer(Vector2 _inputDir, bool _dash)
    {
        if (_dash)
        {
            if (!playerScript.isAttacking && !playerScript.isShooting)
            {
                if ((Time.time - lastDashTime) > dashsCoolDown)
                {
                    lastDashTime = Time.time;
                    playerScript.audioSource.PlayOneShot(playerScript.audioManager.dashSound);
                    playerScript.anim.SetTrigger("Dash");
                }
            }
        }
        else
        {
            playerScript_Animation.Anim_Movement(_inputDir.magnitude);
        }
        float targetSpeed = moveSpeed * _inputDir.magnitude;
        currentSpeed = (playerScript.isDashing) ? moveSpeed : Mathf.SmoothDamp(currentSpeed, targetSpeed, ref moveSmoothVelocity, playerScript.GetModifiedSmoothTime(0.1f));

        speedMultiplier = (playerScript.isDashing) ? 2 : 1;
        if (playerScript.isDashing)
        {
            speedMultiplier = 2;
        }
        else if (playerScript.isShooting)
        {
            speedMultiplier = 0;
        }
        else if (playerScript.isAttacking)
        {
            speedMultiplier = playerScript.attackMoveVelocity;
        }
        else
        {
            speedMultiplier = 1;
        }

        moveDir = transform.forward * currentSpeed * speedMultiplier;
        playerScript.rb.velocity = new Vector3(moveDir.x, playerScript.rb.velocity.y, moveDir.z);
    }
}
