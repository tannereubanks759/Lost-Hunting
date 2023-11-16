using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UI : MonoBehaviour
{
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        if(player != null)
        {
            player.GetComponent<CharacterControllerScript>().isPaused = false;
            player.GetComponent<CharacterControllerScript>().pauseMenu.SetActive(false);
            player.GetComponent<RingManager>().RainPanel.SetActive(false);
            player.GetComponent<RingManager>().SnowPanel.SetActive(false);
            player.GetComponent<RingManager>().WindPanel.SetActive(false);
        }
    }
}
