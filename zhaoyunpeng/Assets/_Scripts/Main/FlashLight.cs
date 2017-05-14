using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    public GameObject flashlight;
    public GameObject flashlightaudio;
    public AudioClip FlashAudioClip;
    private AudioSource FlashAudioSource;

	// Use this for initialization
	void Start () {
        FlashAudioSource = flashlightaudio.GetComponent<AudioSource>();
        FlashAudioSource.clip = FlashAudioClip;
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("a");
        if (Input.GetKeyDown(KeyCode.H))
        {
            //Debug.Log("H");
            
            //Debug.Log(flashlightaudio.active);

            if (flashlight.active == true)
            {
                //Debug.Log("Close");  
                flashlight.active = false;
                FlashAudioSource.Play();
            }
            else
            {
                //Debug.Log("Open");
                flashlight.active = true;
                FlashAudioSource.Play();
            }
            //Debug.Log(flashlightaudio.active);
        }

	}
}
