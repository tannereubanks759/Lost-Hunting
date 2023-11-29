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

    public float SprintingDetectionRange = 50f;
    public float WalkingDetectionRange = 30f;
    public float RainSprintDetectionRange = 70f;
    public float RainWalkingDetectionRange = 50f;

    private bool playDeathSound;
    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        state = actionState.Idle;
        playDeathSound = false;
    }

    // Update is called once per frame
    void Update()
    {
        distanceFromPlayer = Vector3.Distance(player.transform.position, agent.transform.position);

        if (isDead == true)
        {
            state = actionState.Die;
        }
        else if (distanceFromPlayer <= 20f && player.GetComponent<CharacterControllerScript>().isDead == false)
        {
            state = actionState.Attacking;
        }

        switch (state)
        {
            case actionState.Attacking:
                Attacking();
                break;
            case actionState.Idle:
                Idle(distanceFromPlayer);
                break;
            case actionState.Running:
                Running();
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
    void Attacking()
    {
        if(inRange ==false)
        {
            bearSound.PlayOneShot(AggroClip, 1f);
        }
        inRange = true;
        agent.SetDestination(player.transform.position);
        if(distanceFromPlayer < 2.1)
        {
            if (!hasAttackingTime)
            {
                attackingNextTIme = Time.time + .6f;
                hasAttackingTime = true;
            }
            agent.isStopped = true;
            agent.velocity = Vector3.zero;

            bearAnim.SetBool("isRunning", false);
            bearAnim.SetBool("isAttacking", true);
            
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
        
        if(playDeathSound == false)
        {
            bearSound.PlayOneShot(DieClip, .5f);
            playDeathSound = true;
        }
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
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
        if(hasDirection == true && agent.transform.position.x != randomPosition.x && agent.transform.position.z != randomPosition.z)
        {
            bearAnim.SetBool("isRunning", true);
            agent.isStopped = false;
            agent.SetDestination(randomPosition);
        }
        else
        {
            state = actionState.Idle;
        }
        if(randomPosition.x > 230 || randomPosition.x < -230 || randomPosition.z > 230 || randomPosition.z < -230) 
        {
            hasDirection = false;
        }
    }
    void Idle(float distance)
    {
        
        bearAnim.SetBool("isRunning", false);
        bearAnim.SetBool("isAttacking", false);
        agent.isStopped = true;
        CharacterControllerScript playerController = player.GetComponent<CharacterControllerScript>();
        if(distance < 70)
        {
            if (distance < RainSprintDetectionRange && playerController.inRain && playerController.isSprinting == true)
            {
                heardNoise();
            }
            else if (distance < RainWalkingDetectionRange && playerController.inRain && playerController.isSprinting == false)
            {
                heardNoise();
            }
            else if (distance < SprintingDetectionRange && playerController.isSprinting == true)
            {
                heardNoise();
            }
            else if (distance < WalkingDetectionRange && playerController.isSprinting == false)
            {
                heardNoise();
            }
        }
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

    

}
