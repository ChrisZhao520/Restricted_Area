using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Aim : MonoBehaviour {
    public GameObject m_gun;
    public Camera m_Camera;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (GetComponent<Player>().m_aim) {
            
            if (m_Camera.GetComponent<Camera>().fieldOfView <= 60)
            {
                m_Camera.GetComponent<Camera>().fieldOfView -= Time.deltaTime * 50;
            }
            if (m_Camera.GetComponent<Camera>().fieldOfView <= 45)
            {
                //Debug.Log("123");
                m_Camera.GetComponent<Camera>().fieldOfView = 45;
                GetComponent<Aim>().enabled = false;
            }
        }
        
        if (!GetComponent<Player>().m_aim)
        {
            if (m_Camera.GetComponent<Camera>().fieldOfView >= 45)
            {
                m_Camera.GetComponent<Camera>().fieldOfView += Time.deltaTime * 50;
            }
            if (m_Camera.GetComponent<Camera>().fieldOfView >= 60)
            {
                //Debug.Log("123");
                m_Camera.GetComponent<Camera>().fieldOfView = 60;
                GetComponent<Aim>().enabled = false;
            }
        }
	}
}
