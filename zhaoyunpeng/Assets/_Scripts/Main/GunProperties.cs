using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class GunProperties : MonoBehaviour
{
    public int GunNum;
    public int Minammo;
    public int Maxammo;
    public float ShootSpeedCD;
    public GameObject GameManager;
    public GameObject m_player;
    public AudioClip TargetAudio;
    public AudioClip AmmoOut;
	// Use this for initialization
	void Start () {
        for (int n = 0; n < GameManager.GetComponent<GameManager>().m_minammo.Length; n++)
        {
            if (m_player.GetComponent<Player>().m_gun.GetComponent<GunProperties>().GunNum == n)
            {
                /*GameManager.GetComponent<GameManager>().m_minammo[n] = Minammo;
                GameManager.GetComponent<GameManager>().m_maxammo[n] = Maxammo;
                GameManager.GetComponent<GameManager>().m_sumammo[n] = Minammo + Maxammo;
                GameManager.GetComponent<GameManager>()._ammo[n] = Minammo;*/
                m_player.GetComponent<Player>().m_shootSpeedCD = ShootSpeedCD;
            }
        }
        
	}
	
	// Update is called once per frame
	void Update () {
        
	}
    public void GunUpdate()
    {
        for (int n = 0; n < GameManager.GetComponent<GameManager>().m_minammo.Length; n++)
        {
            if (m_player.GetComponent<Player>().m_gun.GetComponent<GunProperties>().GunNum == n)
            {
                GameManager.GetComponent<GameManager>().Txt_minammo.text = GameManager.GetComponent<GameManager>().m_minammo[n].ToString();
                GameManager.GetComponent<GameManager>().Txt_maxammo.text = GameManager.GetComponent<GameManager>().m_maxammo[n].ToString();
                GameManager.GetComponent<GameManager>()._ammo[n] = Minammo;
                m_player.GetComponent<Player>().m_shootSpeedCD = ShootSpeedCD;
            }
        }
    }
}
