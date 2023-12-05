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
    public float snowSpeed;
    public float snowSprintSpeed;

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
    private bool inChamber = true;

    public GameObject[] bears;
    public GameObject[] dears;

    public bool isDead = false;
    public GameObject deathMenu;
    public GameObject pauseMenu;


    public ParticleSystem rainSystem;
    public AudioSource SnowSound;

    public AudioSource GunSource;
    public AudioClip gunshot;
    public AudioClip reload;
    public AudioClip gunClick;

    public AudioSource BackWalk;
    public AudioSource BackRun;

    public bool inRain;
    public bool inSnow;

    public AudioSource stickAudio;
    public GameObject[] deers;

    public bool onBoat = true;

    public Light light1;

    public Material DaySkybox;
    public Material NightSkybox;

    public GameObject flashlight;
    public GameObject Dirt;


    Color newCol;
    //public GameObject rifle;
    public GameObject scopeImage;
    // Start is called before the first frame update
    void Start()
    {
        isPaused = true;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        deathMenu.SetActive(false);

        deers = GameObject.FindGameObjectsWithTag("deer");
        controller = GetComponent<CharacterController>();
        cursorDisable();
        moveSpeed = walkSpeed;
        sprintSpeed = moveSpeed * 1.5f;
        isSprinting = false;
        aimSensX = sensX * .2f;
        aimSensY = sensY * .2f;
        nextFire = Time.time;
        bears = GameObject.FindGameObjectsWithTag("Bear");

        snowSpeed = walkSpeed / 2f;
        snowSprintSpeed = (moveSpeed/2f) * 1.5f;

        RenderSettings.fogColor = Color.black;
        light1.intensity = 1f;
        RenderSettings.fogStartDistance = 80f;
        RenderSettings.ambientIntensity = .2f;
        inSnow = false;
        inRain = true;
        RenderSettings.skybox = NightSkybox;
        rainSystem.gameObject.SetActive(true);
        if(flashlight != null)
        {
            flashlight.SetActive(true);
        }
        Dirt.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isPaused && !isDead)
        {
            //movement
            if (isAiming == false)
            {
                //sprinting
                if (Input.GetKey(SprintKey) && (horizontal != 0 || vertical != 0))
                {
                    if (BackRun.isPlaying == false)
                    {
                        BackRun.Play();
                    }
                    if (inSnow)
                    {
                        moveSpeed = snowSprintSpeed;
                    }
                    else
                    {
                        moveSpeed = sprintSpeed;
                    }
                    isSprinting = true;
                    camAnim.SetBool("isRunning", true);
                }
                else
                {
                    BackRun.Stop();
                    isSprinting = false;
                    if (inSnow)
                    {
                        moveSpeed = snowSpeed;
                    }
                    else
                    {
                        moveSpeed = walkSpeed;
                    }
                    camAnim.SetBool("isRunning", false);
                }

                if ((horizontal != 0 || vertical != 0) && isAiming == false)
                {
                    if (BackWalk.isPlaying == false)
                    {
                        BackWalk.Play();
                    }
                }
                else
                {
                    BackWalk.Stop();
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

            //aiming
            if (Input.GetKey(AimKey) && isSprinting == false && controller.isGrounded)
            {
                BackWalk.Stop();
                isAiming = true;
                
                camAnim.SetBool("isAiming", true);
                sensY = aimSensY;
                sensX = aimSensX;
                if (Input.GetKeyDown(ShootKey) && inChamber == true && Time.time > nextFire)
                {
                    GunSource.PlayOneShot(gunshot, 1f);
                    Shoot();
                }
                else if(Input.GetKeyDown(ShootKey) && inChamber == false && Time.time > nextFire)
                {
                    GunSource.PlayOneShot(gunClick, 1f);
                }
            }
            else if (Input.GetKeyUp(AimKey) && isAiming == true)
            {
                isAiming = false;
                camAnim.SetBool("doneAiming", false);
                camAnim.SetBool("isAiming", false);
                scopeImage.SetActive(false);
                sensX *= 5;
                sensY *= 5;
                mainCam.fieldOfView = 60;
            }
            else
            {
                mainCam.fieldOfView = 60;
            }

            //reload
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (inChamber == false)
                {
                    GunSource.PlayOneShot(reload, 1f);
                    nextFire = Time.time + RateOfFire;
                    inChamber = true;
                }
            }
        }
        
        //death check
        if (isDead)
        {
            deathMenu.SetActive(true);
            cursorEnable();
        }


        //pause input
        if(Input.GetKeyDown(KeyCode.Escape) && isDead == false && camAnim.GetBool("BoatDone") == true)
        {
            isPaused = !isPaused;
            if(pauseMenu.activeSelf == true)
            {
                cursorDisable();
                Time.timeScale = 1f;
                pauseMenu.SetActive(false);
            }
            else
            {
                cursorEnable();
                Time.timeScale = 0f;
                pauseMenu.SetActive(true);
            }
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
        for(int i = 0; i < deers.Length; i++)
        {
            if (Vector3.Distance(deers[i].transform.position, this.gameObject.transform.position) < ShotSoundDistance && deers[i].GetComponent<DeerAi>().isDead != true)
            {
                deers[i].GetComponent<DeerAi>().heardNoise();
            }
        }

        inChamber = false;
    }
    public void die()
    {
        BackRun.Stop();
        BackWalk.Stop();
        camAnim.SetBool("isDead", true);
        isDead = true;
    }
    public void waterDie()
    {
        BackRun.Stop();
        BackWalk.Stop();
        camAnim.SetBool("WaterDie", true);
        isDead = true;
    }
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Log");
        if(other.tag == "Terrain" && other.GetComponent<TerrainScript>() != null)
        {
            TerrainScript terrain = other.GetComponent<TerrainScript>();
            if (terrain.isRain == true && rainSystem.isPlaying == false){
                light1.intensity = 1f;
                RenderSettings.fogStartDistance = 80f;
                RenderSettings.fogColor = Color.black;
                RenderSettings.ambientIntensity = .2f;
                inSnow = false;
                inRain = true;
                rainSystem.gameObject.SetActive(true);
                RenderSettings.skybox = NightSkybox;
                flashlight.SetActive(true);
                Dirt.SetActive(false);
                SnowSound.Stop();
            }
            else if(terrain.isSnow == true && SnowSound.isPlaying == false)
            {
                SnowSound.Play();
                RenderSettings.fogColor = Color.white;
                light1.intensity = 5f;
                RenderSettings.ambientIntensity = 1f;
                inRain = false;
                RenderSettings.fogStartDistance = 40f;
                rainSystem.gameObject.SetActive(false);
                inSnow = true;
                RenderSettings.skybox = DaySkybox;
                flashlight.SetActive(false);
                Dirt.SetActive(true);
            }
            else if(terrain.isRain == false && terrain.isSnow == false)
            {
                
                bool bConverted = ColorUtility.TryParseHtmlString("#9BA6AF", out newCol);
                //Did it successfully parse the Hex?
                if (bConverted)
                {
                    RenderSettings.fogColor = newCol;
                }
                SnowSound.Stop();
                light1.intensity = 5f;
                RenderSettings.ambientIntensity = 1f;
                inRain = false;
                inSnow = false;
                RenderSettings.fogStartDistance = 80f;
                rainSystem.gameObject.SetActive(false);
                RenderSettings.skybox = DaySkybox;
                flashlight.SetActive(false);
                Dirt.SetActive(false);
            }
        }

        if (other.gameObject.tag == "Water")
        {
            waterDie();
        }

        //sticks
        if (other.tag == "stick")
        {
            stickAudio.Stop();
            stickAudio.Play();

            for (int i = 0; i < bears.Length; i++)
            {
                if (Vector3.Distance(bears[i].transform.position, this.gameObject.transform.position) < 80 && bears[i].GetComponent<BearAi>().isDead != true)
                {
                    bears[i].GetComponent<BearAi>().heardNoise();
                }

            }
            for (int i = 0; i < deers.Length; i++)
            {
                if (Vector3.Distance(bears[i].transform.position, this.gameObject.transform.position) < 80 && bears[i].GetComponent<BearAi>().isDead != true)
                {
                    deers[i].GetComponent<DeerAi>().heardNoise();
                }
            }
        }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Water")
        {
            waterDie();
        }
    }

}
