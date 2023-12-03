using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingManager : MonoBehaviour
{
    public bool hasSnowRing = false;
    public bool hasRainRing = false;
    public bool hasWindRing = false;

    private CharacterControllerScript player;

    public AudioSource source;

    public GameObject RainPanel;
    public GameObject SnowPanel;
    public GameObject WindPanel;

    public bool panelOpen = false;

    public GameObject Croc;

    // Start is called before the first frame update
    void Start()
    {
        player = this.GetComponent<CharacterControllerScript>();
        Croc.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "RainRing")
        {
            RenderSettings.ambientIntensity = 1f;
            player.BackWalk.Stop();
            player.BackRun.Stop();
            panelOpen = true;
            RainPanel.SetActive(true);
            cursorEnable();
            Destroy(other.gameObject);
            RainRing();
        }
        else if(other.tag == "WindRing") 
        {
            player.BackWalk.Stop();
            player.BackRun.Stop();
            panelOpen = true;
            WindPanel.SetActive(true);
            cursorEnable();
            Destroy(other.gameObject);
            WindRing();
        }
        else if(other.tag == "SnowRing")
        {
            player.BackWalk.Stop();
            player.BackRun.Stop();
            SnowPanel.SetActive(true);
            cursorEnable();
            Destroy(other.gameObject);
            SnowRing();
        }
    }
    void RainRing()
    {
        Croc.SetActive(true);
        source.Play();
        player.rainSystem.gameObject.SetActive(false);
        player.inRain = false;
        hasRainRing = true;
        ChunkLoader loader = this.GetComponent<ChunkLoader>();
        Terrain[] chunks = loader.GetChunks();
        for(int i = 0; i < chunks.Length; i++)
        {
            TerrainScript tscript = chunks[i].GetComponent<TerrainScript>();
            if (tscript.isRain == true)
            {
                tscript.isRain = false;
            }
        }

    }
    void SnowRing()
    {
        player.inSnow = false;
        player.SnowSound.Stop();
        source.Play();
        hasSnowRing = true;
        ChunkLoader loader = this.GetComponent<ChunkLoader>();
        Terrain[] chunks = loader.GetChunks();
        for (int i = 0; i < chunks.Length; i++)
        {
            TerrainScript tscript = chunks[i].GetComponent<TerrainScript>();
            if (tscript.isSnow == true)
            {
                tscript.isSnow = false;
            }
        }
    }
    void WindRing()
    {
        source.Play();
        BulletScript.windForce = 0f;
    }
    public void cursorEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        player.isPaused = true;
    }
}
