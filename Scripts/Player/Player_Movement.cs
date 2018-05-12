using System.Collections;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    [SerializeField] float walkSpeed = 2f;
    [SerializeField] float runSpeed = 4f;
    [SerializeField] float jumpPower = 300f;


    Player playerScript;
    Player_Animation playerAnimation;
    Vector3 moveDir;
    float currentSpeed;

    /// <summary>
    /// ref for SmoothDamp
    /// </summary>
    float turnSmoothVelocity;
    float moveSmoothVelocity;

    void Awake()
    {
        playerScript = GetComponent<Player>();
        playerAnimation = GetComponent<Player_Animation>();

    }

    void Update()
    {
        if (GameManager.state == GameManager.NORMAL_STATE && !playerScript.isFalling)
        {
            MovementInput();
        }
    }

    public void MovementInput()
    {
        Vector2 moveInputDir = GameManager.MOVE_INPUT;  // movedir input
        playerScript.isRunning = GameManager.RUN_INPUT;    // running input
        bool jumpInput = GameManager.JUMP_INPUT;  // jump input
        RotatePlayer(moveInputDir);   // rotate the player
        MovePlayer(moveInputDir, playerScript.isRunning);
        JumpInput(playerScript.isRunning, jumpInput);
    }
    void JumpInput(bool _running, bool _jumpInput)
    {

        if (playerScript.IsGrounded()) // if player is grounded, he will able to jump and move
        {
            if (_jumpInput)
            {
                Jump(_running);
            }
            else
            {
                playerScript.isJumping = false;
            }
            playerScript.rb.velocity = new Vector3(moveDir.x, playerScript.rb.velocity.y, moveDir.z);
            playerAnimation.Anim_Jump(playerScript.isJumping);
        }
    }

    void RotatePlayer(Vector2 _inputDir)
    {
        // if there are inputs for movement, then player will  rotate along the camera
        if (_inputDir != Vector2.zero)
        {
            // geometry use right hand coordinate system
            // unity use left hand coordinate system
            // so in order to get the Angle of the input should be this equation angle= atan(x/y), not in math equation angle =atan(y/x);
            // caculate the the angle between input dir, the range of atan[-pi/2,Pi/2], However the range of atan2 is [-pi,pi];
            //so use atan2
            float targetRotation = Mathf.Atan2(_inputDir.x, _inputDir.y) * Mathf.Rad2Deg + playerScript.mainCam.transform.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, 0.1f, 2000f, Time.deltaTime);
        }
    }

    void MovePlayer(Vector2 _inputDir, bool _running) // add speed for player when moving
    {
        float targetSpeed = ((_running) ? runSpeed : walkSpeed) * _inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref moveSmoothVelocity, 0.1f);
        moveDir = transform.forward * currentSpeed;
        playerAnimation.Anim_Movement(_inputDir.magnitude);
        LockMovementWhenJumping();
    }

    void Jump(bool _running) // jump function different jumppower between running and not walking
    {
        playerScript.isJumping = true;

        StartCoroutine(JumpDelay(_running));
    }

    public void LockMovementWhenJumping() // limit animation when jumping // TODO: Add more limitation for animation
    {
        if (playerScript.anim.GetCurrentAnimatorStateInfo(0).IsName("JumpFwd_Land"))
        {
            moveDir.x = Mathf.Lerp(moveDir.x, 0, 0.1f);
            moveDir.z = Mathf.Lerp(moveDir.z, 0, 0.1f);
        }
        if (playerScript.anim.GetCurrentAnimatorStateInfo(0).IsName("Fall") && playerScript.anim.IsInTransition(0))
        {
            moveDir.x = 0;
            moveDir.z = 0;
        }
    }
    IEnumerator JumpDelay(bool _running)
    {
        yield return new WaitForSeconds(0.2f);
        float jumpForce = (_running) ? jumpPower * 1.5f : jumpPower;
        playerScript.rb.AddForce(transform.up * jumpForce);
    }

}
