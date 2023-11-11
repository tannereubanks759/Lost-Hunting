using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BearAi : MonoBehaviour
{
    public Animator bearAnim;
    public enum actionState { Running, Attacking, Idle, Die }
    public actionState state;

    private NavMeshAgent agent;
    private GameObject player;
    public float wanderDistance = 40f;
    public bool inRange;

    public bool hasDirection = false;

    Vector3 randomPosition;

    public bool isDead = false;
    float distanceFromPlayer;


    private float attackingNextTIme;
    private bool hasAttackingTime;

    public AudioSource bearSound;
    public AudioClip AttackClip;
    public AudioClip DieClip;
    public AudioClip AggroClip;
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
        switch (state)
        {
            case actionState.Attacking:
                Attacking();
                break;
            case actionState.Idle:
                Idle();
                break;
            case actionState.Running:
                Running();
                break;
            case actionState.Die:
                Die();
                break;
        }
        if(isDead == true)
        {
            state = actionState.Die;
        }
        else if (distanceFromPlayer <= 20f && player.GetComponent<CharacterControllerScript>().isDead == false)
        {
            state = actionState.Attacking;
        }
        if (player == null || distanceFromPlayer > 180f)
        {
            state = actionState.Idle;
        }

    }
    void Attacking()
    {
        if(inRange ==false)
        {
            bearSound.PlayOneShot(AggroClip, 1f);
        }
        inRange = true;
        bearAnim.SetBool("isRunning", true);
        agent.SetDestination(player.transform.position);
        if(distanceFromPlayer < 2.5)
        {
            if (!hasAttackingTime)
            {
                attackingNextTIme = Time.time + 1.1f;
                hasAttackingTime = true;
            }
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            bearAnim.SetBool("isAttacking", true);
            bearAnim.SetBool("isRunning", false);
            
            if(Time.time > attackingNextTIme)
            {
                bearSound.PlayOneShot(AttackClip, 1f);
                hit();
            }
        }
        else
        {
            bearAnim.SetBool("isRunning", true);
            bearAnim.SetBool("isAttacking", false);
            agent.isStopped = false;
            hasAttackingTime = false;
        }

        
    }
    void Die()
    {
        if(agent.isStopped == false)
        {
            bearSound.PlayOneShot(DieClip, .5f);
        }
        agent.isStopped = true;
        bearAnim.SetBool("isDead", true);
    }
    void Running()
    {
        if(hasDirection == false)
        {
            Vector3 randomDirection = Random.insideUnitSphere; // Random direction vector
            randomDirection.y = 0; // Make it stay in the same plane (2D if you want)
            randomDirection.Normalize(); // Normalize the vector
            randomPosition = this.transform.position + randomDirection * wanderDistance;
            hasDirection = true;
        }
        if(agent.transform.position.x != randomPosition.x && agent.transform.position.z != randomPosition.z)
        {
            bearAnim.SetBool("isRunning", true);
            agent.isStopped = false;
            agent.SetDestination(randomPosition);
        }
        else
        {
            state = actionState.Idle;
        }
        
    }
    void Idle()
    {
        
        bearAnim.SetBool("isRunning", false);
        bearAnim.SetBool("isAttacking", false);
        agent.isStopped = true;
    }
    public void heardNoise()
    {
        if (!isDead)
        {
            hasDirection = false;
            state = actionState.Running;
        }
    }
    public void hit()
    {
        if(distanceFromPlayer < 2.5)
        {
            player.GetComponent<CharacterControllerScript>().die();
            state = actionState.Idle;
        }

        hasAttackingTime = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Terrain")
        {
            other.GetComponent<TerrainScript>().hasBear = true;
            Debug.Log("HasBear Set True");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.tag == "Terrain")
        {
            other.GetComponent<TerrainScript>().hasBear = false;
            Debug.Log("Has Bear Set False");
        }
    }
}
