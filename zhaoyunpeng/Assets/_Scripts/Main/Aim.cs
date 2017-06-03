using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Aim : MonoBehaviour {
    public GameObject m_gun;
    public Camera m_Camera;

    private float m_gunposX;                                 // 枪最初的位置
    private float m_gunposY;
    private float m_gunposZ;
	// Use this for initialization
	void Start () {
        m_gunposX = m_gun.GetComponent<Transform>().localPosition.x;
        m_gunposY = m_gun.GetComponent<Transform>().localPosition.y;
        m_gunposZ = m_gun.GetComponent<Transform>().localPosition.z;
	}
	
	// Update is called once per frame
	void Update () {
        if (GetComponent<Player>().m_aim) {
            if (m_gun.GetComponent<Transform>().localPosition.x > 0.001)
            {
                m_gun.GetComponent<Transform>().localPosition = new Vector3(m_gun.GetComponent<Transform>().localPosition.x - Time.deltaTime, m_gun.GetComponent<Transform>().localPosition.y, m_gun.GetComponent<Transform>().localPosition.z);
            }
            if (m_gun.GetComponent<Transform>().localPosition.y < -0.17f)
            {
                m_gun.GetComponent<Transform>().localPosition = new Vector3(m_gun.GetComponent<Transform>().localPosition.x, m_gun.GetComponent<Transform>().localPosition.y + Time.deltaTime * 0.2f, m_gun.GetComponent<Transform>().localPosition.z);
            }
            if (m_gun.GetComponent<Transform>().localPosition.z > 0.04f)
            {
                m_gun.GetComponent<Transform>().localPosition = new Vector3(m_gun.GetComponent<Transform>().localPosition.x, m_gun.GetComponent<Transform>().localPosition.y, m_gun.GetComponent<Transform>().localPosition.z - Time.deltaTime);
            }
            if (m_Camera.GetComponent<Camera>().fieldOfView <= 60)
            {
                m_Camera.GetComponent<Camera>().fieldOfView -= Time.deltaTime * 50;
            }
            if (m_gun.GetComponent<Transform>().localPosition.x <= 0.001 ||
                m_gun.GetComponent<Transform>().localPosition.y >= -0.17f ||
                m_gun.GetComponent<Transform>().localPosition.z <= 0.04f ||
                m_Camera.GetComponent<Camera>().fieldOfView <= 45)
            {
                //Debug.Log("123");
                m_gun.GetComponent<Transform>().localPosition = new Vector3(0.001f, -0.17f, 0.04f);
                m_Camera.GetComponent<Camera>().fieldOfView = 45;
                GetComponent<Aim>().enabled = false;
            }
        }
        
        if (!GetComponent<Player>().m_aim)
        {
            if (m_gun.GetComponent<Transform>().localPosition.x < m_gunposX)
            {
                m_gun.GetComponent<Transform>().localPosition = new Vector3(m_gun.GetComponent<Transform>().localPosition.x + Time.deltaTime, m_gun.GetComponent<Transform>().localPosition.y, m_gun.GetComponent<Transform>().localPosition.z);
            }
            if (m_gun.GetComponent<Transform>().localPosition.y > m_gunposY)
            {
                m_gun.GetComponent<Transform>().localPosition = new Vector3(m_gun.GetComponent<Transform>().localPosition.x, m_gun.GetComponent<Transform>().localPosition.y - Time.deltaTime * 0.2f, m_gun.GetComponent<Transform>().localPosition.z);
            }
            if (m_gun.GetComponent<Transform>().localPosition.z < m_gunposZ)
            {
                m_gun.GetComponent<Transform>().localPosition = new Vector3(m_gun.GetComponent<Transform>().localPosition.x, m_gun.GetComponent<Transform>().localPosition.y, m_gun.GetComponent<Transform>().localPosition.z + Time.deltaTime);
            }
            if (m_Camera.GetComponent<Camera>().fieldOfView >= 45)
            {
                m_Camera.GetComponent<Camera>().fieldOfView += Time.deltaTime * 50;
            }
            if (m_gun.GetComponent<Transform>().localPosition.x >= m_gunposX ||
                m_gun.GetComponent<Transform>().localPosition.y <= m_gunposY ||
                m_gun.GetComponent<Transform>().localPosition.z >= m_gunposZ ||
                m_Camera.GetComponent<Camera>().fieldOfView >= 60)
            {
                //Debug.Log("123");
                m_gun.GetComponent<Transform>().localPosition = new Vector3(m_gunposX, m_gunposY, m_gunposZ);
                m_Camera.GetComponent<Camera>().fieldOfView = 60;
                GetComponent<Aim>().enabled = false;
            }
        }
	}
}
