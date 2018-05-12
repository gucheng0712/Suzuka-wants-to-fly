using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLightDir : MonoBehaviour
{

    void Update()
    {
        // rotating the character light align the camera to make character looks good
        Quaternion targetRot = Quaternion.LookRotation(Camera.main.transform.forward + Camera.main.transform.right);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 5 * Time.deltaTime);
    }
}
