using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public Transform m_transform;
    public CharacterController m_ch;
    public float m_movSpeed = 7.0f;                          // 角色移动速度
    public float m_runSpeed = 10.0f;                         // 角色奔跑速度
    public float m_gravity = 8.0f;                           // 重力
    public float m_jumpSpeed = 12.0f;                        // 跳跃速度
    public int m_life = 100;                                 // 生命值
    public int m_hungry = 100;                               // 饱食度 
    public float hgyTime;                                    // 每隔几秒饱食度递减
    public float hgyRunValue;                                // 跑步每隔几秒饱食度递减    
    public int hgyValue;                                     // 饱食度递减的值

    public AudioClip[] m_FootstepSounds;        
    public AudioClip m_JumpSound;               
    public AudioClip m_LandSound;

    private Vector3 m_movDirection = Vector3.zero;

    private Transform m_camTeansform;
    private Vector3 m_camRot;                                // 摄像机旋转
    private float m_camHeight = 1f;

    Transform m_muzzlepoint;                                 // 射线
    public LayerMask m_layer;
    public Transform m_fx;
    public AudioClip m_audio;                                // 枪声
    float m_shootTimer = 0;
    private AudioSource m_AudioSource;
    private float t = 0;                                     // 计算饱食度的中间变量

    void Start()
    {
        m_transform = this.transform;
        m_ch = this.GetComponent<CharacterController>();
        
        m_camTeansform = Camera.main.transform;              // 获取摄像机
        Vector3 pos = m_transform.position;
        pos.y += m_camHeight;
        m_camTeansform.position = pos;
        m_camTeansform.rotation = m_transform.rotation;
        m_camRot = m_camTeansform.eulerAngles;
        Screen.lockCursor = true;
        m_muzzlepoint = m_camTeansform.FindChild("M16/weapon/muzzlepoint").transform;

        m_AudioSource = GetComponent<AudioSource>();

        
    }

    void Update()
    {
        
        if (m_life <= 0)
        {
            return;
        }
        Control();

        if (t >= hgyTime)
        {
            t = 0;
            StartCoroutine(WaitAndPrintSetHgy(hgyValue));
        }
        t += Time.deltaTime;
        
        m_shootTimer -= Time.deltaTime;
        if (Input.GetMouseButton(0) && m_shootTimer <= 0)
        {
            m_shootTimer = 0.1F;
            m_AudioSource.PlayOneShot(m_audio);
            GameManager.Instance.SetAmmo(1);
            RaycastHit info;
            bool hit = Physics.Raycast(m_muzzlepoint.position,
                m_camTeansform.TransformDirection(Vector3.forward), out info, 100, m_layer);
            if (hit)
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

    }

    void Control()
    {
        float rh = Input.GetAxis("Mouse X");                    // 获得鼠标水平滑动的距离
        float rv = Input.GetAxis("Mouse Y");                    // 获得鼠标垂直滑动的距离
        m_camRot.x -= rv;
        m_camRot.y += rh;
        m_camTeansform.eulerAngles = m_camRot;
        Vector3 camRot = m_camTeansform.eulerAngles;
        camRot.x = 0;
        camRot.z = 0;
        m_transform.eulerAngles = camRot;
        Vector3 pos = m_transform.position;
        pos.y += m_camHeight;
        m_camTeansform.position = pos;

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

        if (Input.GetKeyDown(KeyCode.Space) && m_ch.isGrounded == true)                    // 跳跃
        {
            PlayJumpSound();
            m_movDirection.y = m_jumpSpeed;
            
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))               // 蹲
        {
            gameObject.GetComponent<CharacterController>().height = 0.4f;
            m_transform.position = new Vector3(m_transform.position.x, (m_transform.position.y - 0.4f * 0.75f), m_transform.position.z);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            gameObject.GetComponent<CharacterController>().height = 1.0f;
            m_transform.position = new Vector3(m_transform.position.x, (m_transform.position.y + 0.4f * 0.75f), m_transform.position.z);
        }

        




        /*if (Input.GetMouseButtonDown(0)&& GameObject.FindGameObjectWithTag("Particle System").GetComponent<ParticleSystem>())                            // 射击
        {
            gunlight.active = true;
            GameObject.FindGameObjectWithTag("Particle System").GetComponent<ParticleSystem>().Play();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            gunlight.active = false;
            GameObject.FindGameObjectWithTag("Particle System").GetComponent<ParticleSystem>().Stop();
        }*/
            
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

    /*private void PlayFootStepAudio()
    {
        if (!m_ch.isGrounded)
        {
            return;
        }
        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        int n = Random.Range(1, m_FootstepSounds.Length);
        m_AudioSource.clip = m_FootstepSounds[n];
        m_AudioSource.PlayOneShot(m_AudioSource.clip);
        // move picked sound to index 0 so it's not picked next time
        m_FootstepSounds[n] = m_FootstepSounds[0];
        m_FootstepSounds[0] = m_AudioSource.clip;
    }*/

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
