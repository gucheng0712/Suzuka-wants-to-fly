using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    void Start()
    {

        if (GameObject.FindGameObjectWithTag("Enemy"))
        {
            //transform.LookAt(GameObject.FindGameObjectWithTag("Enemy").transform.position + Vector3.up * 0.5f);
        }
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        GetComponent<Rigidbody>().velocity = (transform.forward * 10f);
    }

    void OnTriggerEnter(Collider col)
    {
        Destroy(gameObject);
    }

}
