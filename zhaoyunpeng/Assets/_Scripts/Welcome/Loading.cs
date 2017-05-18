using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour {
    public float timer = 1.0f;

    private float alpha1;
    private float alpha2;

	// Use this for initialization
	void Start () {
        alpha1 = 0;
        alpha2 = 1;
        
	}
	
	// Update is called once per frame
	void Update () {
        
        timer -= Time.deltaTime * 0.5f;
        //Debug.Log(alpha);
        if (alpha1 <= 1)
        {
            alpha1 += 0.015f;
            alpha2 = 1;
            GetComponent<Text>().color = new Color(220, 220, 220, alpha1);
        }
        if (alpha1 > 1)
        {
            alpha2 -= 0.015f;
            GetComponent<Text>().color = new Color(220, 220, 220, alpha2);
            if (alpha2 < 0)
            {
                alpha1 = 0;
                alpha2 = 1;
            }
        }
        GetComponent<Text>().text = "加载中.  ";
        if (timer <= 0.66f)
        {
            GetComponent<Text>().text = "加载中.. ";
        }
        if (timer <= 0.33f)
        {
            GetComponent<Text>().text = "加载中...";
            
        }
        if (timer <= 0f)
        {
            timer = 1.0f;
        }
	}

}
