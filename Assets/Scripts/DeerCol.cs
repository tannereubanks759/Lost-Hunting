using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerCol : MonoBehaviour
{
    public DeerAi deer;
    private GameManager manager;
    public float graceperiod = 2f;
    private static float nextTime;
    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<GameManager>();
        nextTime = 0;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet" && deer.isDead != true && Time.time > nextTime)
        {
            Destroy(collision.gameObject);
            manager.animalDie();
            deer.isDead = true;
            nextTime = Time.time + graceperiod;
        }
    }
}
