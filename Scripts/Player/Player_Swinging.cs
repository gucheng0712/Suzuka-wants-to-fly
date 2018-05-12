using UnityEngine;

public class Player_Swinging : MonoBehaviour
{
    [SerializeField] float swingHorizontalForce = 15f;
    [SerializeField] float swingForwardForce = 20f;

    public Transform leftHand; // line renderer start transform

    Player playerScript;
    float ropeLength = 15f;
    Vector3 targetPos;

    void Awake()
    {
        playerScript = GetComponent<Player>();
    }

    void Update()
    {
        if (playerScript.isInAttackMode)
        {
            DisableHook();
            return;
        }
        SwingInput();
    }

    void SwingInput()
    {
        if (GameManager.SWING_START_INPUT)
        {
            if (!playerScript.IsGrounded() && !playerScript.isWallWalking && !playerScript.isBursting && !playerScript.isInAttackMode)
            {
                GameManager.state = GameManager.SWING_STATE;
                FindTargetPoint();
                EnableHook();
            }
        }
        else if (GameManager.SWINGING_INPUT && GameManager.state == GameManager.SWING_STATE)
        {
            if (playerScript.isSwinging)
            {
                Swinging();
                ContrainVelocity(playerScript.swingSpeedMultiplier);
            }
        }
        else if (GameManager.SWING_END_INPUT && GameManager.state == GameManager.SWING_STATE)
        {
            DisableHook();
        }
        else
        {
            DisableHook();
        }
    }
    void FindTargetPoint() // preset the target pos
    {
        Vector3 targetForward = playerScript.mainCam.transform.position + playerScript.mainCam.transform.forward * ropeLength * .2f;
        Vector3 targetHeight = playerScript.mainCam.transform.up * ropeLength;
        targetPos = targetForward + targetHeight;
        playerScript.hook.transform.position = targetPos;
    }

    void EnableHook()
    {
        playerScript.isSwinging = true;
        // reset angular velocity
        playerScript.hook.transform.rotation = Quaternion.identity;
        playerScript.hook.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        playerScript.rb.freezeRotation = false;
        playerScript.hook.SetActive(true);
        if (playerScript.hook.GetComponent<FixedJoint>() == null)
        {
            playerScript.hook.AddComponent<FixedJoint>().connectedBody = playerScript.rb;
        }

    }
    void DisableHook()
    {
        playerScript.isSwinging = false;
        playerScript.rb.angularDrag = 5f;

        // when stop swinging destroy the hook; 
        Destroy(playerScript.hook.GetComponent<FixedJoint>()); //TODO: If Possible, Put <FixedJoint> into ObjectPool Manager in the future
        playerScript.line.enabled = false;
        playerScript.hook.SetActive(false);
        playerScript.rb.freezeRotation = true;
    }
    void Swinging()
    {
        playerScript.line.enabled = true;
        playerScript.line.SetPosition(0, leftHand.position);// line renderer start
        playerScript.line.SetPosition(1, playerScript.hook.transform.position); // line renderer end
        float input_Horizontal = GameManager.MOVE_INPUT.x;

        // force direction
        Vector3 forceDir = transform.forward * swingForwardForce + playerScript.mainCam.transform.right * input_Horizontal * swingHorizontalForce;
        playerScript.rb.AddForce(forceDir);

    }

    // Clamp rigidbody velocity
    void ContrainVelocity(float multiplier)
    {
        if (playerScript.hook.activeSelf)
        {
            float maxVelocity = (transform.position - playerScript.hook.transform.position).magnitude * multiplier;
            if (playerScript.rb.velocity.magnitude > maxVelocity)
            {
                playerScript.rb.velocity = Vector3.ClampMagnitude(playerScript.rb.velocity, maxVelocity);
            }
        }
    }

}
