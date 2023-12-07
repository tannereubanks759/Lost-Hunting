using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    private float seconds = 0f;
    private float minutes = 0f;
    public float animalsLeft = 5f;
    private string timeingame = "";
    public CharacterControllerScript player;
    public GameObject WindEnd;
    public GameObject SnowEnd;
    public GameObject RainEnd;
    public GameObject WinScreen;

    public TextMeshProUGUI text;
    public TextMeshProUGUI animalsKilledText;

    private float secondNextTime;

    public AudioSource[] sound;

    private RingManager rings;

    private float nextAudioCue = 0;

    public GameObject[] bears;
    public GameObject BearAudioCue;

    int smallestDistanceBear = 0;

    public Animator camAnim;

    // Start is called before the first frame update
    void Start()
    {
        BulletScript.windForce = 5f;
        bears = GameObject.FindGameObjectsWithTag("Bear");
        sound = FindObjectsOfType<AudioSource>();
        WinScreen.SetActive(false);
        text.text = "00:00";
        rings = player.gameObject.GetComponent<RingManager>();

        for (int i = 0; i < bears.Length; i++)
        {
            if (bears[i].GetComponent<BearAi>().isDead != true)
            {
                smallestDistanceBear = i;
                return;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= secondNextTime)
        {
            for (int i = 0; i < bears.Length; i++)
            {
                if (bears[i].GetComponent<BearAi>().isDead == false && Vector3.Distance(bears[i].transform.position, player.transform.position) < Vector3.Distance(bears[smallestDistanceBear].transform.position, player.transform.position))
                {
                    smallestDistanceBear = i;
                }
            }

            seconds += 1;
            if(seconds == 60)
            {
                nextAudioCue = Random.Range(1, 59);
                minutes += 1;
                seconds = 0;
            }
            timeingame = minutes.ToString("00") + ":" + seconds.ToString("00");
            text.text = timeingame;
            secondNextTime = Time.time + 1f;
        }
        


        if(nextAudioCue > 0)
        {
            if(seconds == nextAudioCue)
            {
                Instantiate(BearAudioCue, bears[smallestDistanceBear].transform.position, Quaternion.identity);
                nextAudioCue = 0;
            }
        }
    }

    public void animalDie()
    {
        camAnim.SetBool("UpdateNotepad", true);
        
        animalsLeft -= 1;
        animalsKilledText.text = "Killed: " + (5 - animalsLeft) + "/5";
        if(animalsLeft <= 0)
        {
            camAnim.SetBool("win", true);
            
        }
    }
    
    public void WinStart()
    {
        player.isPaused = true;
        player.cursorEnable();
        for (int i = 0; i < sound.Length; i++)
        {
            sound[i].Stop();
        }
    }
    public void WinEnd()
    {
        WinScreen.SetActive(true);
        if (rings.hasRainRing)
        {
            RainEnd.SetActive(true);
        }
        if (rings.hasSnowRing)
        {
            SnowEnd.SetActive(true);
        }
        if (rings.hasWindRing)
        {
            WindEnd.SetActive(true);
        }
    }


}
