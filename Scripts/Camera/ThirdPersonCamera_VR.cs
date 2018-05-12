using UnityEngine;


public class ThirdPersonCamera_VR : MonoBehaviour
{
    Player playerScript;

    Transform target; // target transfrom need to follow

    public float normalDisFromTarget = 3.5f;
    public Vector2 DisFromTargetClampValue = new Vector2(1, 5); // clamp value of the distance between camera and player
    public float currentDisFromTarget = 8; // give a value higher than presetDistance for making a smooth lerp from far to close when starting the game
    public float disFromTarget = 3;

    float scrollSmoothVelocity;


    void Awake()
    {
        playerScript = FindObjectOfType<Player>();
        target = GameObject.FindGameObjectWithTag("LookTarget").transform;
    }



    void LateUpdate()
    {
        transform.position = target.position - Camera.main.transform.forward * currentDisFromTarget; // or eye camera
        Vector3 offset = transform.position - Camera.main.transform.position;
        offset.y = 0;
        transform.position = transform.position + offset;
        CameraViewAlongSpeedChange();

    }

    void CameraViewAlongSpeedChange()
    {
        disFromTarget = Mathf.Clamp(disFromTarget, DisFromTargetClampValue.x, DisFromTargetClampValue.y);
        if (playerScript.isSwinging || playerScript.isBursting)
        {
            disFromTarget = DisFromTargetClampValue.y;
        }
        else if (!(playerScript.isSwinging || playerScript.isBursting))
        {
            disFromTarget = (playerScript.isRunning) ? DisFromTargetClampValue.y * 0.8f : normalDisFromTarget;
        }
        else
        {
            disFromTarget = normalDisFromTarget;
        }
        currentDisFromTarget = Mathf.SmoothDamp(currentDisFromTarget, disFromTarget, ref scrollSmoothVelocity, 0.3f);
    }
}
