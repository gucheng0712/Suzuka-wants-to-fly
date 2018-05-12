using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public List<GameObject> pool = new List<GameObject>();

    GameObject parent;
    const int STREET_V = -1;
    const int STREET_H = -2;
    const int STREET_CROSS = -3;

    public GameObject ground;
    public GameObject[] buildings;
    public GameObject horizontalStreet;
    public GameObject verticalStreet;
    public GameObject crossStreet;

    public int mapSize = 50;
    public int spaceMultplier = 50;
    public float buildingOffset = 25f;
    int[,] mapGrid;
    Vector3 offset;


    void Start()
    {
        offset = new Vector3(mapSize * spaceMultplier / 2 - buildingOffset, -0.1f, mapSize * spaceMultplier / 2 - buildingOffset);
        GenerateCity(Vector3.zero);

        // instantiate 15 city at the begining
        for (int i = 0; i < 30; i++)
        {
            GameObject city = GenerateCity(Vector3.zero);
            city.SetActive(false);
            pool.Add(city);
        }
    }

    public GameObject GenerateCity(Vector3 generatePos)
    {
        parent = new GameObject("City");

        mapGrid = new int[mapSize, mapSize];
        for (int w = 0; w < mapSize; w++)
        {
            for (int h = 0; h < mapSize; h++)
            {
                mapGrid[w, h] = Random.Range(0, buildings.Length);
            }
        }

        //roads
        int x = 0;
        int randomV = Random.Range(3, 3);
        for (int w = 0; w < mapSize; w++)
        {
            x += randomV;
            if (x >= mapSize) break;
            for (int h = 0; h < mapSize; h++)
            {
                mapGrid[x, h] = -1;// vertical roads
            }
        }

        int z = 0;
        int randomSeed = Random.Range(5, 5);
        for (int h = 0; h < mapSize; h++)
        {
            if (z >= mapSize) break;
            for (int w = 0; w < mapSize; w++)
            {
                if (mapGrid[w, z] == -1)// if the road is occupied by vertical road
                {
                    mapGrid[w, z] = -3; //      this road should be replaced by crossroad
                }
                else
                {
                    mapGrid[w, z] = -2;//      THIS road should be the horizontal road
                }
            }
            z += randomSeed;
        }


        for (int w = 0; w < mapSize; w++)
        {
            for (int h = 0; h < mapSize; h++)
            {
                int result = mapGrid[w, h];
                Vector3 pos = new Vector3(w * spaceMultplier, 0, h * spaceMultplier);
                switch (result)
                {
                    case STREET_V:
                        Instantiate(verticalStreet, pos, verticalStreet.transform.rotation, parent.transform);
                        break;
                    case STREET_H:
                        Instantiate(horizontalStreet, pos, horizontalStreet.transform.rotation, parent.transform);
                        break;
                    case STREET_CROSS:
                        Instantiate(crossStreet, pos, crossStreet.transform.rotation, parent.transform);
                        break;
                    default:
                        Instantiate(buildings[result], pos, transform.rotation, parent.transform);
                        break;
                }

            }
        }
        parent.AddComponent(typeof(GenerateDetector));
        GameObject newGround = Instantiate(ground, parent.transform.position + offset, Quaternion.identity);
        newGround.transform.localScale = new Vector3(mapSize * spaceMultplier, 0f, mapSize * spaceMultplier);
        newGround.transform.SetParent(parent.transform);
        parent.transform.position = generatePos;
        return parent;
    }


}
