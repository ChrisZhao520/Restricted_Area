using UnityEngine;
using System.Collections;
using System;

public class ondoubleclicked : MonoBehaviour
{
    public float doubeClickDelay = 0.4f;
    public bool firstClick = false;
    public float firstClickTime;
    public Vector2 mousePosition;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (firstClick == false)
            {
                firstClick = true;
                firstClickTime = Time.time;
                return;
            }
            if (firstClick)
            {
                Vector2 pos = this.mousePosition;
                //GameObject obj = pos.Equals()
                if (gameObject.tag == "banzhuan")
                {
                    gameObject.SetActive(false);
                    Debug.LogWarning("111");
                }
                
            }
        }
        if (Time.time - firstClickTime > doubeClickDelay)
        {
            firstClick = false;
        }
        
    }

}
