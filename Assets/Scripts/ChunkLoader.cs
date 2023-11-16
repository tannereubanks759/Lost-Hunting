using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkLoader : MonoBehaviour
{
    private Terrain[] chunks;
    Vector3 playerPos;

    private float nextFire;
    public GameObject[] bears;
    // Start is called before the first frame update
    void Start()
    {

        bears = GameObject.FindGameObjectsWithTag("Bear");
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
            if(Vector3.Distance(chunks[i].transform.position, playerPos) > 150)
            {
                chunks[i].gameObject.SetActive(false);
            }
            else
            {
                chunks[i].gameObject.SetActive(true);
            }
        }
        for(int i = 0; i < 5; i++)
        {
            if (Vector3.Distance(bears[i].transform.position, playerPos) > 100)
            {
                bears[i].SetActive(false);
            }
            else if(bears[i].GetComponent<BearAi>().isDead != true)
            {
                bears[i].SetActive(true);
            }
        }
    }

    public Terrain[] GetChunks()
    {
        return chunks;
    }
    
}
