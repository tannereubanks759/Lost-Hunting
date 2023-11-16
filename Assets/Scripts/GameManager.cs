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

    private float secondNextTime;

    public AudioSource[] sound;

    private RingManager rings;
    // Start is called before the first frame update
    void Start()
    {

        sound = FindObjectsOfType<AudioSource>();
        WinScreen.SetActive(false);
        text.text = "00:00";
        rings = player.gameObject.GetComponent<RingManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= secondNextTime)
        {
            seconds += 1;
            if(seconds == 60)
            {
                minutes += 1;
                seconds = 0;
            }
            timeingame = minutes.ToString("00") + ":" + seconds.ToString("00");
            text.text = timeingame;
            secondNextTime = Time.time + 1f;
        }
        
    }

    public void animalDie()
    {
        animalsLeft -= 1;
        if(animalsLeft <= 0)
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
            player.isPaused = true;
            player.cursorEnable();
            for(int i = 0; i < sound.Length; i++)
            {
                sound[i].Stop();
            }
            Time.timeScale = 0f;
        }
    }
}
