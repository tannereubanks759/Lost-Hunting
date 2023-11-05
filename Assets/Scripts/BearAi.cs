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

    public bool hasDirection = false;

    Vector3 randomPosition;

    public bool isDead = false;
    float distanceFromPlayer;


    private float attackingNextTIme;
    private bool hasAttackingTime;
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
        else if (distanceFromPlayer <= 20f)
        {
            state = actionState.Attacking;
        }
        if(player == null)
        {
            state = actionState.Idle;
        }

    }
    void Attacking()
    {
        bearAnim.SetBool("isRunning", true);
        agent.SetDestination(player.transform.position);
        if(distanceFromPlayer < 5)
        {
            if (!hasAttackingTime)
            {
                attackingNextTIme = Time.time + 1.1f;
                hasAttackingTime = true;
            }
            bearAnim.SetBool("isAttacking", true);
            bearAnim.SetBool("isRunning", false);
            agent.isStopped = true;
            if(Time.time > attackingNextTIme)
            {
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
        if(distanceFromPlayer < 5)
        {
            player.GetComponent<CharacterControllerScript>().die();
        }
    }
}
