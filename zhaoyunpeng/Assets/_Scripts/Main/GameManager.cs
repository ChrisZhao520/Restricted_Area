using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class GameManager : MonoBehaviour {
    public static GameManager Instance = null;  
    public int []m_minammo;                           // 显示的最小子弹数
    public int []m_maxammo;                           // 显示的最大子弹数
    public int []m_sumammo;                           // 枪总共子弹数
    public int []_ammo;                               // 弹夹容纳量
    public Text Txt_minammo;
    public Text Txt_maxammo;
    public Text Txt_destroyenemy;
    public Text Txt_life;
    public Text Txt_hgy;
    public Text Txt_survialday;
    public Text Txt_maxsurvialday;

    public GameObject HP;
    public GameObject HY;

    Player m_player;

    private int m_destroyenemy = 0;
    private int m_maxsurvialday = 0;
    private int m_survialday = 0;

	// Use this for initialization
	void Start ()
    {
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Instance = this;
                                           
        //Debug.Log(m_maxsurvialday.ToString("f0"));
        for (int i = 0; i < m_minammo.Length; i++)
        {
            if (m_player.m_gun.GetComponent<GunProperties>().GunNum == i)
            {
                foreach (Transform t in this.transform.GetComponentsInChildren<Transform>())
                {
                    if (t.name.CompareTo("Txt_minammo") == 0)
                    {
                        Txt_minammo = t.GetComponent<Text>();
                        Txt_minammo.text = m_minammo[i].ToString();
                    }
                    else if (t.name.CompareTo("Txt_maxammo") == 0)
                    {
                        Txt_maxammo = t.GetComponent<Text>();
                        Txt_maxammo.text = m_maxammo[i].ToString();
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
        }
	}

    void Update()
    {
        m_survialday = GameObject.Find("GameTime").GetComponent<GameTime>()._survialday;
        for (int i = 0; i < m_minammo.Length; i++)
        {
            if (m_player.m_gun.GetComponent<GunProperties>().GunNum == i)
            {
                m_maxammo[i] = m_sumammo[i] - m_minammo[i];
            }
        }
        
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
        for (int i = 0; i < m_minammo.Length; i++)
        {
            if (m_player.m_gun.GetComponent<GunProperties>().GunNum == i)
            {
                if (m_minammo[i] > 0 && m_maxammo[i] >= 0)
                {
                    if (m_maxammo[i] > 0)
                    {
                        m_minammo[i] -= ammo;
                        m_sumammo[i] -= 1;
                        /*if (!PlayerPrefs.HasKey("Txt_minammo"))
                        {
                            PlayerPrefs.SetInt("Txt_minammo", m_minammo[i]);
                        }
                        if (!PlayerPrefs.HasKey("Txt_maxammo"))
                        {
                            PlayerPrefs.SetInt("Txt_maxammo", m_maxammo[i]);
                        }*/
                    }
                    else if (m_maxammo[i] == 0)
                    {
                        m_minammo[i] -= ammo;
                        m_sumammo[i] = m_minammo[i];
                        /*if (!PlayerPrefs.HasKey("Txt_minammo"))
                        {
                            PlayerPrefs.SetInt("Txt_minammo", m_minammo[i]);
                        }
                        if (!PlayerPrefs.HasKey("Txt_maxammo"))
                        {
                            PlayerPrefs.SetInt("Txt_maxammo", m_maxammo[i]);
                        }*/
                    }
                    if (!Input.GetKey(KeyCode.R))
                    {
                        Txt_minammo.text = m_minammo[i].ToString();
                        Txt_maxammo.text = m_maxammo[i].ToString();
                    }
                }
            }
        }

        
        //Debug.Log(_ammo);
        //Debug.Log(m_sumammo);

    }

    public void SetLife(int life)
    {
        if (life < 0)
        {
            life = 0;
        }
        HP.GetComponent<Image>().fillAmount = life * 0.01f;
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
        HY.GetComponent<Image>().fillAmount = m_player.m_hungry * 0.01f;
        Txt_hgy.text = m_player.m_hungry.ToString();

        //Debug.Log(m_player.m_hungry);

        if (m_player.m_hungry <= 0)
        {
            m_player.m_hungry = 0;
            SceneManager.LoadScene("Death");
        }

    }

}
