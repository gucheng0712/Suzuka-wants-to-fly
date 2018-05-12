using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateDetector : MonoBehaviour
{
    MapGenerator mapGenerator;
    Vector3 newMapPos;
    Transform player;
    public GameObject detector;
    Ray detectorRay;

    float lastDetectTime;
    float detectCooldown = 0.2f;

    void Start()
    {
        mapGenerator = FindObjectOfType<MapGenerator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        detector = GameObject.CreatePrimitive(PrimitiveType.Cube);
        detector.name = "Detector Cube";
        detector.transform.parent = transform;
        detector.transform.position = transform.position;
        detector.transform.Translate(0, -100, 0);
        detectorRay = new Ray();
    }

    void Update()
    {
        if (Time.time - lastDetectTime < detectCooldown)
        {
            return;
        }

        lastDetectTime = Time.time;
        Vector3 currentPos = transform.position;
        int detectDirIndex = Random.Range(0, 4);
        detectorRay.origin = detector.transform.position;

        if (detectDirIndex == 0) //left
        {
            newMapPos = new Vector3(currentPos.x - mapGenerator.mapSize * mapGenerator.spaceMultplier, currentPos.y, currentPos.z);
            detectorRay.direction = -detector.transform.right;
        }
        else if (detectDirIndex == 1) //right
        {
            newMapPos = new Vector3(currentPos.x + mapGenerator.mapSize * mapGenerator.spaceMultplier, currentPos.y, currentPos.z);
            detectorRay.direction = detector.transform.right;
        }
        else if (detectDirIndex == 2) //forward
        {
            newMapPos = new Vector3(currentPos.x, currentPos.y, currentPos.z + mapGenerator.mapSize * mapGenerator.spaceMultplier);
            detectorRay.direction = detector.transform.forward;
        }
        else if (detectDirIndex == 3) //backward
        {
            newMapPos = new Vector3(currentPos.x, currentPos.y, currentPos.z - mapGenerator.mapSize * mapGenerator.spaceMultplier);
            detectorRay.direction = -detector.transform.forward;
        }


        if (Vector3.Distance(newMapPos, player.transform.position) < 2 * mapGenerator.mapSize * mapGenerator.spaceMultplier)
        {
            if (!Physics.Raycast(detectorRay, mapGenerator.mapSize * mapGenerator.spaceMultplier))
            {
                GameObject city = mapGenerator.pool[0];
                mapGenerator.pool.Remove(city);
                city.SetActive(true);
                city.transform.position = newMapPos;
                mapGenerator.pool.Add(city);

            }
        }

        if (Vector3.Distance(transform.position, player.transform.position) > 3 * mapGenerator.mapSize * mapGenerator.spaceMultplier)
        {
            gameObject.SetActive(false);
        }
    }


}
