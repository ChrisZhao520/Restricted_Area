using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float m_timer = 1.0f;
    void Start()
    {
        Destroy(this.gameObject, m_timer);
    }
	
}
