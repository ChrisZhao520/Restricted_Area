using UnityEngine;
using System.Collections;

public class SelfDestroy : MonoBehaviour 
{
	public float m_time = 1.0f;
	void Start () 
	{
		Destroy(this.gameObject, m_time);
	}
	

}
