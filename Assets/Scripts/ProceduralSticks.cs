using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralSticks : MonoBehaviour
{
    public GameObject stick;
    private GameObject[] sticks;
    [Range(0.0f, 1000.0f)] public float spawnPower = 500f;

    public float RenderDistance = 20f;

    private float nextTime = 2f;

    private GameObject player;

    private GameObject[] worldObjects;
    public float worldObjectRender = 120f;

    // Start is called before the first frame update
    void Start()
    {
        worldObjects = GameObject.FindGameObjectsWithTag("WorldObject");
        player = GameObject.FindGameObjectWithTag("Player");
        for(int i = 0; i < spawnPower; i++)
        {
            spawn();
        }
        sticks = GameObject.FindGameObjectsWithTag("stick");
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > nextTime)
        {
            float distance; 
            for(int i = 0; i < sticks.Length; i++)
            {
                distance = Vector3.Distance(player.transform.position, sticks[i].transform.position);
                if(distance > RenderDistance)
                {
                    sticks[i].SetActive(false);
                }
                else if(sticks[i].activeSelf == false)
                {
                    sticks[i].SetActive(true);
                }
            }
            for(int i = 0; i < worldObjects.Length; i++)
            {
                distance = Vector3.Distance(player.transform.position, worldObjects[i].transform.position);
                if (distance > worldObjectRender)
                {
                    worldObjects[i].SetActive(false);
                }
                else if (worldObjects[i].activeSelf == false)
                {
                    worldObjects[i].SetActive(true);
                }
            }
            nextTime += 1f;
        }
    }

    void spawn()
    {
        Vector3 RandomPosition = new Vector3(Random.Range(-240, 240), .1f, Random.Range(-240, 240));

        float randomYRotation = Random.Range(0f, 360f); // Generate a random angle for rotation around the y-axis
        Quaternion randomRotation = Quaternion.Euler(0, randomYRotation, 0); // Create a rotation using the random angle
        
        Instantiate(stick, RandomPosition, randomRotation);
    }
}
