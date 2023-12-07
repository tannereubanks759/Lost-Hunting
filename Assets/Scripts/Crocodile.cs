using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crocodile : MonoBehaviour
{
    public GameManager manager;
    public Animator anim;
    public CrocodileDieScript die;
    public AudioClip crocDie;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            this.GetComponent<AudioSource>().PlayOneShot(crocDie, 1f);
            die.isDead = true;
            manager.animalDie();
            Destroy(collision.gameObject);
        }
    }
}
