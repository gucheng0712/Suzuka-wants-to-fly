using UnityEngine;



public class Player : MonoBehaviour
{
    public GameObject hook;
    public GameObject[] weapons = new GameObject[2];
    public float swingSpeedMultiplier = 2f;
    public float maxBurstVelocity = 50f;
    [Range(0, 1)] public float airControlPct;
    public LayerMask groundableLayer;

    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Camera mainCam;
    [HideInInspector] public LineRenderer line;
    [HideInInspector] public Animator anim;

    [HideInInspector] public CapsuleCollider myCollider;
    // distance to check ground and side collsion
    [HideInInspector] public float disToGround;
    [HideInInspector] public float disToSide;

    // Conditions
    [HideInInspector] public bool isFalling;
    [HideInInspector] public bool isRunning;
    [HideInInspector] public bool isSwinging;
    [HideInInspector] public bool isBursting;
    [HideInInspector] public bool isJumping;
    [HideInInspector] public bool isWallWalking;
    [HideInInspector] public bool isDashing;
    public bool isEncounterEnemy;
    public bool isInAttackMode;
    public bool isShooting;
    public bool isAttacking;
    [HideInInspector] public float attackMoveVelocity;

    [HideInInspector] public GameManager_Audio audioManager;
    [HideInInspector] public AudioSource audioSource;

    float turnSmoothVelocity;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        myCollider = GetComponentInChildren<CapsuleCollider>();
        line = GetComponent<LineRenderer>();
        audioManager = FindObjectOfType<GameManager_Audio>();
        audioSource = GetComponentInChildren<AudioSource>();
    }

    void Start()
    {
        GameManager.state = GameManager.NORMAL_STATE;
        mainCam = Camera.main;
        disToGround = myCollider.bounds.extents.y; // get the half radius of the capsule collider 
        disToSide = myCollider.bounds.extents.x;// get the half height of the capsule collider
    }


    void Update()
    {
        if (IsGrounded())
        {
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            anim.SetBool("IsInAttackMode", isInAttackMode);
            if (isInAttackMode && !isSwinging && !isBursting)
            {
                GameManager.state = GameManager.BATTLE_STATE;
            }
            else
            {
                GameManager.state = GameManager.NORMAL_STATE;
            }
        }


        //if (IsGrounded())
        //{
        //    if (GameManager.BATTLE_STATE_INPUT)
        //    {
        //        isInAttackMode = !isInAttackMode;
        //        ActiveWeapon(weapons[0]);
        //        anim.SetBool("IsInAttackMode", isInAttackMode);
        //        if (isInAttackMode)
        //        {
        //            GameManager.state = GameManager.BATTLE_STATE;
        //        }
        //        else
        //        {
        //            
        //        }
        //    }

        //}
    }

    void FixedUpdate()
    {
        // use for check if in battle state


        if (!isSwinging && !isInAttackMode)
        {
            CheckCollisionDetectionMode();
            if (IsGrounded()) // if grounded back to normal state
            {
                if (line.enabled == true)
                {
                    line.enabled = false;
                }

                GameManager.state = GameManager.NORMAL_STATE;
                // if is grounded player will reset the x and z rotation to make player stand up
                isFalling = false;
            }
            else
            {
                if (IsCloseToWall() && !isWallWalking) // if player is close to the wall, the state will be wallwalking state, player have ability to wallwalking
                {
                    GameManager.state = GameManager.WALLWALK_STATE;
                    isFalling = false;
                }
                else
                {
                    Quaternion targetRot = Quaternion.LookRotation(mainCam.transform.forward);// if isn't grounded player  rotate along camera x, y, z axis
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10 * Time.deltaTime);// smooth rotation
                    isFalling = true;
                }
            }
        }
    }


    // check ground
    public bool IsGrounded()
    {
        RaycastHit hit;

        Vector3 rayStartPos;

        float centerX = myCollider.bounds.center.x;
        float centerZ = myCollider.bounds.center.z;
        float centerY = myCollider.bounds.center.y;

        // Middle Raycast
        rayStartPos = new Vector3(centerX, centerY, centerZ);
        Debug.DrawRay(rayStartPos, -transform.up, Color.cyan);
        if (Physics.Raycast(rayStartPos, Vector3.down, out hit, disToGround + 0.1f, groundableLayer))
        {
            return true;
        }

        // Left Raycast
        rayStartPos = new Vector3(centerX - disToSide, centerY, centerZ);
        Debug.DrawRay(rayStartPos, Vector3.down, Color.cyan);
        if (Physics.Raycast(rayStartPos, Vector3.down, out hit, disToGround + 0.1f, groundableLayer))
        {
            return true;
        }

        // Right Raycast
        rayStartPos = new Vector3(centerX + disToSide, centerY, centerZ);
        Debug.DrawRay(rayStartPos, Vector3.down, Color.cyan);
        if (Physics.Raycast(rayStartPos, Vector3.down, out hit, disToGround + 0.1f, groundableLayer))
        {
            return true;
        }

        // Forward Raycast
        rayStartPos = new Vector3(centerX, centerY, centerZ + disToSide);
        Debug.DrawRay(rayStartPos, Vector3.down, Color.cyan);
        if (Physics.Raycast(rayStartPos, Vector3.down, out hit, disToGround + 0.1f, groundableLayer))
        {
            return true;
        }

        // Backward Raycast
        rayStartPos = new Vector3(centerX, centerY, centerZ - disToSide);
        Debug.DrawRay(rayStartPos, Vector3.down, Color.cyan);
        if (Physics.Raycast(rayStartPos, Vector3.down, out hit, disToGround + 0.1f, groundableLayer))
        {
            return true;
        }
        return false;
    }


    // check if close to wall
    public bool IsCloseToWall()
    {
        RaycastHit hit;
        if (Physics.Raycast(myCollider.bounds.center, transform.forward, out hit, disToSide + 0.5f, groundableLayer))  // forward check
        {
            return true;
        }
        //else if (Physics.Raycast(myCollider.bounds.center, -transform.forward, out hit, disToSide + 0.5f, groundableLayer)) // back check
        //{
        //    return true;
        //}
        //else if (Physics.Raycast(myCollider.bounds.center, transform.right, out hit, disToSide + 0.5f, groundableLayer)) // right check
        //{
        //    return true;
        //}
        //else if (Physics.Raycast(myCollider.bounds.center, -transform.right, out hit, disToSide + 0.5f, groundableLayer)) // left check
        //{
        //    return true;
        //}
        else
        {
            return false; // if didn't detect anything return false;
        }
    }

    void CheckCollisionDetectionMode()  // prevent player go through the wall when velocity too high;
    {
        if (rb.velocity.sqrMagnitude > 5)
        {
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
        else
        {
            rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
        }
    }

    // Check if is in animation 
    public bool CheckState(string stateName, string layerName = "Base Layer")
    {
        int layerIndex = anim.GetLayerIndex(layerName);
        return anim.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName);

    }
    public bool CheckInTransition(string layerName = "Base Layer")
    {
        int layerIndex = anim.GetLayerIndex(layerName);
        return anim.IsInTransition(layerIndex);
    }

    public float GetModifiedSmoothTime(float smoothTime)
    {
        if (!isDashing)
        {
            return smoothTime;
        }
        else
        {
            if (airControlPct == 0)
            {
                return float.MaxValue;
            }
            return smoothTime / airControlPct;
        }
    }

}
