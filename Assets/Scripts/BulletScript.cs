using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float bulletSpeed;
    public Rigidbody rb;

    Vector3 WindDirection;
    public float windForce;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.forward * bulletSpeed;
        rb.velocity = (rb.velocity + (new Vector3(1, -.3f, 0) * windForce));
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit");
        Destroy(this.gameObject);
    }
}
