using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearCol : MonoBehaviour
{
    public BearAi bear;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            bear.isDead = true;
        }
    }
}
