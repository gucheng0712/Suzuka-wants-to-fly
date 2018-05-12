using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ThirdPersonCamera : MonoBehaviour
{
    public GameManager inputManager;
    Player playerScript;
    [Header("Mouse Settings")]
    [SerializeField]
    bool lockCursor = true;
    [SerializeField]
    float mouseSensitivity = 10;
    //    float scrollSpeed = 3;

    Transform target; // target transfrom need to follow
    float rotateY_Input;
    float rotateX_Input;
    Vector2 pitchClampValue = new Vector2(-15, 85);

    public float normalDisFromTarget = 3.5f;
    public Vector2 DisFromTargetClampValue = new Vector2(1, 5); // clamp value of the distance between camera and player
    public float currentDisFromTarget = 8; // give a value higher than presetDistance for making a smooth lerp from far to close when starting the game
    public float disFromTarget = 3;
    Vector3 currentRotation;
    /// <summary>
    /// ref for SmoothDamp
    /// </summary>
    Vector3 rotationSmoothVelocity;
    float scrollSmoothVelocity;


    void Awake()
    {
        //inputManager = GetComponentInChildren<GameManager_Input>();
        //  inputManager =GameObject.FindGameObjectWithTag("ViveController_R").GetComponent<GameManager_Input>();

        playerScript = FindObjectOfType<Player>();
        target = GameObject.FindGameObjectWithTag("LookTarget").transform;
    }

    void Start()
    {
        if (inputManager.inputMode != InputMode.VR)
        {
            if (lockCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    void Update()
    {
        if (inputManager.inputMode != InputMode.VR)
        {
            CursorVisibleSwitch();
        }

    }

    void LateUpdate()
    {
        if (inputManager.inputMode == InputMode.PC)
        {
            //print(inputManager.inputMode);
            rotateY_Input += Input.GetAxis("Mouse X") * mouseSensitivity;
            rotateX_Input -= Input.GetAxis("Mouse Y") * mouseSensitivity; // the vertical move of the mouse is opposite to the MouseY Input 
            rotateX_Input = Mathf.Clamp(rotateX_Input, pitchClampValue.x, pitchClampValue.y);
            CameraViewAlongSpeedChange();
            Vector3 targetRotation = new Vector3(rotateX_Input, rotateY_Input, 0);
            currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref rotationSmoothVelocity, 0.12f);
            Camera.main.transform.eulerAngles = currentRotation;
            transform.position = target.position - Camera.main.transform.forward * currentDisFromTarget; ;

        }

    }

    void CameraViewAlongSpeedChange()
    {


        if (playerScript.isSwinging || playerScript.isBursting)
        {
            disFromTarget = DisFromTargetClampValue.y;
        }
        else if (playerScript.isRunning)
        {
            disFromTarget = DisFromTargetClampValue.y * 0.8f;
        }
        else
        {
            disFromTarget -= Input.GetAxis("Mouse ScrollWheel") * 2f;
        }
        disFromTarget = Mathf.Clamp(disFromTarget, DisFromTargetClampValue.x, DisFromTargetClampValue.y);
        currentDisFromTarget = Mathf.SmoothDamp(currentDisFromTarget, disFromTarget, ref scrollSmoothVelocity, 0.3f);
    }

    void CursorVisibleSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Escape))// if press escape swithc the locCorsor mode
        {
            lockCursor = !lockCursor;
        }
        if (lockCursor)
        {

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

    }
}
