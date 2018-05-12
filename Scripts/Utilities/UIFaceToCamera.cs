using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFaceToCamera : MonoBehaviour
{
    void LateUpdate()
    {
        //Vector3 targetPos = -Camera.main.transform.forward;
        //targetPos.y = 0;
        //transform.rotation = Quaternion.LookRotation(targetPos);
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.back,
                         Camera.main.transform.rotation * Vector3.up);
    }
}
