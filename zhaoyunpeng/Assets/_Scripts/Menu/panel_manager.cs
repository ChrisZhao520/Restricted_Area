using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class panel_manager : MonoBehaviour {

	public Animator startButton;
	public Animator loadlistButton;
	public Animator loadpanleButton;
    public Animator savelistButton;
    public Animator savepanleButton;
	public Animator setlistButton;
	public Animator helplistButton;
	public Animator aboutlistButton;
	public Animator setpanleButton;
	public Animator helppanleButton;
	public Animator aboutpanleButton;
	public GameObject audio;
	public GameObject display;
	public GameObject control;
    public GameObject backgroundBlack;

    public GameObject ButtonAudio;
    private AudioSource ButtonAudioSource;
    public GameObject highlightedAudio;
    public AudioClip NoneClip;
    public AudioClip HighlightedAudioClip;
    private AudioSource HighlightedAudioSource;

    public float timer;

    // Use this for initialization
    void Start()
    {
        timer = 0;
        ButtonAudioSource = ButtonAudio.GetComponent<AudioSource>();
        HighlightedAudioSource = highlightedAudio.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

	public void openload () {
        timer = 1;

        HighlightedAudioSource.Pause();
        ButtonAudioSource.Play();
        backgroundBlack.GetComponent<Image>().enabled = true;
		startButton.SetBool ("ishidden",true);
		loadlistButton.SetBool ("ishidden", false);
		loadpanleButton.SetBool ("ishidden", false);

        StartCoroutine(WaitAndPrint(0.3f));
	}
    public void closedload()
    {
        timer = 0;

        HighlightedAudioSource.Pause();
        ButtonAudioSource.Play();
        backgroundBlack.GetComponent<Image>().enabled = false;
        startButton.SetBool("ishidden", false);
        loadlistButton.SetBool("ishidden", true);
        loadpanleButton.SetBool("ishidden", true);

        StartCoroutine(WaitAndPrint(0.3f));
    }
    public void opensave()
    {
        timer = 2;

        HighlightedAudioSource.Pause();
        ButtonAudioSource.Play();
        backgroundBlack.GetComponent<Image>().enabled = true;
        startButton.SetBool("ishidden", true);
        savelistButton.SetBool("ishidden", false);
        savepanleButton.SetBool("ishidden", false);

        StartCoroutine(WaitAndPrint(0.3f));
    }
    public void closedsave()
    {
        timer = 0;

        HighlightedAudioSource.Pause();
        ButtonAudioSource.Play();
        backgroundBlack.GetComponent<Image>().enabled = false;
        startButton.SetBool("ishidden", false);
        savelistButton.SetBool("ishidden", true);
        savepanleButton.SetBool("ishidden", true);

        StartCoroutine(WaitAndPrint(0.3f));
    }

	public void openset(){
        timer = 3;

        HighlightedAudioSource.Pause();
        ButtonAudioSource.Play();
        backgroundBlack.GetComponent<Image>().enabled = true;
		startButton.SetBool ("ishidden",true);
		setlistButton.SetBool ("ishidden", false);
		setpanleButton.SetBool ("ishidden", false);

        StartCoroutine(WaitAndPrint(0.3f));
	}
	public void openhelp(){
        timer = 4;

        HighlightedAudioSource.Pause();
        ButtonAudioSource.Play();
        backgroundBlack.GetComponent<Image>().enabled = true;
		startButton.SetBool ("ishidden",true);
		helplistButton.SetBool ("ishidden", false);
		helppanleButton.SetBool ("ishidden", false);

        StartCoroutine(WaitAndPrint(0.3f));
	}
	public void openabout(){
        timer = 5;

        HighlightedAudioSource.Pause();
        ButtonAudioSource.Play();
        backgroundBlack.GetComponent<Image>().enabled = true;
		startButton.SetBool ("ishidden",true);
		aboutlistButton.SetBool ("ishidden", false);
		aboutpanleButton.SetBool ("ishidden", false);

        StartCoroutine(WaitAndPrint(0.3f));
	}
	
	public void closedset(){
        timer = 0;

        HighlightedAudioSource.Pause();
        ButtonAudioSource.Play();
        backgroundBlack.GetComponent<Image>().enabled = false;
		startButton.SetBool ("ishidden",false);
		setlistButton.SetBool ("ishidden", true);
		setpanleButton.SetBool ("ishidden", true);

        audio.SetActive(true);
        display.SetActive(false);
        control.SetActive(false);

        StartCoroutine(WaitAndPrint(0.3f));
	}
	public void closedhelp(){
        timer = 0;

        HighlightedAudioSource.Pause();
        ButtonAudioSource.Play();
        backgroundBlack.GetComponent<Image>().enabled = false;
		startButton.SetBool ("ishidden",false);
		helplistButton.SetBool ("ishidden", true);
		helppanleButton.SetBool ("ishidden", true);

        StartCoroutine(WaitAndPrint(0.3f));
	}
	public void closedabout(){
        timer = 0;

        HighlightedAudioSource.Pause();
        ButtonAudioSource.Play();
        backgroundBlack.GetComponent<Image>().enabled = false;
		startButton.SetBool ("ishidden",false);
		aboutlistButton.SetBool ("ishidden", true);
		aboutpanleButton.SetBool ("ishidden", true);

        StartCoroutine(WaitAndPrint(0.3f));
	}
	public void openaudio(){

        HighlightedAudioSource.Pause();
        ButtonAudioSource.Play();

        audio.SetActive(true);
        display.SetActive(false);
        control.SetActive(false);

        StartCoroutine(WaitAndPrint(0.3f));
	}
	public void opendisplay(){

        HighlightedAudioSource.Pause();
        ButtonAudioSource.Play();

        audio.SetActive(false);
        display.SetActive(true);
        control.SetActive(false);

        StartCoroutine(WaitAndPrint(0.3f));
	}
	public void opencontrol(){

        HighlightedAudioSource.Pause();
        ButtonAudioSource.Play();

        audio.SetActive(false);
        display.SetActive(false);
        control.SetActive(true);

        StartCoroutine(WaitAndPrint(0.3f));
	}

    IEnumerator WaitAndPrint(float waitTime)
    {

        yield return new WaitForSeconds(waitTime);
        //等待之后执行的动作
        HighlightedAudioSource.clip = HighlightedAudioClip;
    }
}
