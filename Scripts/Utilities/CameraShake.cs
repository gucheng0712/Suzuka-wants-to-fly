using UnityEngine;
using System.Collections;
public class CameraShake : MonoBehaviour
{
    float shakeDelta = 0.01f;
    public Camera cam;
    public float shakeFPS = 30;
    public static bool isshakeCamera = false;
    float frameTime;
    Vector3 originalPosition;
    private void Start()
    {
        originalPosition = transform.localPosition;
    }

    void LateUpdate()
    {
        if (isshakeCamera)
        {
            frameTime += Time.deltaTime;
            if (frameTime > (1 / shakeFPS))
            {
                //frameTime = 0;
                //cam.rect = new Rect(shakeDelta * (-1.0f + 2.0f * Random.value), shakeDelta * (-1.0f + 2.0f * Random.value), 1.0f, 1.0f);
                Camera.main.transform.localPosition += (Random.insideUnitSphere * shakeDelta);
            }
        }
        else
        {
            Camera.main.transform.localPosition = originalPosition;
        }
    }

}

