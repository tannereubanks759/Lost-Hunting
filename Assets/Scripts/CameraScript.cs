using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Camera cam;
    public GameObject rifle;
    public GameObject image;

    public GameObject boatPrebab;
    public GameObject boatrot;

    public GameManager manager;

    public ChunkLoader loader;
    
    // Start is called before the first frame update
    void Start()
    {
        loader.renderDistance = 150f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Zoom()
    {
        cam.fieldOfView = 20;
    }
    
    public void UnZoom()
    {
        cam.fieldOfView = 60;
    }
    public void DoneAiming()
    {
        rifle.SetActive(true);
        image.SetActive(true);
        this.GetComponent<Animator>().SetBool("doneAiming", true);
    }
    public void DoneAimingFalse()
    {

        this.GetComponent<Animator>().SetBool("doneAiming", false);
    }
    public void BoatDone()
    {
        this.GetComponent<Animator>().SetBool("BoatDone", true);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<CharacterControllerScript>().isPaused = false;
        GameObject.Find("Boat_5").SetActive(false);
        player.GetComponent<CharacterControllerScript>().cursorDisable();
        player.GetComponent<CharacterControllerScript>().GunSource.PlayOneShot(player.GetComponent<CharacterControllerScript>().reload, 1f);
        Instantiate(boatPrebab, new Vector3(.006f, -.03999996f, 237.72f), boatrot.transform.rotation);
    }

    public void WinDone()
    {
        manager.WinEnd();
        Time.timeScale = 0f;
    }
    public void WinAnimBegin()
    {
        manager.WinStart();
        RenderSettings.fog = false;
        loader.renderDistance = 700f;
    }
}

