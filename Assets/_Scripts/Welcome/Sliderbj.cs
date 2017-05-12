using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Sliderbj : MonoBehaviour {
    private bool bl = false;

	// Use this for initialization
	void Start () {
        StartCoroutine(WaitAndPrint1(2.0f));
	}

	// Update is called once per frame
	void Update () {
        if (bl) { 
            if (GetComponent<Slider>().value <= 0.6f)
            {
                GetComponent<Slider>().value += Time.deltaTime * 0.2f;
            }
            if (GetComponent<Slider>().value > 0.6f && GetComponent<Slider>().value < 0.7f)
            {
                GetComponent<Slider>().value += Time.deltaTime * 0.05f;
            }
            if (GetComponent<Slider>().value > 0.7f)
            {
                GetComponent<Slider>().value += Time.deltaTime * 0.5f;
            }
            if (GetComponent<Slider>().value >= 1)
            {
                StartCoroutine(WaitAndPrint2(2.0f));
            }
        }
	}

    public void ToMenuScene()
    {
        SceneManager.LoadScene("Menu");
    }

    IEnumerator WaitAndPrint1(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //等待之后执行的动作  
        bl = true;
    }
    IEnumerator WaitAndPrint2(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //等待之后执行的动作  
        ToMenuScene();
    }
}
