using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour
{
    Player m_player;

    public GameObject objs;
    public GameObject quitWindow;
    public GameObject ButtonAudio;
    public GameObject Highlighted;
    public float menuTimer;

    private AudioSource ButtonAudioSource;
    
	void Start () 
    {
        Screen.lockCursor = false;
        ButtonAudioSource = ButtonAudio.GetComponent<AudioSource>();
        
	}

    void Update()
    {
        menuTimer = GetComponent<panel_manager>().timer;
        //Debug.Log(menuTimer);
        if (Input.GetKeyUp(KeyCode.Escape) && quitWindow.active == false && menuTimer == 0)
        {
            GetComponent<quit>().QuitMenuOpen();
        }
        else if (Input.GetKeyUp(KeyCode.Escape) && quitWindow.active == true && menuTimer == 0)
        {
            GetComponent<quit>().QuitMenuClose();
        }
        else if (Input.GetKeyUp(KeyCode.Escape) && menuTimer != 0 && quitWindow.active == false)
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
}
