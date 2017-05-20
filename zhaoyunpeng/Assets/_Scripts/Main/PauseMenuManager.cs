﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    Player m_player;

    public GameObject objs;
    public GameObject PauseMenu;
    public GameObject quitWindow;
    public GameObject ButtonAudio;
    public GameObject BGMAudio;
    public GameObject Highlighted;

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
        //Debug.Log(menuTimer);
        if (Input.GetKeyUp(KeyCode.Escape) && PauseMenu.active == false && menuTimer == 0)
        {
            Time.timeScale = 0;                             // 暂停
            //m_player.m_transform.rotation

            BGMAudio.GetComponent<AudioSource>().Pause();
            Screen.lockCursor = false;
            PauseMenu.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Escape) && PauseMenu.active == true && quitWindow.active == false && menuTimer == 0)
        {

            Time.timeScale = 1;                             // 继续

            BGMAudio.GetComponent<AudioSource>().Play();
            Screen.lockCursor = true;
            PauseMenu.SetActive(false);
        }
        if (Input.GetKeyUp(KeyCode.Escape) && quitWindow.active == true && menuTimer == 0)
        {
            GetComponent<quit>().QuitMenuClose();
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
        BGMAudio.GetComponent<AudioSource>().Play();
        foreach (Transform child in objs.transform)
        {
            //Debug.Log(child.GetComponent<Button>().interactable);
            child.GetComponent<Button>().interactable = false;
        }

        Time.timeScale = 1;                             // 继续

        Screen.lockCursor = true;
        PauseMenu.SetActive(false);

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
        Time.timeScale = 1;                             // 继续

        StartCoroutine(WaitAndPrint(1));
        
    }

    IEnumerator WaitAndPrint(float waitTime)
    {

        yield return new WaitForSeconds(waitTime);
        //等待之后执行的动作

        SceneManager.LoadScene("Menu");
    }

}
