using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewGame : MonoBehaviour {
    public GameObject objs;
    public GameObject ButtonAudio;
    public GameObject gradually;
    public GameObject menuBGM;

    private AudioSource ButtonAudioSource;

    void Start()
    {
        ButtonAudioSource = ButtonAudio.GetComponent<AudioSource>();
    }

    public void StartGame(){
        ButtonAudioSource.Play();
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
        menuBGM.GetComponent<AudioSource>().Stop();
        gradually.GetComponent<GraduallyActive>().enabled = true;
        
        StartCoroutine(WaitAndPrint(4.0f));

    }

    IEnumerator WaitAndPrint(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //等待之后执行的动作
        foreach (Transform child in objs.transform)
        {
            //Debug.Log(child.GetComponent<Button>().interactable);
            child.GetComponent<Button>().interactable = true;
        }
        SceneManager.LoadScene("Plot");
    }
}
