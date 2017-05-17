using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;


public class EnterWater : MonoBehaviour {
    public GameObject enterwater;
    public float timeDelayWater;                              // 水中几秒后开始掉血
    public float timeWater;                                   // 水中每隔几秒一次掉血
    public int waterLife;                                     // 水中掉血的数值

    public AudioClip footstep01;
    public AudioClip footstep02;
    public AudioClip jumpaudio;
    public AudioClip landaudio;

    Player m_player;

    private GameObject objs;
    private float t1 = 0;                                     // 计算在水中生命值的中间变量1
    private float t2 = 0;                                     // 计算在水中生命值的中间变量2

	// Use this for initialization
	void Start () {
        m_player = GetComponent<Player>();
        objs = GameObject.FindGameObjectWithTag("WaterAudio");
	}
	
	// Update is called once per frame
	void Update () {
        

        if (GetComponent<Transform>().position.y <= 1.28f) {

            enterwater.SetActive(true);

            m_player.m_movSpeed = 2;
            m_player.m_runSpeed = 4;
            m_player.m_gravity = 0;
            m_player.m_jumpSpeed = 1f;
            m_player.m_FootstepSounds[0] = null;
            m_player.m_FootstepSounds[1] = null;
            m_player.m_JumpSound = null;
            m_player.m_LandSound = null;

            foreach (Transform child in objs.transform)
            {
                child.GetComponent<AudioSource>().pitch = 0.1f;
                child.GetComponent<AudioSource>().minDistance = 1.99f;
            }

            t1 += Time.deltaTime;

            if (t1 >= timeDelayWater)
            {
                StartCoroutine(WaitAndPrintSetLife(timeWater));
            }
                     
        }
        else
        {
            enterwater.SetActive(false);

            m_player.m_movSpeed = 7;
            m_player.m_runSpeed = 8;
            m_player.m_gravity = 2;
            m_player.m_jumpSpeed = 10;
            m_player.m_FootstepSounds[0] = footstep01;
            m_player.m_FootstepSounds[1] = footstep02;
            m_player.m_JumpSound = jumpaudio;
            m_player.m_LandSound = landaudio;

            foreach (Transform child in objs.transform)
            {
                child.GetComponent<AudioSource>().pitch = 1;
                child.GetComponent<AudioSource>().minDistance = 1f;
            }
            t1 = 0;
            
        }
	}

    IEnumerator WaitAndPrintSetLife(float waitTime)
    {
        
        yield return new WaitForSeconds(waitTime);
        //等待之后执行的动作
  
        t2 += Time.deltaTime;
        if (t2 >= 1)
        {
            //Debug.Log(m_player.m_life);

            //GetComponent<AudioSource>()                               // 掉血时的音效
            m_player.m_life -= waterLife;                        
            GameManager.Instance.SetLife(m_player.m_life);

            t2 = 0;
        }         
    }
}
