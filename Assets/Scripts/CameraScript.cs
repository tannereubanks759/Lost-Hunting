using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Camera cam;
    public GameObject rifle;
    public GameObject image;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Zoom()
    {
        cam.fieldOfView = 20;
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
}

