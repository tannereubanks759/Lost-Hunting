using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class DeerAi : MonoBehaviour
{
    public Animator deerAnim;
    public enum actionState { Running, Walking, Idle, Die }
    public actionState state;

    private NavMeshAgent agent;
    private GameObject player;
    public float wanderDistance = 150f;
    public bool inRange;

    public bool hasDirection = false;

    Vector3 randomPosition;

    public bool isDead = false;
    float distanceFromPlayer;

    public AudioSource deerSound;
    public AudioClip DieClip;


    public float SprintingDetectionRange = 50f;
    public float WalkingDetectionRange = 30f;
    public float RainSprintDetectionRange = 70f;
    public float RainWalkingDetectionRange = 50f;

    private float idleTime = 0;

    private bool playDeathSound;
    // Start is called before the first frame update
    void Start()
    {
        idleTime = 0f;
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

        switch (state)
        {
            case actionState.Walking:
                Walking();
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

    void Idle(float distance)
    {
        if(idleTime == 0f)
        {
            idleTime = Time.time + Random.Range(2, 6);
        }
        if(Time.time > idleTime)
        {
            idleTime = 0;
            hasDirection = false;
            state = actionState.Walking;
        }
        deerAnim.SetBool("isRunning", false);
        deerAnim.SetBool("isWalking", false);
        agent.isStopped = true;
        CharacterControllerScript playerController = player.GetComponent<CharacterControllerScript>();
        if (distance < 70 && player.GetComponent<CharacterController>().velocity != Vector3.zero)
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

    void Walking()
    {
        agent.speed = 3;
        deerAnim.SetBool("isRunning", false);
        if (hasDirection == false)
        {
            Vector3 randomDirection = Random.insideUnitSphere; // Random direction vector
            randomDirection.y = 0; // Make it stay in the same plane (2D if you want)
            randomDirection.Normalize(); // Normalize the vector
            randomPosition = this.transform.position + randomDirection * Random.Range(10, 20);
            hasDirection = true;
        }
        if (hasDirection == true && agent.transform.position.x != randomPosition.x && agent.transform.position.z != randomPosition.z)
        {
            deerAnim.SetBool("isWalking", true);
            agent.isStopped = false;
            agent.SetDestination(randomPosition);
        }
        else
        {
            state = actionState.Idle;
        }
        if (randomPosition.x > 230 || randomPosition.x < -230 || randomPosition.z > 230 || randomPosition.z < -230)
        {
            hasDirection = false;
        }
    }
    void Running()
    {
        agent.speed = 20;
        deerAnim.SetBool("isWalking", false);
        if (hasDirection == false)
        {
            Vector3 randomDirection = Random.insideUnitSphere; // Random direction vector
            randomDirection.y = 0; // Make it stay in the same plane (2D if you want)
            randomDirection.Normalize(); // Normalize the vector
            randomPosition = this.transform.position + randomDirection * wanderDistance;
            hasDirection = true;
        }
        if (hasDirection == true && agent.transform.position.x != randomPosition.x && agent.transform.position.z != randomPosition.z)
        {
            deerAnim.SetBool("isRunning", true);
            agent.isStopped = false;
            agent.SetDestination(randomPosition);
        }
        else
        {
            state = actionState.Idle;
        }
        if (randomPosition.x > 230 || randomPosition.x < -230 || randomPosition.z > 230 || randomPosition.z < -230)
        {
            hasDirection = false;
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

    void Die()
    {

        if (playDeathSound == false)
        {
            deerSound.PlayOneShot(DieClip, .5f);
            playDeathSound = true;
        }
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        deerAnim.SetBool("isDead", true);
    }
}
