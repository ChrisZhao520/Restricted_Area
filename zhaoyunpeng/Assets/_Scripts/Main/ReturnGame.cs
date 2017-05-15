using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReturnGame : MonoBehaviour {
    public GameObject objs;
    public GameObject PauseMenu;
    public GameObject ButtonAudio;
    public GameObject Highlighted;

    private AudioSource ButtonAudioSource;
    
	void Start () 
    {
        ButtonAudioSource = ButtonAudio.GetComponent<AudioSource>();
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

        foreach (Transform child in objs.transform)
        {
            //Debug.Log(child.GetComponent<Button>().interactable);
            child.GetComponent<Button>().interactable = true;
        }
        Highlighted.SetActive(false);
    }
}
