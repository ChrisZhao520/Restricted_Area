using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	public GameObject audios;
	public GameObject lights;
	public GameObject cameras;

    public GameObject ButtonAudio;
    private AudioSource ButtonAudioSource;
    public GameObject highlightedAudio;
    public AudioClip NoneClip;
    public AudioClip HighlightedAudioClip;
    private AudioSource HighlightedAudioSource;

    // Use this for initialization
    void Start()
    {
        ButtonAudioSource = ButtonAudio.GetComponent<AudioSource>();
        HighlightedAudioSource = highlightedAudio.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

	public void openload () {
        HighlightedAudioSource.clip = NoneClip;
        ButtonAudioSource.Play();
		startButton.SetBool ("ishidden",true);
		loadlistButton.SetBool ("ishidden", false);
		loadpanleButton.SetBool ("ishidden", false);
        StartCoroutine(WaitAndPrint(0.3f));
	}
    public void closedload()
    {
        HighlightedAudioSource.clip = NoneClip;
        ButtonAudioSource.Play();
        startButton.SetBool("ishidden", false);
        loadlistButton.SetBool("ishidden", true);
        loadpanleButton.SetBool("ishidden", true);
        StartCoroutine(WaitAndPrint(0.3f));
    }
    public void opensave()
    {
        HighlightedAudioSource.clip = NoneClip;
        ButtonAudioSource.Play();
        startButton.SetBool("ishidden", true);
        savelistButton.SetBool("ishidden", false);
        savepanleButton.SetBool("ishidden", false);
        StartCoroutine(WaitAndPrint(0.3f));
    }
    public void closedsave()
    {
        HighlightedAudioSource.clip = NoneClip;
        ButtonAudioSource.Play();
        startButton.SetBool("ishidden", false);
        savelistButton.SetBool("ishidden", true);
        savepanleButton.SetBool("ishidden", true);
        StartCoroutine(WaitAndPrint(0.3f));
    }

	public void openset(){
        HighlightedAudioSource.clip = NoneClip;
        ButtonAudioSource.Play();
		startButton.SetBool ("ishidden",true);
		setlistButton.SetBool ("ishidden", false);
		setpanleButton.SetBool ("ishidden", false);
        StartCoroutine(WaitAndPrint(0.3f));
	}
	public void openhelp(){
        HighlightedAudioSource.clip = NoneClip;
        ButtonAudioSource.Play();
		startButton.SetBool ("ishidden",true);
		helplistButton.SetBool ("ishidden", false);
		helppanleButton.SetBool ("ishidden", false);
        StartCoroutine(WaitAndPrint(0.3f));
	}
	public void openabout(){
        HighlightedAudioSource.clip = NoneClip;
        ButtonAudioSource.Play();
		startButton.SetBool ("ishidden",true);
		aboutlistButton.SetBool ("ishidden", false);
		aboutpanleButton.SetBool ("ishidden", false);
        StartCoroutine(WaitAndPrint(0.3f));
	}
	
	public void closedset(){
        HighlightedAudioSource.clip = NoneClip;
        ButtonAudioSource.Play();
		startButton.SetBool ("ishidden",false);
		setlistButton.SetBool ("ishidden", true);
		setpanleButton.SetBool ("ishidden", true);
        StartCoroutine(WaitAndPrint(0.3f));
	}
	public void closedhelp(){
        HighlightedAudioSource.clip = NoneClip;
        ButtonAudioSource.Play();
		startButton.SetBool ("ishidden",false);
		helplistButton.SetBool ("ishidden", true);
		helppanleButton.SetBool ("ishidden", true);
        StartCoroutine(WaitAndPrint(0.3f));
	}
	public void closedabout(){
        HighlightedAudioSource.clip = NoneClip;
        ButtonAudioSource.Play();
		startButton.SetBool ("ishidden",false);
		aboutlistButton.SetBool ("ishidden", true);
		aboutpanleButton.SetBool ("ishidden", true);
        StartCoroutine(WaitAndPrint(0.3f));
	}
	public void openaudios(){
        HighlightedAudioSource.clip = NoneClip;
        ButtonAudioSource.Play();
		audios.gameObject.SetActive (true);
		lights.gameObject.SetActive (false);
		cameras.gameObject.SetActive (false);
        StartCoroutine(WaitAndPrint(0.3f));
	}
	public void openlights(){
        HighlightedAudioSource.clip = NoneClip;
        ButtonAudioSource.Play();
		audios.gameObject.SetActive (false);
		lights.gameObject.SetActive (true);
		cameras.gameObject.SetActive (false);
        StartCoroutine(WaitAndPrint(0.3f));
	}
	public void opencameras(){
        HighlightedAudioSource.clip = NoneClip;
        ButtonAudioSource.Play();
		audios.gameObject.SetActive (false);
		lights.gameObject.SetActive (false);
		cameras.gameObject.SetActive (true);
        StartCoroutine(WaitAndPrint(0.3f));
	}

    IEnumerator WaitAndPrint(float waitTime)
    {

        yield return new WaitForSeconds(waitTime);
        //等待之后执行的动作
        HighlightedAudioSource.clip = HighlightedAudioClip;
    }
}
