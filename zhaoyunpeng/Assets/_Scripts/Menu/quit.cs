using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class quit : MonoBehaviour {
    public GameObject objs;
    public GameObject quitWindow;
    public GameObject ButtonAudio;
    public GameObject backgroundBlack;

    private AudioSource ButtonAudioSource;

    void Start()
    {
        ButtonAudioSource = ButtonAudio.GetComponent<AudioSource>();
    }

    void Update()
    {
       
    }
    public void QuitMenuOpen()
    {
        //Debug.Log("QuitMenuOpen");

        ButtonAudioSource.Play();
        quitWindow.SetActive(true);
        backgroundBlack.GetComponent<Image>().enabled = true;
        foreach (Transform child in objs.transform)
        {
            //Debug.Log(child.GetComponent<Button>().interactable);
            child.GetComponent<Button>().interactable = false;
            foreach (Transform childchild in child.transform)
            {
                if (childchild.GetComponent<ActiveSystem>())
                {
                    childchild.GetComponent<ActiveSystem>().enabled = false;
                }
                else if (childchild.GetComponent<Image>())
                {
                    childchild.gameObject.SetActive(false);
                }
            }
        }
    }

    public void QuitMenuClose()
    {
        //Debug.Log("QuitMenuClose");

        ButtonAudioSource.Play();
        quitWindow.SetActive(false);
        backgroundBlack.GetComponent<Image>().enabled = false;
        foreach (Transform child in objs.transform)
        {
            //Debug.Log(child.GetComponent<Button>().interactable);
            child.GetComponent<Button>().interactable = true;
            foreach (Transform childchild in child.transform)
            {
                if (childchild.GetComponent<ActiveSystem>())
                {
                    childchild.GetComponent<ActiveSystem>().enabled = true;
                }
            }
        }
        foreach (Transform child in quitWindow.transform)
        {
            foreach (Transform childchild in child.transform)
            {
                if (childchild.GetComponent<Image>())
                {
                    childchild.gameObject.SetActive(false);
                }
            }
        }
    }

	public void Quit () 
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
        gameObject.GetComponent<GameMenuManager>().enabled = false;

        StartCoroutine(WaitAndPrint(1));

	}


    IEnumerator WaitAndPrint(float waitTime)
    {

        yield return new WaitForSeconds(waitTime);
        //等待之后执行的动作
#if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }
}
