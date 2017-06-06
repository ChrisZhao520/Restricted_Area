﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class PauseMenuManager : MonoBehaviour
{
    Player m_player;

    public GameObject objs;
    public GameObject PauseMenu;
    public GameObject quitWindow;
    public GameObject ButtonAudio;
    public GameObject BGMAudio;
    public GameObject Highlighted;
    public GameObject waterAudio;
    public float menuTimer = 0;

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
        if (Input.GetKeyDown(KeyCode.Escape) && PauseMenu.active == false && menuTimer == 0)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;                             // 暂停
            m_player.enabled = false;
            waterAudio.SetActive(false);
            BGMAudio.GetComponent<AudioSource>().Pause();
            PauseMenu.SetActive(true);
            //Debug.Log("暂停");
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && PauseMenu.active == true && quitWindow.active == false && menuTimer == 0)
        {
            if (m_player.m_backpack.GetComponent<Canvas>().enabled == false)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else 
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            /*if (Cursor.lockState == CursorLockMode.Locked)
            {
                Debug.Log("继续2");
            }*/
            m_player.enabled = true;
            waterAudio.SetActive(true);
            BGMAudio.GetComponent<AudioSource>().Play();
            Time.timeScale = 1;                             // 继续
            PauseMenu.SetActive(false);
            //Debug.Log("继续1");
        }
        if (Input.GetKeyDown(KeyCode.Escape) && quitWindow.active == true && menuTimer == 0)
        {
            GetComponent<quit>().QuitMenuClose();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && PauseMenu.active == true && menuTimer != 0)
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

        if (m_player.m_backpack.GetComponent<Canvas>().enabled == false)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        m_player.enabled = true;
        waterAudio.SetActive(true);

        Time.timeScale = 1;                             // 继续

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
        foreach (Transform child in quitWindow.transform)
        {
            if (child.GetComponent<Button>())
            {
                //Debug.Log(child.GetComponent<Button>().interactable);
                child.GetComponent<Button>().interactable = false;
            }
        }
        gameObject.GetComponent<PauseMenuManager>().enabled = false;

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
