using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Snail : MonoBehaviour
{

    private NavMeshAgent agent;
    private CharacterControllerScript player;

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterControllerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.transform.position);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            player.cam.transform.rotation = player.transform.rotation;
            player.die();
        }
    }
}
