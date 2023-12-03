using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkLoader : MonoBehaviour
{
    private Terrain[] chunks;
    Vector3 playerPos;

    private float nextFire;
    public GameObject[] bears;
    public GameObject[] deers;
    public GameObject[] snakes;

    public float renderDistance = 150f;
    // Start is called before the first frame update
    void Start()
    {
        deers = GameObject.FindGameObjectsWithTag("deer");
        bears = GameObject.FindGameObjectsWithTag("Bear");
        snakes = GameObject.FindGameObjectsWithTag("Snake");
        chunks = FindObjectsOfType<Terrain>();
        for (int i = 0; i < chunks.Length; i++)
        {
            chunks[i].terrainData.wavingGrassSpeed = 0;
            chunks[i].terrainData.wavingGrassStrength = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > nextFire)
        {
            ChunkChecker();
        }
    }

    void ChunkChecker()
    {
        nextFire = Time.time + 1;
        playerPos = this.gameObject.transform.position;
        for(int i = 0; i < chunks.Length; i++)
        {
            if(Vector3.Distance(chunks[i].transform.position, playerPos) > renderDistance)
            {
                chunks[i].gameObject.SetActive(false);
            }
            else
            {
                chunks[i].gameObject.SetActive(true);
            }
        }
        for(int i = 0; i < bears.Length; i++)
        {
            if (Vector3.Distance(bears[i].transform.position, playerPos) > 130)
            {
                bears[i].SetActive(false);
            }
            else if(bears[i].GetComponent<BearAi>().isDead != true)
            {
                bears[i].SetActive(true);
            }
        }
        for(int i = 0; i < deers.Length; i++)
        {
            if (Vector3.Distance(deers[i].transform.position, playerPos) > 130)
            {
                deers[i].SetActive(false);
            }
            else if (deers[i].GetComponent<DeerAi>().isDead != true)
            {
                deers[i].SetActive(true);
            }
        }
        /*
        for (int i = 0; i < snakes.Length; i++)
        {
            if (Vector3.Distance(snakes[i].transform.position, playerPos) > 130)
            {
                snakes[i].SetActive(false);
            }
            else if (snakes[i].GetComponent<Snake>().state != Snake.actionState.Die)
            {
                snakes[i].SetActive(true);
            }
        }
        */
    }

    public Terrain[] GetChunks()
    {
        return chunks;
    }
    
}
