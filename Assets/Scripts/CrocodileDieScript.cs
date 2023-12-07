using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocodileDieScript : MonoBehaviour
{
    public bool isDead;
    public float nextTime;
    public float speed = 5f;
    public bool begStop;

    public float distance;
    public bool isSpawned;
    // Start is called before the first frame update
    void Start()
    {
        isSpawned = false;
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            this.transform.position = this.transform.position + (Vector3.down * Time.deltaTime * speed);
            /*
            if (Time.time > nextTime)
            {
                this.gameObject.SetActive(false);
            }
            */
        }
        
    }

    void Die()
    {
        nextTime = Time.time + 10f;
        this.gameObject.GetComponent<Animator>().enabled = false;
        isDead = true;
    }
}
