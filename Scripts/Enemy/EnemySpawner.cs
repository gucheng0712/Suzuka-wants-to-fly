using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public ParticleSystem spawnEffect;
    public GameObject enemyPrefab;
    Vector3 spawnPos;
    public float spawnDelay = 1f;
    // Use this for initialization
    void Start()
    {
        spawnPos = transform.position + Vector3.up * 2f;
        spawnEffect = GameObject.FindGameObjectWithTag("SpawnEffect").GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            spawnEffect.transform.position = spawnPos;

            spawnEffect.Play();
            StartCoroutine("SpawnDelay");

        }
    }

    IEnumerator SpawnDelay()
    {
        yield return new WaitForSeconds(1f);
        Instantiate(enemyPrefab, spawnPos + Vector3.down * 0.7f, Quaternion.identity);
        gameObject.SetActive(false);
    }



}
