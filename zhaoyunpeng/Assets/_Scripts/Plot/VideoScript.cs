using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class VideoScript : MonoBehaviour
{
    //电影纹理  
    public MovieTexture movTexture;
    public float movieLength;
    public GameObject graduallyActive;

    private float timer;
    private float t;

    void Start()
    {
        //设置当前对象的主纹理为电影纹理
        gameObject.GetComponent<Renderer>().material.mainTexture = movTexture;

        //设置电影纹理播放模式为循环  
        movTexture.loop = false;

        Screen.lockCursor = true;
        timer = 0;
        t = 0;

    }

    void Update()
    {
        timer += Time.deltaTime;
        //Debug.Log(timer);
        if (timer >= movieLength)
        {
            t += Time.deltaTime * 0.5f;
            graduallyActive.GetComponent<Image>().color = new Color(0, 0, 0, t);
            gameObject.GetComponent<AudioSource>().volume = 1 - t;
            if (t >= 2)
            {
                movTexture.Stop();
                SceneManager.LoadScene("Main");
            }
            
        }
        if (Input.GetKey(KeyCode.Escape)) 
        {
            movTexture.Stop();
            SceneManager.LoadScene("Main");

        }
        
    }

    void OnGUI()
    {
        movTexture.Play();
        /*if (!movTexture.isPlaying)
        {
            SceneManager.LoadScene("Main");
        }*/
    }
}