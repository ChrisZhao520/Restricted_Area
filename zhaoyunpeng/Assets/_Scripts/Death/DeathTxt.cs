using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathTxt : MonoBehaviour {
    public int m_survialday;
    public int m_maxsurvialday;
    Text Txt_survialday;
    Text Txt_maxsurvialday;
	// Use this for initialization
	void Start () {
        //生存天数
        foreach (Transform t in this.transform.GetComponentsInChildren<Transform>())
        {
           
            if (t.name.CompareTo("Txt_survialday") == 0)
            {
                //Debug.Log(m_survialday);
                Txt_survialday = t.GetComponent<Text>();

            }
            else if (t.name.CompareTo("Txt_maxsurvialday") == 0)
            {
                Txt_maxsurvialday = t.GetComponent<Text>();

            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        m_survialday = PlayerPrefs.GetInt("Txt_survialday");
        Txt_survialday.text = "本次游戏你一共生存了 " + m_survialday.ToString("f0") + " 天";

        m_maxsurvialday = PlayerPrefs.GetInt("Txt_maxsurvialday");
        Txt_maxsurvialday.text = "你的最高生存记录是 " + m_maxsurvialday.ToString("f0") + " 天";

	}
    public void LoadText()
    {
        SceneManager.LoadScene("Death");
    }
    public void ReplayText()
    {
        SceneManager.LoadScene("Plot");
    }
    public void BackText()
    {
        SceneManager.LoadScene("Menu");
    }
}
