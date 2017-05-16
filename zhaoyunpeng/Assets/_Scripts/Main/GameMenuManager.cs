using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour
{
    Player m_player;

    public GameObject objs;
    public GameObject PauseMenu;
    public GameObject ButtonAudio;
    public GameObject Highlighted;
    public AudioClip NoneClip;
    public AudioClip gunClip;

    public float menuTimer;

    private AudioSource ButtonAudioSource;
    
	void Start () 
    {
        ButtonAudioSource = ButtonAudio.GetComponent<AudioSource>();
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}

    void Update()
    {
        menuTimer = GetComponent<panel_manager>().timer;
        Debug.Log(menuTimer);
        if (Input.GetKeyUp(KeyCode.Escape) && PauseMenu.active == false && menuTimer == 0)
        {
            //Time.timeScale = 0;
            m_player.m_audio = NoneClip;

            
            Screen.lockCursor = false;
            PauseMenu.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Escape) && PauseMenu.active == true && menuTimer == 0)
        {
            m_player.m_audio = gunClip;

            
            Screen.lockCursor = true;
            PauseMenu.SetActive(false);
        }
        else if (Input.GetKeyUp(KeyCode.Escape) && PauseMenu.active == true && menuTimer != 0)
        {

            if (menuTimer == 1)
            {
                GetComponent<panel_manager>().closedload();
            }
            if (menuTimer == 2)
            {
                GetComponent<panel_manager>().closedsave();
            }
            if (menuTimer == 3)
            {
                GetComponent<panel_manager>().closedset();
            }
            if (menuTimer == 4)
            {
                GetComponent<panel_manager>().closedhelp();
            }
            if (menuTimer == 5)
            {
                GetComponent<panel_manager>().closedabout();
            }
        }
    }


    public void RtnGame()
    {
        ButtonAudioSource.Play();
        foreach (Transform child in objs.transform)
        {
            //Debug.Log(child.GetComponent<Button>().interactable);
            child.GetComponent<Button>().interactable = false;
        }

        Screen.lockCursor = true;
        PauseMenu.SetActive(false);

        m_player.m_audio = gunClip;

        foreach (Transform child in objs.transform)
        {
            //Debug.Log(child.GetComponent<Button>().interactable);
            child.GetComponent<Button>().interactable = true;
        }
        Highlighted.SetActive(false);
    }

    public void RtnMenu()
    {
        ButtonAudioSource.Play();
        foreach (Transform child in objs.transform)
        {
            //Debug.Log(child.GetComponent<Button>().interactable);
            child.GetComponent<Button>().interactable = false;
        }
        StartCoroutine(WaitAndPrint(1));
        
    }

    IEnumerator WaitAndPrint(float waitTime)
    {

        yield return new WaitForSeconds(waitTime);
        //等待之后执行的动作

        SceneManager.LoadScene("Menu");
    }

}
