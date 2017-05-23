using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Instance = null;
    int m_destroyenemy = 0;
    int m_maxsurvialday = 0;
    int m_survialday = 0;
    public int m_ammo;
    public int m_maxammo;

    Player m_player;
    Text Txt_ammo;
    Text Txt_destroyenemy;
    Text Txt_life;
    Text Txt_hgy;
    Text Txt_survialday;
    Text Txt_maxsurvialday;

    private int _ammo;
    

	// Use this for initialization
	void Start ()
    {
        Instance = this;
        _ammo = m_ammo;                                     // 中间变量
        //Debug.Log(m_maxsurvialday.ToString("f0"));
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        foreach (Transform t in this.transform.GetComponentsInChildren<Transform>())
        {
            if (t.name.CompareTo("Txt_ammo") == 0)
            {
                Txt_ammo = t.GetComponent<Text>();
            }
            else if (t.name.CompareTo("Txt_destroyenemy") == 0)
            {
                Txt_destroyenemy = t.GetComponent<Text>();
                Txt_destroyenemy.text = "消灭敌人数: " + m_destroyenemy;
            }
            else if (t.name.CompareTo("Txt_life") == 0)
            {
                Txt_life = t.GetComponent<Text>();
            }
            else if (t.name.CompareTo("Txt_hgy") == 0)
            {
                Txt_hgy = t.GetComponent<Text>();
            }
            else if (t.name.CompareTo("Txt_survialday") == 0)
            {
                //Debug.Log(m_survialday);
                Txt_survialday = t.GetComponent<Text>();
                
            }
            else if (t.name.CompareTo("Txt_maxsurvialday") == 0)
            {
                Txt_maxsurvialday = t.GetComponent<Text>();
                
            }
        }
        //PlayerPrefs.DeleteAll();              // 重置最高天数
	}

    void Update()
    {
        m_survialday = GameObject.Find("GameTime").GetComponent<GameTime>()._survialday;

        //Debug.Log(m_survialday);
    }

    public void SetDestroyEnemy(int destroyenemy)
    {
        m_destroyenemy += destroyenemy;
        Txt_destroyenemy.text = "消灭敌人数: " + m_destroyenemy;
    }

    public void SetSurvialDay(int survialday)
    {
        //Debug.Log("123");
        if (!PlayerPrefs.HasKey("Txt_maxsurvialday"))
        {
            PlayerPrefs.SetInt("Txt_survialday", survialday);
        }
        else
        {
            if (survialday > PlayerPrefs.GetInt("Txt_maxsurvialday"))
            {
                PlayerPrefs.SetInt("Txt_survialday", survialday);
            }
            else
            {
                PlayerPrefs.SetInt("Txt_survialday", survialday);
            }
        }

        m_survialday = PlayerPrefs.GetInt("Txt_survialday");
        Txt_survialday.text = "生存天数: " + m_survialday.ToString("f0");

    }

    public void SetMaxSurvialDay(int survialday)
    {
        //Debug.Log("123");
        if (!PlayerPrefs.HasKey("Txt_maxsurvialday"))
        {
            PlayerPrefs.SetInt("Txt_maxsurvialday", survialday);
            
        }
        else
        {
            if (survialday > PlayerPrefs.GetInt("Txt_maxsurvialday"))
            {
                PlayerPrefs.SetInt("Txt_maxsurvialday", survialday);
                
            }
        }

        m_maxsurvialday = PlayerPrefs.GetInt("Txt_maxsurvialday");
        Txt_maxsurvialday.text = "历史最高生存天数: " + m_maxsurvialday.ToString("f0");

    }

    public void SetAmmo(int ammo)
    {       
        m_ammo -= ammo;
        m_maxammo -= 1;

        //Debug.Log(_ammo);
        //Debug.Log(m_maxammo);

        if (m_ammo <= 0)
        {
            m_ammo = _ammo;

        }
        Txt_ammo.text = m_ammo.ToString() + "/" + (m_maxammo - m_ammo);
    }

    public void SetLife(int life)
    {
        if (life < 0)
        {
            life = 0;
        }
        Txt_life.text = life.ToString();
        if (m_player.m_life <= 0)
        {
            SceneManager.LoadScene("Death");
            //Application.LoadLevel(Application.loadedLevelName);
        }
        
    }
    public void SetHgy(int hungry)
    {
        m_player.m_hungry -= hungry;
        Txt_hgy.text = m_player.m_hungry.ToString();

        //Debug.Log(m_player.m_hungry);

        if (m_player.m_hungry <= 0)
        {
            m_player.m_hungry = 0;
            SceneManager.LoadScene("Death");
        }

    }

}
