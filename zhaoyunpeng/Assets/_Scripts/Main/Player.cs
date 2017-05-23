using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;
using UnityStandardAssets.CrossPlatformInput;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour {

    public Transform m_transform;
    public CharacterController m_ch;
    public float m_movSpeed = 7.0f;                          // 角色移动速度
    public float m_runSpeed = 12.0f;                         // 角色奔跑速度
    public float m_gravity = 2.0f;                           // 重力
    public float m_jumpSpeed = 10.0f;                        // 跳跃速度
    public float m_StickToGroundForce = 20.0f;               // 跳跃时角色受的力
    public int m_life = 100;                                 // 生命值
    public int m_hungry = 100;                               // 饱食度 
    public float hgyTime;                                    // 每隔几秒饱食度递减
    public float hgyRunValue;                                // 跑步每隔几秒饱食度递减    
    public int hgyValue;                                     // 饱食度递减的值

    public float minX;                                       // 限制视野范围
    public float maxX;                                       // 限制视野范围

    public AudioClip[] m_FootstepSounds;        
    public AudioClip m_JumpSound;               
    public AudioClip m_LandSound;

    private bool m_Jump;
    private bool m_Jumping;
    private bool m_PreviouslyGrounded;
    private float m_Height;

    private Vector3 m_movDirection = Vector3.zero;
    private Transform m_camTransform;
    private Vector3 m_camRot;                                // 摄像机旋转
    private float m_camHeight;

    Transform m_muzzlepoint;                                 // 射线
    public LayerMask m_layer;
    public Transform m_fx;
    public float m_shootcd;                                  // 射击距离
    public AudioClip m_shotAudio;                            // 枪声
    float m_shootTimer = 0;
    private AudioSource m_AudioSource;
    private float t = 0;                                     // 计算饱食度的中间变量

    public GameObject flashlight;
    public GameObject flashlightaudio;
    public AudioClip FlashAudioClip;
    private AudioSource FlashAudioSource;

    public GameObject m_backpack;
    public float m_Raycastcd;                                // 拾取范围

    void Start()
    {
        m_transform = this.transform;
        m_ch = this.GetComponent<CharacterController>();
        m_Height = m_ch.height;
        m_camHeight = m_ch.height / 2;
        m_Jumping = false;

        m_camTransform = Camera.main.transform;              // 获取摄像机
        Vector3 pos = m_transform.position;
        pos.y += m_camHeight;
        m_camTransform.position = pos;
        m_camTransform.rotation = m_transform.rotation;
        m_camRot = m_camTransform.eulerAngles;
        Screen.lockCursor = true;
        m_muzzlepoint = m_camTransform.FindChild("M16/weapon/muzzlepoint").transform;

        m_AudioSource = GetComponent<AudioSource>();

        FlashAudioSource = flashlightaudio.GetComponent<AudioSource>();
        FlashAudioSource.clip = FlashAudioClip;
        
    }

    void Update()
    {
        
        if (m_life <= 0)
        {
            return;
        }
        Control();
        if (!m_Jump)
        {
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }

        if (!m_PreviouslyGrounded && m_ch.isGrounded)
        {
            //StartCoroutine(m_JumpBob.DoBobCycle());

            //PlayLandingSound();
            m_movDirection.y = 0f;
            m_Jumping = false;
        }
        if (!m_ch.isGrounded && !m_Jumping && m_PreviouslyGrounded)
        {
            m_movDirection.y = 0f;
        }

        m_PreviouslyGrounded = m_ch.isGrounded;


        if (t >= hgyTime)
        {
            t = 0;
            StartCoroutine(WaitAndPrintSetHgy(hgyValue));
        }
        t += Time.deltaTime;
        
        m_shootTimer -= Time.deltaTime;
        if (Input.GetMouseButton(0) && m_shootTimer < 0 && m_backpack.GetComponent<Canvas>().enabled == false && Time.timeScale != 0)
        {
            m_shootTimer = 0.1F;
            m_AudioSource.PlayOneShot(m_shotAudio);
            GameManager.Instance.SetAmmo(1);
            RaycastHit info;
            bool hit = Physics.Raycast(m_muzzlepoint.position,
                m_camTransform.TransformDirection(Vector3.forward), out info, m_shootcd, m_layer);
            if (hit)                                                // 射击
            {
                //Debug.Log("Hit");
                if (info.transform.tag.CompareTo("enemy") == 0)
                {
                    Enemy enemy = info.transform.GetComponent<Enemy>();
                    enemy.OnDamage(1);
                    //Debug.Log("enemy's life -1");
                }
                Instantiate(m_fx, info.point, info.transform.rotation);
            }
        }

        if (Input.GetKeyDown(KeyCode.H))                            // 打开关闭手电筒
        {
            //Debug.Log("H");

            //Debug.Log(flashlightaudio.active);

            if (flashlight.active == true)
            {
                //Debug.Log("Close");  
                flashlight.active = false;
                FlashAudioSource.Play();
            }
            else
            {
                //Debug.Log("Open");
                flashlight.active = true;
                FlashAudioSource.Play();
            }
            //Debug.Log(flashlightaudio.active);
        }

        if (Input.GetKeyDown(KeyCode.B))                            // 打开关闭背包
        {
            if (m_backpack.GetComponent<Canvas>().enabled == true)
            {

                Screen.lockCursor = true;
                m_backpack.GetComponent<Canvas>().enabled = false;
            }
            else
            {
                Screen.lockCursor = false;
                m_backpack.GetComponent<Canvas>().enabled = true;
            }
        }

        if (Input.GetKey(KeyCode.F))                                // 拾取道具
        {

            RaycastHit info;
            bool hit = Physics.Raycast(m_muzzlepoint.position,
                m_camTransform.TransformDirection(Vector3.forward), out info, m_Raycastcd);
            if (hit)
            {
                //划出射线，只有在scene视图中才能看到
                GameObject gameObj = info.collider.gameObject;
                if (gameObj.tag == "qiang")//当射线碰撞目标为qiang类型的物品 ，执行拾取操作
                {
                    Debug.Log(gameObj.tag);
                    backpack_manger.Instancce.StoreItem(0);
                    Destroy(gameObj);
                    return;
                }
                if (gameObj.tag == "mubang")
                {
                    //Debug.Log("pick up!");
                    backpack_manger.Instancce.StoreItem(2);
                    Destroy(gameObj);
                    return;
                }
                if (gameObj.tag == "bishou")
                {

                    backpack_manger.Instancce.StoreItem(1);
                    Destroy(gameObj);
                    return;
                }
                if (gameObj.tag == "banzhuan")
                {

                    backpack_manger.Instancce.StoreItem(3);
                    Destroy(gameObj);
                    return;
                }
                
                if (gameObj.tag == "pistol")
                {
                    
                    backpack_manger.Instancce.StoreItem(6);
                    Destroy(gameObj);
                    return;
                }
                if (gameObj.tag == "Pistol cartridges")
                {
                    
                    backpack_manger.Instancce.StoreItem(7);
                    Destroy(gameObj);
                    return;
                }
            }
        }

    }
    void FixedUpdate() 
    { 

    }
    void Control()
    {
        float rh = Input.GetAxis("Mouse X");                    // 获得鼠标水平滑动的距离
        float rv = Input.GetAxis("Mouse Y");                    // 获得鼠标垂直滑动的距离
        m_camRot.x -= rv;
        m_camRot.y += rh;
        m_camRot.x = Mathf.Clamp(m_camRot.x, minX, maxX);       // 限制视觉范围
        m_camTransform.eulerAngles = m_camRot;
        Vector3 camRot = m_camTransform.eulerAngles;
        camRot.x = 0;
        camRot.z = 0;
        m_transform.eulerAngles = camRot;
        Vector3 pos = m_transform.position;
        pos.y += m_camHeight;
        m_camTransform.position = pos;

        float xm = 0, ym = 0, zm = 0;
        ym -= m_gravity * Time.deltaTime;

        if (Input.GetKey(KeyCode.W))                            // 前
        {
            //PlayFootStepAudio();

            if (Input.GetKey(KeyCode.LeftShift))                // 跑
            {
                zm += m_runSpeed * Time.deltaTime;
                hgyTime = hgyRunValue;
            }
            else
            {
                zm += m_movSpeed * Time.deltaTime;
                hgyTime = 10;
            }
                

        }
        else if (Input.GetKey(KeyCode.S))                       // 后
        {
            //PlayFootStepAudio();
            zm -= m_movSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))                            // 左
        {
            //PlayFootStepAudio();
            xm -= m_movSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))                       // 右
        {
            //PlayFootStepAudio();
            xm += m_movSpeed * Time.deltaTime;
        }

        if (m_ch.isGrounded)                                    // 跳跃
        {
            m_movDirection.y = -m_StickToGroundForce;

            if (m_Jump)
            {
                PlayJumpSound();
                m_movDirection.y = m_jumpSpeed;
                m_Jump = false;
                m_Jumping = true;
            }
        }
        else
        {
            m_movDirection += Physics.gravity * m_gravity * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))               // 蹲
        {
            m_ch.height = m_Height - 0.8f - m_ch.skinWidth * 2;
            m_transform.position = new Vector3(m_transform.position.x, (m_transform.position.y - (m_Height - 0.8f) * 0.75f), m_transform.position.z);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            m_ch.height = m_Height;
            m_transform.position = new Vector3(m_transform.position.x, (m_transform.position.y + (m_Height - 0.8f) * 0.75f), m_transform.position.z);
        }
            
        m_movDirection.y -= m_gravity * Time.deltaTime;
        m_ch.Move(m_transform.TransformDirection(new Vector3(xm, ym, zm)));
        m_ch.Move(m_movDirection * Time.deltaTime);

    }

    public void OnDamage(int damage)
    {
        m_life -= damage;
        GameManager.Instance.SetLife(m_life);
        if (m_life <= 0)
        {
            Screen.lockCursor = false;
        }
    }

    private void PlayJumpSound()
    {
        m_AudioSource.clip = m_JumpSound;
        m_AudioSource.Play();
    }

    IEnumerator WaitAndPrintSetHgy(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        //等待之后执行的动作  
        GameManager.Instance.SetHgy(hgyValue);

    }



}
