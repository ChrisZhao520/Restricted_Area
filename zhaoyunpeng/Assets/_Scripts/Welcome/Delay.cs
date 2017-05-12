using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Delay : MonoBehaviour {
    public GameObject SliderObj;
    public GameObject LoadingObj;

	// Use this for initialization
	void Start () {
        StartCoroutine(WaitAndPrintSlider(2.0f));
        StartCoroutine(WaitAndPrintLoading(3.0f));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator WaitAndPrintSlider(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        //等待之后执行的动作  
        SliderObj.SetActive(true);
      
    }
    IEnumerator WaitAndPrintLoading(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        //等待之后执行的动作  
        LoadingObj.SetActive(true);

    }
}
