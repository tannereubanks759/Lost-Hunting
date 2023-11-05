using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerScript : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    private Vector3 moveInput;
    private Vector3 moveDirection;
    public float walkSpeed;
    float moveSpeed;
    private float idleSpeed;
    private CharacterController controller;
    private Vector3 Velocity;
    public float jumpForce;
    public KeyCode SprintKey;
    float sprintSpeed;

    public float sensX;
    public float sensY;
    float aimSensX;
    float aimSensY;
    public GameObject cam;
    float mouseX;
    float mouseY;
    float multiplier = .01f;
    float xRotation;
    float yRotation;
    public GameObject cameraPos;

    public bool isPaused = false;


    public bool isSprinting;
    private bool isAiming = false;
    public KeyCode ShootKey;
    public Animator camAnim;
    public KeyCode AimKey;

    public Camera mainCam;
    public GameObject barrelEnd;
    public GameObject bullet;

    public float RateOfFire = 2f;
    public float nextFire;
    public float ShotSoundDistance = 100f;

    public GameObject[] bears;
    public GameObject[] dears;

    public bool isDead = false;
    public GameObject deathMenu;

    private float attackingNextTIme;
    //public GameObject rifle;
    //public GameObject scopeImage;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        cursorDisable();
        moveSpeed = walkSpeed;
        sprintSpeed = moveSpeed * 1.5f;
        isSprinting = false;
        aimSensX = sensX * .2f;
        aimSensY = sensY * .2f;
        nextFire = Time.time;
        bears = GameObject.FindGameObjectsWithTag("Bear");
    }

    // Update is called once per frame
    void Update()
    {
        
        //movement
        if (isAiming == false && isDead != true)
        {
            //sprinting
            if (Input.GetKey(SprintKey))
            {
                isSprinting = true;
                moveSpeed = sprintSpeed;
            }
            else
            {
                isSprinting = false;
                moveSpeed = walkSpeed;
            }


            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
            moveDirection = transform.forward * vertical + transform.right * horizontal;
            if (controller.isGrounded && Input.GetKey(KeyCode.Space))
            {
                Velocity.y = jumpForce;
            }
            else
            {
                Velocity.y -= -9.81f * -2f * Time.deltaTime;
            }
            controller.SimpleMove((moveDirection).normalized * moveSpeed);
            controller.Move(Velocity * Time.deltaTime);
        }
        
        //mouse Looking
        if (isPaused == false && isDead != true)
        {
            mouseX = Input.GetAxisRaw("Mouse X");
            mouseY = Input.GetAxisRaw("Mouse Y");
            yRotation += mouseX * sensX * multiplier;
            xRotation -= mouseY * sensY * multiplier;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            cam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
            transform.rotation = Quaternion.Euler(0, yRotation, 0);
            cam.transform.position = cameraPos.transform.position;
        }



        //Aiming
        if(!isPaused && !isDead)
        {
            if (Input.GetKey(AimKey) && isSprinting == false)
            {
                isAiming = true;
                camAnim.SetBool("isAiming", true);
                sensY = aimSensY;
                sensX = aimSensX;
                if (Input.GetKeyDown(ShootKey) && Time.time > nextFire)
                {
                    Shoot();
                    nextFire = Time.time + RateOfFire;
                }
            }
            else if (Input.GetKeyUp(AimKey))
            {
                isAiming = false;
                camAnim.SetBool("isAiming", false);
                sensX *= 5;
                sensY *= 5;
                mainCam.fieldOfView = 60;
            }
            else
            {
                mainCam.fieldOfView = 60;
            }
        }


        if (isDead)
        {
            deathMenu.SetActive(true);
            cursorEnable();
        }
        
    }

    public void cursorDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void cursorEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void Shoot()
    {
        Instantiate(bullet, barrelEnd.transform.position, barrelEnd.transform.rotation);
        for(int i = 0; i < bears.Length; i++)
        {
            if(Vector3.Distance(bears[i].transform.position, this.gameObject.transform.position) < ShotSoundDistance && bears[i].GetComponent<BearAi>().isDead != true)
            {
                bears[i].GetComponent<BearAi>().heardNoise();
            }
        }
    }
    public void die()
    {
        isDead = true;
    }
}
