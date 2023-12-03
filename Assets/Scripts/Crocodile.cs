using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crocodile : MonoBehaviour
{
    public GameManager manager;
    public Animator anim;


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            manager.animalDie();
            Destroy(collision.gameObject);
            Destroy(GameObject.FindGameObjectWithTag("Croc"));
        }
    }
}
