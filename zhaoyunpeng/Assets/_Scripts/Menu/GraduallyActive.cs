using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraduallyActive : MonoBehaviour {

    private float timer;
	// Use this for initialization
	void Start () {
        timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
        StartCoroutine(WaitAndPrint(3.0f));
        
	}

    IEnumerator WaitAndPrint(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //等待之后执行的动作
        timer += Time.deltaTime*2;
        if (timer <= 1)
        {
            gameObject.GetComponent<Image>().color = new Color(0, 0, 0, timer);
        }
    }
}
