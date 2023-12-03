using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crocodile : MonoBehaviour
{
    public GameManager manager;
    public bool isDead;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        isDead = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet" && isDead == false)
        {
            isDead = true;
            anim.SetBool("isDead", true);
            manager.animalDie();
        }
    }
}
