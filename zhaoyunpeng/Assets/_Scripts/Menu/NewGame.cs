using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewGame : MonoBehaviour {
    private GameObject objs;

    public void StartGame(){
        objs = GameObject.FindGameObjectWithTag("Menu-button");
        foreach (Transform child in objs.transform)
        {
            //Debug.Log(child.GetComponent<Button>().interactable);
            child.GetComponent<Button>().interactable = false;
        }
        StartCoroutine(WaitAndPrint(2.0f));
    }

    IEnumerator WaitAndPrint(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //等待之后执行的动作  
        SceneManager.LoadScene("Main");
    }
}
