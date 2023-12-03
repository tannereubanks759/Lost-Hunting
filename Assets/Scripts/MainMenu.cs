using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public GameObject Main;
    public GameObject Controls;
    public GameObject Tutorial;

    public AudioSource source;
    public AudioClip select;

    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        Main.SetActive(true);
        Controls.SetActive(false);
        Tutorial.SetActive(false);
        anim.SetBool("Fade", false);
    }

    

    public void LoadLevel(string name)
    {
        Select();
        SceneManager.LoadScene(name);
    }
    public void Quit()
    {
        Select();
        Application.Quit();
    }
    public void ToMenu()
    {
        Select();
        Controls.SetActive(false);
        Main.SetActive(true);
        Tutorial.SetActive(false);
    }
    public void ControlsMenu()
    {
        Select();
        Main.SetActive(false);
        Controls.SetActive(true);
    }
    public void TutorialMenu()
    {
        Select();
        Main.SetActive(false);
        Tutorial.SetActive(true);
    }
    public void Select()
    {
        source.PlayOneShot(select, 1f);
    }
    public void LoadGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void PlayButton()
    {
        anim.SetBool("Fade", true);
    }
}
