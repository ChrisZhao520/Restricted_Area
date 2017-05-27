using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WelcomeActive : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        StartCoroutine(WaitAndPrint(2));
	}

    IEnumerator WaitAndPrint(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        //等待之后执行的动作  
        Screen.lockCursor = true;
        gameObject.GetComponent<Animator>().enabled = true;
        StartCoroutine(WaitAndTZ(5));
        
    }
    IEnumerator WaitAndTZ(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        //等待之后执行的动作  
        SceneManager.LoadScene("Menu");

    }
}
