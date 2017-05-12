using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class panel_manager : MonoBehaviour {
	public Animator startButton;
	public Animator loadlistButton;
	public Animator loadpanleButton;
	public Animator setlistButton;
	public Animator helplistButton;
	public Animator aboutlistButton;
	public Animator setpanleButton;
	public Animator helppanleButton;
	public Animator aboutpanleButton;
	public GameObject audios;
	public GameObject lights;
	public GameObject cameras;
	// Use this for initialization

	public void openload () {
		startButton.SetBool ("ishidden",true);
		loadlistButton.SetBool ("ishidden", false);
		loadpanleButton.SetBool ("ishidden", false);
	}
    public void closedload()
    {
        startButton.SetBool("ishidden", false);
        loadlistButton.SetBool("ishidden", true);
        loadpanleButton.SetBool("ishidden", true);
    }

	public void openset(){
		startButton.SetBool ("ishidden",true);
		setlistButton.SetBool ("ishidden", false);
		setpanleButton.SetBool ("ishidden", false);
	}
	public void openhelp(){
		startButton.SetBool ("ishidden",true);
		helplistButton.SetBool ("ishidden", false);
		helppanleButton.SetBool ("ishidden", false);
	}
	public void openabout(){
		startButton.SetBool ("ishidden",true);
		aboutlistButton.SetBool ("ishidden", false);
		aboutpanleButton.SetBool ("ishidden", false);
	}
	
	public void closedset(){
		startButton.SetBool ("ishidden",false);
		setlistButton.SetBool ("ishidden", true);
		setpanleButton.SetBool ("ishidden", true);
	}
	public void closedhelp(){
		startButton.SetBool ("ishidden",false);
		helplistButton.SetBool ("ishidden", true);
		helppanleButton.SetBool ("ishidden", true);
	}
	public void closedabout(){
		startButton.SetBool ("ishidden",false);
		aboutlistButton.SetBool ("ishidden", true);
		aboutpanleButton.SetBool ("ishidden", true);
	}
	public void openaudios(){
		audios.gameObject.SetActive (true);
		lights.gameObject.SetActive (false);
		cameras.gameObject.SetActive (false);
	}
	public void openlights(){
		audios.gameObject.SetActive (false);
		lights.gameObject.SetActive (true);
		cameras.gameObject.SetActive (false);
	}
	public void opencameras(){
		audios.gameObject.SetActive (false);
		lights.gameObject.SetActive (false);
		cameras.gameObject.SetActive (true);
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
