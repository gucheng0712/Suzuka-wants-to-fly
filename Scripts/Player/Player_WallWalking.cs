using System.Collections;
using UnityEngine;

public class Player_WallWalking : MonoBehaviour
{
    [SerializeField] float moveSpeed = 6;
    [SerializeField] float jumpForce = 300;

    bool isInJumpingTime;

    Player playerScript;

    Vector3 surfaceNormal; // normal of the landing ground(maybe wall or ground)
    Vector3 upPos; // up position of the character

    void Awake()
    {
        playerScript = GetComponent<Player>();
    }

    void Start()
    {
        upPos = transform.up; // normal starts as character up direction
    }

    void Update()
    {
        if (GameManager.state == GameManager.WALLWALK_STATE)
        {
            CheckWallWalking();
            WallWalk();
        }
        else
        {
            playerScript.isWallWalking = false;
        }
    }

    void UpdatePlayerRotation()
    {
        upPos = Vector3.Lerp(upPos, surfaceNormal, 10 * Time.deltaTime);// update  character up position to surface normal

        Vector3 myForward = Vector3.Cross(transform.right, upPos);// find forward direction with new upPos:

        // rotate character to the new upPos while keeping the forward direction:
        Quaternion targetRot = Quaternion.LookRotation(myForward, upPos);
        transform.rotation = targetRot;

    }
    void WallWalk()
    {
        float wallWalkInput = GameManager.MOVE_INPUT.y;
        if (!playerScript.isWallWalking && Mathf.Abs(wallWalkInput) > 0.1f)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, playerScript.disToSide + 0.5f, playerScript.groundableLayer)) // check if there is a wall ahead
            {
                JumpToWall(hit.point, hit.normal);
            }
        }

        if (playerScript.isWallWalking)// jump pressed:
        {

            // when the input magnitude is 0 or jump, player cannot will fall down
            if (Mathf.Abs(wallWalkInput) < 0.1f)
            {
                playerScript.isWallWalking = false;
                return;
            }
            else if (GameManager.JUMP_INPUT)
            {
                playerScript.isWallWalking = false;
                playerScript.rb.AddForce((surfaceNormal + Vector3.up * 2) * jumpForce);
                return;
            }
            UpdatePlayerRotation();
            playerScript.rb.velocity = wallWalkInput * transform.forward * moveSpeed;// MoveDirection

        }
    }

    void CheckWallWalking()
    {
        Ray ray = new Ray(transform.position, -upPos); // cast ray check if is grounded the wall
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, playerScript.disToGround + 0.1f, playerScript.groundableLayer))
        {
            playerScript.isWallWalking = true;
            surfaceNormal = hit.normal;// use it to update surfaceNormal
        }
        else
        {
            playerScript.isWallWalking = false;
            // to make sure when player leave wall the surface normal will reset to Vector3.up, so that it will rotate player back
            surfaceNormal = Vector3.up;// TODO: still need to test
        }
    }

    void JumpToWall(Vector3 point, Vector3 normal)// rotate player align the wall
    {
        Vector3 myForward = Vector3.Cross(transform.right, normal);
        Quaternion targetRot = Quaternion.LookRotation(myForward, normal);

        transform.position = point + normal * (playerScript.disToGround); //give a offset position to make player not go through the wall
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 0.1f);
        upPos = normal;
    }
}
