using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGraduallyActive : MonoBehaviour
{

    private float timer;
	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Image>().enabled = true;
        timer = 1;
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime*0.5f;
        if (timer >= 0)
        {
            gameObject.GetComponent<Image>().color = new Color(0, 0, 0, timer);
        }
	}

}
