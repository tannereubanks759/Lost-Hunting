using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Snake : MonoBehaviour
{
    public Animator anim;
    public enum actionState { Attacking, Idle, Die }
    public actionState state;

    private NavMeshAgent agent;
    private GameObject player;
    public float wanderDistance = 150f;
    public bool inRange;

    public bool hasDirection = false;

    Vector3 randomPosition;

    public bool isDead = false;
    float distanceFromPlayer;

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        state = actionState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        distanceFromPlayer = Vector3.Distance(player.transform.position, agent.transform.position);

        if (isDead == true)
        {
            state = actionState.Die;
        }

        switch (state)
        {
            case actionState.Attacking:
                AttackPlayer(distanceFromPlayer);
                break;
            case actionState.Idle:
                Idle(distanceFromPlayer);
                break;
            case actionState.Die:
                Die();
                break;
        }

        if (player == null || distanceFromPlayer > 180f)
        {
            state = actionState.Idle;
        }
    }

    void Idle(float distance)
    {
        anim.SetBool("Move", false);
        anim.SetBool("Attack", false);
        agent.isStopped = true;
        CharacterControllerScript playerController = player.GetComponent<CharacterControllerScript>();
        if (distance < 40)
        {
            if(this.GetComponent<AudioSource>().isPlaying == false)
            {
                this.GetComponent<AudioSource>().Play();
            }
            state = actionState.Attacking;
        }
    }

    void AttackPlayer(float distance)
    {

        anim.SetBool("Move", true);
        agent.isStopped = false;
        agent.SetDestination(player.transform.position);
        if (distance < 1.5f)
        {
            player.GetComponent<CharacterControllerScript>().die();
        }
        else if (distance > 60f)
        {
            state = actionState.Idle;
        }
    }

    void Die()
    {

        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        anim.SetBool("Die", true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet" && state != actionState.Die)
        {
            Destroy(collision.gameObject);
            state = actionState.Die;
        }
    }
}
