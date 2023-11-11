using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralSticks : MonoBehaviour
{
    public GameObject stick;
    [Range(0.0f, 1000.0f)] public float spawnPower = 500f;
    
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < spawnPower; i++)
        {
            spawn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void spawn()
    {
        Vector3 RandomPosition = new Vector3(Random.Range(-240, 240), .1f, Random.Range(-240, 240));

        float randomYRotation = Random.Range(0f, 360f); // Generate a random angle for rotation around the y-axis
        Quaternion randomRotation = Quaternion.Euler(0, randomYRotation, 0); // Create a rotation using the random angle

        Instantiate(stick, RandomPosition, randomRotation);
    }
}
