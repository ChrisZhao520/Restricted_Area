using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class VideoScript2 : MonoBehaviour
{
    //电影纹理  
    public MovieTexture movTexture;
    private float timer;

    void Start()
    {

        Screen.lockCursor = true;
        timer = 0;
        //设置当前对象的主纹理为电影纹理  
        
        //设置电影纹理播放模式为循环  
        movTexture.loop = false;
        //StartCoroutine(DownLoadMovie());  
    }

    void Update()
    {
        timer += Time.deltaTime;
        //Debug.Log(timer);
        if (timer >= 77f)
        {
            movTexture.Stop();
            SceneManager.LoadScene("Main");
        }
        if (Input.GetKey(KeyCode.Escape)) 
        {
            movTexture.Stop();
            SceneManager.LoadScene("Main");

        }
        
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), movTexture, ScaleMode.StretchToFill);
        movTexture.Play();
        if (!movTexture.isPlaying)
        {
            SceneManager.LoadScene("Main");
        }
    }
}