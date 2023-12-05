using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crocodile : MonoBehaviour
{
    public GameManager manager;
    public Animator anim;
    public CrocodileDieScript die;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            die.isDead = true;
            manager.animalDie();
            Destroy(collision.gameObject);
        }
    }
}
