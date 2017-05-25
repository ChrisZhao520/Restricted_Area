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
    private float movSpeed;
    private float runSpeed;
    private float gravity;
    private float jumpSpeed;
    private float t1 = 0;                                     // 计算在水中生命值的中间变量1
    private float t2 = 0;                                     // 计算在水中生命值的中间变量2
    private float t3 = 0;

	// Use this for initialization
	void Start () {
        m_player = GetComponent<Player>();

        movSpeed = m_player.m_movSpeed;
        runSpeed = m_player.m_runSpeed;
        gravity = m_player.m_gravity;
        jumpSpeed = m_player.m_jumpSpeed;
        objs = GameObject.FindGameObjectWithTag("WaterAudio");
	}
	
	// Update is called once per frame
	void Update () {
        

        if (GetComponent<Transform>().position.y <= 1.5f) {

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
            t3 = 1;
                     
        }
        else
        {
            enterwater.SetActive(false);
            if (t3 == 1)
            {
                m_player.m_movSpeed = movSpeed;
                t3 = 0;
            }

            //Debug.Log(m_player.m_squatSpeed);
            m_player.m_runSpeed = runSpeed;
            m_player.m_gravity = gravity;
            m_player.m_jumpSpeed = jumpSpeed;
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
