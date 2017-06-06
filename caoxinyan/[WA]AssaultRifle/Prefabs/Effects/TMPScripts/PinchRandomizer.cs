using UnityEngine;
using System.Collections;

public class PinchRandomizer : MonoBehaviour 
{
	public float m_PinchRandom;
	private AudioSource m_Source;
	void Start () 
	{
		m_Source = GetComponent<AudioSource>();
		m_Source.pitch += Random.Range(-m_PinchRandom,m_PinchRandom);
	}

}
