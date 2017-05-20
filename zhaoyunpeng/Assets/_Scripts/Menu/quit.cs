using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class quit : MonoBehaviour {
    public GameObject objs;

    public GameObject ButtonAudio;
    private AudioSource ButtonAudioSource;

    void Start()
    {
        ButtonAudioSource = ButtonAudio.GetComponent<AudioSource>();
    }

	public void Quit () 
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
#if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }
}
