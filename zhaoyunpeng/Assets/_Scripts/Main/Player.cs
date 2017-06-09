using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Utility;
using UnityStandardAssets.CrossPlatformInput;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(AudioSource))]
    public class Player : MonoBehaviour
    {
        public bool m_IsWalking;
        public Transform m_transform;
        public CharacterController m_ch;
        public float m_movSpeed = 7.0f;                          // 角色移动速度
        public float m_runSpeed = 12.0f;                         // 角色奔跑速度
        public float m_squatSpeed = 4.0f;                        // 角色蹲走速度
        [Range(0f, 1f)] public float m_RunstepLenghten;          // 走路和跑步时屏幕晃动值
        [Range(0f, 1f)] public float m_ShotstepLenghten;         // 开枪时屏幕晃动值
        public float m_gravity = 2.0f;                           // 重力
        public float m_jumpSpeed = 10.0f;                        // 跳跃速度
        public float m_StickToGroundForce = 20.0f;               // 跳跃时角色受的力
        public int m_life = 100;                                 // 生命值
        public int m_hungry = 100;                               // 饱食度 
        public float hgyTime;                                    // 每隔几秒饱食度递减
        public float hgyRunValue;                                // 跑步每隔几秒饱食度递减    
        public int hgyValue;                                     // 饱食度递减的值
        public MouseLook m_MouseLook;
        public bool m_UseFovKick;
        public FOVKick m_FovKick = new FOVKick();
        public bool m_UseHeadBob;
        public CurveControlledBob m_HeadBob = new CurveControlledBob();
        public LerpControlledBob m_JumpBob = new LerpControlledBob();
        public float m_StepInterval;
        public AudioClip[] m_FootstepSounds;
        public AudioClip m_JumpSound;
        public AudioClip m_LandSound;
        public LayerMask m_layer;
        public Transform m_BulletHole;
        public Transform m_Blood;
        public GameObject m_gun;
        public GameObject m__gun;
        public float m_shootSpeedCD;
        public float m_shootcd;                                  // 射击距离                               
        public bool m_aim;                                       // 瞄准镜头缩进
        public GameObject shotAct;                               // 射击动画
        public GameObject shot_Act;                              
        public GameObject []AmmoCanvas;                          // 画布中的子弹
        public GameObject gameManager;
        public GameObject flashlight;
        public GameObject flashlightaudio;
        public AudioClip FlashAudioClip;
        public GameObject m_backpack;
        public float m_Raycastcd;                                // 拾取范围
        public LayerMask m_Picklayer;
        public Image SightBead;                                  // 准星
        public Sprite SightWait;                                 // 待机准星
        public Sprite SightAttack;                               // 攻击准星
        public GameObject followMe;

        private bool m_Jump;
        private bool m_Jumping;
        private bool m_PreviouslyGrounded;
        private float m_Height;
        private Vector2 m_Input;
        private Vector3 m_OriginalCameraPosition;
        private float m_StepCycle;
        private float m_NextStep;
        private Camera m_Camera;
        private float t = 0;                                     // 计算饱食度的中间变量
        private float ms = 7.0f;
        private Vector3 m_movDirection = Vector3.zero;
        private GameObject m___gun;
        private GameObject __shotAct;
        private float m_gunposX;                                 // 枪最初的位置
        private float m_gunposY;
        private float m_gunposZ;
        private bool aiming;
        private float[] AniCeil = new float[1000];
        private bool drawed = true;
        private Transform m_muzzlepoint;                         // 射线     
        private Transform m_fx;
        private float m_shootTimer = 0;
        private CollisionFlags m_CollisionFlags;
        private AudioSource m_AudioSource;
        private AudioSource FlashAudioSource;

        void Start()
        {
            m_transform = this.transform;
            m_ch = this.GetComponent<CharacterController>();
            m_Camera = Camera.main;
            m_Height = m_ch.height;
            m_Jumping = false;
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle / 2f;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            m_FovKick.Setup(m_Camera);
            m_HeadBob.Setup(m_Camera, m_StepInterval);

            m_gunposX = m_gun.GetComponent<Transform>().localPosition.x;
            m_gunposY = m_gun.GetComponent<Transform>().localPosition.y;
            m_gunposZ = m_gun.GetComponent<Transform>().localPosition.z;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            m_muzzlepoint = m_Camera.transform.FindChild("Rifle_FPS/ShootPoint").transform;
            
            m_AudioSource = GetComponent<AudioSource>();
            FlashAudioSource = flashlightaudio.GetComponent<AudioSource>();
            FlashAudioSource.clip = FlashAudioClip;

            m_MouseLook.Init(transform, m_Camera.transform);
        }

        void Update()
        {
            RotateView();
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
                StartCoroutine(m_JumpBob.DoBobCycle());
                PlayLandingSound();
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
            
            for (int n = 0; n < gameManager.GetComponent<GameManager>().m_minammo.Length; n++)
            {
                if (m_gun.GetComponent<GunProperties>().GunNum == n)
                {
                    m_shootTimer -= Time.deltaTime;
                    if (m_backpack.GetComponent<Canvas>().enabled == false && Time.timeScale != 0)
                    {
                        if (Input.GetMouseButton(0) && 
                            (m_gun.GetComponent<Animation>()["reload"].normalizedTime == 0 || m_gun.GetComponent<Animation>()["reload"].normalizedTime == 1) &&
                            (m_gun.GetComponent<Animation>()["draw"].normalizedTime == 0 || m_gun.GetComponent<Animation>()["draw"].normalizedTime == 1) && 
                            !Input.GetKey(KeyCode.R) && 
                            !Input.GetKey(KeyCode.LeftControl) && 
                            m_shootTimer < 0 && 
                            gameManager.GetComponent<GameManager>().m_minammo[n] != 0 && 
                            drawed &&
                            !m_aim)
                        {
                            //Onfire();                                                         // 开枪
                            SightBead.GetComponent<Image>().overrideSprite = SightAttack;
                            m_shootTimer = m_shootSpeedCD;
                            GameManager.Instance.SetAmmo(1);
                            RaycastHit info;
                            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                            bool hit = Physics.Raycast(ray, out info, m_shootcd, m_layer);
                            if (hit)                                                // 射击
                            {
                                GameObject gameObj = info.collider.gameObject;
                                //Debug.Log("Hit");
                                if (info.transform.tag.CompareTo("enemy") == 0)
                                {
                                    m_fx = m_Blood;
                                    Enemy enemy = info.transform.GetComponent<Enemy>();
                                    enemy.OnDamage(1);
                                    //Debug.Log("enemy's life -1");
                                }
                                else
                                {
                                    m_fx = m_BulletHole;
                                    if (gameObj.tag == "Target")
                                    {
                                        m_gun.GetComponent<AudioSource>().clip = m_gun.GetComponent<GunProperties>().TargetAudio;
                                        m_gun.GetComponent<AudioSource>().Play();
                                        //gameObj.GetComponent<ConstantForce>().enabled = true;
                                    }

                                }
                                Instantiate(m_fx, info.point, info.transform.rotation);
                            }
                            //Debug.Log(aiming);
                            if (!aiming)
                            {
                                m_gun.GetComponent<Animation>().Play("shotBurst");
                            }
                            if (gameManager.GetComponent<GameManager>().m_minammo[n] == 0)
                            {
                                m_UseHeadBob = false;
                                AmmoCanvas[1].GetComponent<Text>().color = new Color(195, 0, 0);
                                m_gun.GetComponent<AudioSource>().clip = m_gun.GetComponent<GunProperties>().AmmoOut;
                                m_gun.GetComponent<AudioSource>().loop = true;
                                StartCoroutine(WaitAndAmmoOut(0.8f));
                                StartCoroutine(WaitAndReload(0.05f));
                                foreach (Transform child in shotAct.transform)
                                {
                                    if (child.GetComponent<ParticleSystem>())
                                    {
                                        child.GetComponent<ParticleSystem>().Stop();
                                    }
                                }
                                SightBead.GetComponent<Image>().overrideSprite = SightWait;
                                m_gun.GetComponent<Animation>().Stop("shotBurst");
                            }
                            else
                            {
                                m_UseHeadBob = true;
                                //AmmoCanvas[1].GetComponent<Text>().color = new Color(255, 255, 255);
                                //m_gun.GetComponent<AudioSource>().loop = false;
                                shotAct.GetComponent<SimpleShootingScript>().enabled = true;
                                foreach (Transform child in shotAct.transform)
                                {
                                    if (child.GetComponent<ParticleSystem>())
                                    {
                                        child.GetComponent<ParticleSystem>().Play();
                                    }
                                }
                            }
                        }
                        else if (Input.GetMouseButton(0) && 
                            (m_gun.GetComponent<Animation>()["reload"].normalizedTime == 0 || m_gun.GetComponent<Animation>()["reload"].normalizedTime == 1) &&
                            (m_gun.GetComponent<Animation>()["draw"].normalizedTime == 0 || m_gun.GetComponent<Animation>()["draw"].normalizedTime == 1) &&
                            !Input.GetKey(KeyCode.LeftControl) && m_shootTimer < 0 &&
                            gameManager.GetComponent<GameManager>().m_minammo[n] != 0 &&
                            drawed &&
                            m_aim)                                                            
                        {
                            //AimFire();                                                         // 瞄准后开枪
                            m_shootTimer = m_shootSpeedCD;
                            GameManager.Instance.SetAmmo(1);
                            RaycastHit info;
                            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                            bool hit = Physics.Raycast(ray, out info, m_shootcd, m_layer);
                            if (hit)                                                // 射击
                            {
                                GameObject gameObj = info.collider.gameObject;
                                //Debug.Log("Hit");
                                if (info.transform.tag.CompareTo("enemy") == 0)
                                {
                                    m_fx = m_Blood;
                                    Enemy enemy = info.transform.GetComponent<Enemy>();
                                    enemy.OnDamage(1);
                                    //Debug.Log("enemy's life -1");
                                }
                                else
                                {
                                    m_fx = m_BulletHole;
                                    if (gameObj.tag == "Target")
                                    {
                                        m_gun.GetComponent<AudioSource>().clip = m_gun.GetComponent<GunProperties>().TargetAudio;
                                        m_gun.GetComponent<AudioSource>().Play();
                                        gameObj.GetComponent<ConstantForce>().enabled = true;
                                    }
                                }
                                Instantiate(m_fx, info.point, info.transform.rotation);
                            }
                            if (gameManager.GetComponent<GameManager>().m_minammo[n] == 0)
                            {
                                m_UseHeadBob = false;
                                AmmoCanvas[1].GetComponent<Text>().color = new Color(195, 0, 0);
                                StartCoroutine(WaitAndReload(0.05f));
                                foreach (Transform child in shotAct.transform)
                                {
                                    if (child.GetComponent<ParticleSystem>())
                                    {
                                        child.GetComponent<ParticleSystem>().Stop();
                                    }
                                }
                                m_gun.GetComponent<Animation>().Stop("shotBurst");
                            }
                            else
                            {
                                m_UseHeadBob = true;
                                shotAct.GetComponent<SimpleShootingScript>().enabled = true;
                                foreach (Transform child in shotAct.transform)
                                {
                                    if (child.GetComponent<ParticleSystem>())
                                    {
                                        child.GetComponent<ParticleSystem>().Play();
                                    }
                                }
                            }
                        }
                        else if (Input.GetMouseButton(0) && 
                            (m_gun.GetComponent<Animation>()["reload"].normalizedTime == 0 || m_gun.GetComponent<Animation>()["reload"].normalizedTime == 1) &&
                            (m_gun.GetComponent<Animation>()["draw"].normalizedTime == 0 || m_gun.GetComponent<Animation>()["draw"].normalizedTime == 1) &&
                            Input.GetKey(KeyCode.LeftControl) &&
                            drawed &&
                            m_shootTimer < 0)
                        {
                            //SquatFire();                                                       // 蹲着开枪
                            SightBead.GetComponent<Image>().overrideSprite = SightWait;
                            m_shootTimer = m_shootSpeedCD;
                            GameManager.Instance.SetAmmo(1);
                            RaycastHit info;
                            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                            bool hit = Physics.Raycast(ray, out info, m_shootcd, m_layer);
                            if (hit)                                                                // 射击
                            {
                                //Debug.Log("Hit");
                                if (info.transform.tag.CompareTo("enemy") == 0)
                                {
                                    m_fx = m_Blood;
                                    Enemy enemy = info.transform.GetComponent<Enemy>();
                                    enemy.OnDamage(1);
                                    //Debug.Log("enemy's life -1");
                                }
                                else
                                {
                                    m_fx = m_BulletHole;
                                }
                                Instantiate(m_fx, info.point, info.transform.rotation);
                            }
                            if (!aiming)
                            {
                                m_gun.GetComponent<Animation>().Play("shotBurst");
                            }
                            if (gameManager.GetComponent<GameManager>().m_minammo[n] == 0)
                            {
                                AmmoCanvas[1].GetComponent<Text>().color = new Color(195, 0, 0);
                                StartCoroutine(WaitAndReload(0.05f));
                                foreach (Transform child in shotAct.transform)
                                {
                                    if (child.GetComponent<ParticleSystem>())
                                    {
                                        child.GetComponent<ParticleSystem>().Stop();
                                    }
                                }
                                m_gun.GetComponent<Animation>().Stop("shotBurst");
                            }
                            else
                            {
                                shotAct.GetComponent<SimpleShootingScript>().enabled = true;
                                foreach (Transform child in shotAct.transform)
                                {
                                    if (child.GetComponent<ParticleSystem>())
                                    {
                                        child.GetComponent<ParticleSystem>().Play();
                                    }
                                }
                            }
                        }
                        else if (Input.GetMouseButtonUp(0))
                        {
                            SightBead.GetComponent<Image>().overrideSprite = SightWait;
                            AmmoCanvas[1].GetComponent<Text>().color = new Color(255, 255, 255);
                            m_gun.GetComponent<AudioSource>().loop = false;
                            foreach (Transform child in shotAct.transform)
                            {
                                if (child.GetComponent<ParticleSystem>())
                                {
                                    child.GetComponent<ParticleSystem>().Stop();
                                }
                            }
                            m_gun.GetComponent<Animation>().Stop("shotBurst");
                        }

                        if (Input.GetMouseButtonDown(1) &&
                            (m_gun.GetComponent<Animation>()["draw"].normalizedTime == 0 || m_gun.GetComponent<Animation>()["draw"].normalizedTime == 1) &&
                            (m_gun.GetComponent<Animation>()["reload"].normalizedTime == 0 || m_gun.GetComponent<Animation>()["reload"].normalizedTime == 1) &&
                            drawed &&
                            !m_aim && 
                            !aiming
                            )                       
                        {
                            //OnAim();                                                            // 开始瞄准
                            m_gun.GetComponent<Animation>()["aimIN"].speed = 3;
                            m_gun.GetComponent<Animation>().Play("aimIN");

                            aiming = true;
                            m_aim = true;
                            m_UseFovKick = false;

                            SightBead.GetComponent<Image>().enabled = false;
                            gameObject.GetComponent<Aim>().enabled = true;
                            //Debug.Log(m_gun.GetComponent<Transform>().localPosition.x);

                            if (gameManager.GetComponent<GameManager>().m_minammo[n] == 0)
                            {

                                shotAct.GetComponent<SimpleShootingScript>().enabled = false;
                                foreach (Transform child in shotAct.transform)
                                {
                                    if (child.GetComponent<ParticleSystem>())
                                    {
                                        child.GetComponent<ParticleSystem>().Stop();
                                    }
                                }
                                m_gun.GetComponent<Animation>().Stop("shotBurst");
                            }
                            else
                            {
                                shotAct.GetComponent<SimpleShootingScript>().enabled = true;
                                foreach (Transform child in shotAct.transform)
                                {
                                    if (child.GetComponent<ParticleSystem>())
                                    {
                                        child.GetComponent<ParticleSystem>().Play();
                                    }
                                }
                            }
                        }
                        else if (Input.GetMouseButtonDown(1) &&
                            (m_gun.GetComponent<Animation>()["draw"].normalizedTime == 0 || m_gun.GetComponent<Animation>()["draw"].normalizedTime == 1) &&
                            (m_gun.GetComponent<Animation>()["reload"].normalizedTime == 0 || m_gun.GetComponent<Animation>()["reload"].normalizedTime == 1) &&
                            drawed &&
                            m_aim && 
                            aiming)
                        {
                            //OffAim();                                                           // 关闭瞄准
                            m_gun.GetComponent<Animation>()["aimOUT"].speed = 3;
                            m_gun.GetComponent<Animation>().Play("aimOUT");

                            aiming = false;
                            m_aim = false;
                            //m_UseFovKick = true;

                            m_gun.GetComponent<Animation>().CrossFade("idle", 1);

                            SightBead.GetComponent<Image>().enabled = true;
                            gameObject.GetComponent<Aim>().enabled = true;
                            //Debug.Log(m_gun.GetComponent<Transform>().localPosition.x);

                            if (gameManager.GetComponent<GameManager>().m_minammo[n] == 0)
                            {
                                shotAct.GetComponent<SimpleShootingScript>().enabled = false;
                                foreach (Transform child in shotAct.transform)
                                {
                                    if (child.GetComponent<ParticleSystem>())
                                    {
                                        child.GetComponent<ParticleSystem>().Stop();
                                    }
                                }
                                m_gun.GetComponent<Animation>().Stop("shotBurst");
                            }
                            else
                            {
                                shotAct.GetComponent<SimpleShootingScript>().enabled = true;
                                foreach (Transform child in shotAct.transform)
                                {
                                    if (child.GetComponent<ParticleSystem>())
                                    {
                                        child.GetComponent<ParticleSystem>().Play();
                                    }
                                }
                            }
                        }

                        if (!m_IsWalking)                                                       // 跑步动画
                        {
                            if (!Input.GetMouseButton(0) && 
                                !Input.GetMouseButton(1) && 
                                !Input.GetKey(KeyCode.R) &&
                                (m_gun.GetComponent<Animation>()["draw"].normalizedTime == 0 || m_gun.GetComponent<Animation>()["draw"].normalizedTime == 1) &&
                                (m_gun.GetComponent<Animation>()["reload"].normalizedTime == 0 || m_gun.GetComponent<Animation>()["reload"].normalizedTime == 1) &&
                                drawed)
                            {
                                m_gun.GetComponent<Animation>().Play("running");
                            }
                            for (int i = 0; i < AniCeil.Length; i++)
                            {
                                AniCeil[i] = 0;
                            }
                        }
                        else 
                        {
                            AniCeil[(int)Mathf.Ceil(m_gun.GetComponent<Animation>()["running"].normalizedTime)] = Mathf.Ceil(m_gun.GetComponent<Animation>()["running"].normalizedTime);
                            for (int i = 0; i <= (int)Mathf.Ceil(m_gun.GetComponent<Animation>()["running"].normalizedTime); i++)
                            {
                                if (AniCeil[i] != 0)
                                {
                                    if (m_gun.GetComponent<Animation>().IsPlaying("running") && m_gun.GetComponent<Animation>()["running"].normalizedTime >= AniCeil[i])
                                    {
                                        m_gun.GetComponent<Animation>().Stop("running");
                                        m_gun.GetComponent<Animation>().CrossFade("idle", 7);
                                    }
                                }
                            }       
                        }

                        if (Input.GetKey(KeyCode.R) || 
                            (GameManager.Instance.m_minammo[n] == 0 && Input.GetMouseButtonUp(0)) || 
                            (GameManager.Instance.m_minammo[n] == 0 && Input.GetKey(KeyCode.W)) &&
                            drawed)
                        {
                            //Reload();                                                                       // 加子弹
                            if (GameManager.Instance.m_minammo[n] != GameManager.Instance._ammo[n] && GameManager.Instance.m_maxammo[n] != 0 && (m_gun.GetComponent<Animation>()["draw"].normalizedTime == 0 || m_gun.GetComponent<Animation>()["draw"].normalizedTime == 1))
                            {                                                                               // 加子弹（提交数据）
                                shotAct.GetComponent<SimpleShootingScript>().enabled = false;
                                m_UseHeadBob = false;

                                foreach (Transform child in shotAct.transform)
                                {
                                    if (child.GetComponent<ParticleSystem>())
                                    {
                                        child.GetComponent<ParticleSystem>().Stop();
                                    }
                                }

                                m_gun.GetComponent<Animation>()["reload"].speed = 1.5f;
                                m_gun.GetComponent<Animation>().Play("reload");
                                m_gun.GetComponent<Animation>().CrossFade("idle", 7);

                                if (m_aim)
                                {
                                    aiming = false;
                                    m_aim = false;
                                    gameObject.GetComponent<Aim>().enabled = true;
                                    SightBead.GetComponent<Image>().enabled = true;
                                }
                                if (GameManager.Instance.m_sumammo[n] < GameManager.Instance._ammo[n])
                                {
                                    GameManager.Instance.m_minammo[n] = GameManager.Instance.m_sumammo[n];
                                    GameManager.Instance.Txt_minammo.text = GameManager.Instance.m_minammo[n].ToString();
                                    GameManager.Instance.Txt_maxammo.text = "0";
                                }
                                else
                                {
                                    GameManager.Instance.m_minammo[n] = GameManager.Instance._ammo[n];
                                    GameManager.Instance.m_maxammo[n] = GameManager.Instance.m_sumammo[n] - GameManager.Instance._ammo[n];
                                    GameManager.Instance.Txt_minammo.text = GameManager.Instance.m_minammo[n].ToString();
                                    GameManager.Instance.Txt_maxammo.text = GameManager.Instance.m_maxammo[n].ToString();
                                }
                            }
                        }

                        if (Input.GetKeyDown(KeyCode.G) && !Input.GetMouseButton(0))                                                
                        {
                            //DrawTools();                                                                    // 丢弃道具
                            if (drawed)
                            {
                                m_gun.GetComponent<Animation>()["holster"].speed = 5f;
                                m_gun.GetComponent<Animation>().Play("holster");

                                drawed = false;
                                m_UseHeadBob = false;

                                if (m_aim && aiming)
                                {
                                    aiming = false;
                                    m_aim = false;
                                    SightBead.GetComponent<Image>().enabled = true;
                                    gameObject.GetComponent<Aim>().enabled = true;
                                }

                                foreach (Transform child in shotAct.transform)
                                {
                                    if (child.GetComponent<ParticleSystem>())
                                    {
                                        child.GetComponent<ParticleSystem>().Stop();
                                    }
                                }
                                for (int i = 0; i < AmmoCanvas.Length; i++)
                                {
                                    AmmoCanvas[i].SetActive(false);
                                }

                            }
                            else
                            {
                                m_gun.SetActive(false);
                                m___gun = m_gun;
                                GetComponent<Aim>().m_gun = m__gun;
                                m_gun = m__gun;
                                m__gun = m___gun;

                                __shotAct = shotAct;
                                shotAct = shot_Act;
                                shot_Act = __shotAct;

                                m_gun.SetActive(true);
                                m_gun.GetComponent<Animation>()["draw"].speed = 1.5f;
                                m_gun.GetComponent<Animation>().Play("draw");
                                drawed = true;
                                m_UseHeadBob = false;

                                for (int i = 0; i < AmmoCanvas.Length; i++)
                                {
                                    AmmoCanvas[i].SetActive(true);
                                }
                                m_gun.GetComponent<Animation>().CrossFade("idle", 1.5f);
                                m_gun.GetComponent<GunProperties>().GunUpdate();
                                n++;
                            }
                            shotAct.GetComponent<SimpleShootingScript>().enabled = false;
                            foreach (Transform child in shotAct.transform)
                            {
                                if (child.GetComponent<ParticleSystem>())
                                {
                                    child.GetComponent<ParticleSystem>().Stop();
                                }
                            }
                            /*if (!Input.GetMouseButton(0))
                            {
                                foreach (Transform child in shotAct.transform)
                                {
                                    if (child.GetComponent<ParticleSystem>())
                                    {
                                        child.GetComponent<ParticleSystem>().Play();
                                    }
                                }
                            }*/
                        }

                        if (Input.GetKeyDown(KeyCode.H))                                             // 打开关闭手电筒
                        {
                            //Debug.Log("H");

                            //Debug.Log(flashlightaudio.active);

                            if (flashlight.active == true)
                            {
                                //Debug.Log("Close");  
                                flashlight.SetActive(false);
                                FlashAudioSource.Play();
                            }
                            else
                            {
                                //Debug.Log("Open");
                                flashlight.SetActive(true);
                                FlashAudioSource.Play();
                            }
                            //Debug.Log(flashlightaudio.active);
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.I))                            // 打开关闭背包
                    {
                        if (m_backpack.GetComponent<Canvas>().enabled == true)
                        {
                            Cursor.visible = false; 
                            Cursor.lockState = CursorLockMode.Locked;
                            m_backpack.GetComponent<Canvas>().enabled = false;
                        }
                        else
                        {
                            Cursor.visible = true;
                            Cursor.lockState = CursorLockMode.None;
                            m_backpack.GetComponent<Canvas>().enabled = true;
                        }
                    }

                    RaycastHit infoF;                                             // 拾取道具检测
                    Ray raypick = Camera.main.ScreenPointToRay(Input.mousePosition);
                    bool hitF = Physics.Raycast(raypick, out infoF, m_Raycastcd, m_Picklayer);
                    //Debug.Log(m_Picklayer);
                    if (hitF)
                    {
                        GameObject gameObjF = infoF.collider.gameObject;
                        //Debug.Log(gameObjF.layer);
                        if (gameObjF.layer == 11)
                        {
                            //Debug.Log("123");
                            followMe.GetComponent<FollowMe>().Pickprops = true;
                        }
                        else
                        {
                            followMe.GetComponent<FollowMe>().FinishPick = false;
                        }
                    
                    }
                    if (Input.GetKeyDown(KeyCode.F))                                // 拾取道具
                    {
                        Debug.DrawLine(m_muzzlepoint.position, infoF.point, Color.red, 2);
                        if (hitF)
                        {
                            //划出射线，只有在scene视图中才能看到
                            GameObject gameObj = infoF.collider.gameObject;
                            if (gameObj.tag == "M4A1")
                            {
                                //Debug.Log(gameObj.tag);
                                backpack_manger.Instancce.StoreItem(0);
                                Destroy(gameObj);
                                return;
                            }

                            if (gameObj.tag == "bishou")
                            {

                                backpack_manger.Instancce.StoreItem(1);
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

                            if (gameObj.layer == 11)
                            {
                                //Debug.Log("123");
                                followMe.GetComponent<FollowMe>().FinishPick = true;
                            }
                            else
                            {
                                followMe.GetComponent<FollowMe>().Pickprops = false;
                            }
                        }
                    }
                }
            }

            
        }
        void FixedUpdate()
        {
            float speed;
            GetInput(out speed);

            Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;

            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, m_ch.radius, Vector3.down, out hitInfo,
                               m_ch.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            m_movDirection.x = desiredMove.x * speed;
            m_movDirection.z = desiredMove.z * speed;

            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.Space)) && !Input.GetKey(KeyCode.LeftControl))
            {
                SightBead.GetComponent<Image>().overrideSprite = SightAttack;
            }
            else if ((Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.Space)) && m_backpack.GetComponent<Canvas>().enabled == false && Time.timeScale != 0)
            {
                SightBead.GetComponent<Image>().overrideSprite = SightWait;
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
                m_movDirection += Physics.gravity * m_gravity * Time.fixedDeltaTime;
            }

            m_CollisionFlags = m_ch.Move(m_movDirection * Time.fixedDeltaTime);

            ProgressStepCycle(speed);
            UpdateCameraPosition(speed);

            m_MouseLook.UpdateCursorLock();
        }


        /*public void Onfire()
        {
            SightBead.GetComponent<Image>().overrideSprite = SightAttack;
            m_shootTimer = m_shootSpeedCD;    
            GameManager.Instance.SetAmmo(1);
            RaycastHit info;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool hit = Physics.Raycast(ray, out info, m_shootcd, m_layer);
            if (hit)                                                // 射击
            {
                GameObject gameObj = info.collider.gameObject;
                //Debug.Log("Hit");
                if (info.transform.tag.CompareTo("enemy") == 0)
                {
                    m_fx = m_Blood;
                    Enemy enemy = info.transform.GetComponent<Enemy>();
                    enemy.OnDamage(1);
                    //Debug.Log("enemy's life -1");
                }
                else
                {
                    m_fx = m_BulletHole;
                    if (gameObj.tag == "Target")
                    {
                        m_gun.GetComponent<AudioSource>().clip = m_gun.GetComponent<GunProperties>().TargetAudio;
                        m_gun.GetComponent<AudioSource>().Play();
                        //gameObj.GetComponent<ConstantForce>().enabled = true;
                    }

                }
                Instantiate(m_fx, info.point, info.transform.rotation);
            }
            //Debug.Log(aiming);
            if (!aiming)
            {
                m_gun.GetComponent<Animation>().Play("shotBurst");
            }
            if (gameManager.GetComponent<GameManager>().m_minammo == 0)
            {
                m_UseHeadBob = false;
                AmmoCanvas[1].GetComponent<Text>().color = new Color(195, 0, 0);
                m_gun.GetComponent<AudioSource>().clip = m_gun.GetComponent<GunProperties>().AmmoOut;
                m_gun.GetComponent<AudioSource>().loop = true;
                StartCoroutine(WaitAndAmmoOut(0.8f));
                StartCoroutine(WaitAndReload(0.05f));
                foreach (Transform child in shotAct.transform)
                {
                    if (child.GetComponent<ParticleSystem>())
                    {
                        child.GetComponent<ParticleSystem>().Stop();
                    }
                }
                SightBead.GetComponent<Image>().overrideSprite = SightWait;
                m_gun.GetComponent<Animation>().Stop("shotBurst");
            }
            else
            {
                m_UseHeadBob = true;
                //AmmoCanvas[1].GetComponent<Text>().color = new Color(255, 255, 255);
                //m_gun.GetComponent<AudioSource>().loop = false;
                shotAct.GetComponent<SimpleShootingScript>().enabled = true;
                foreach (Transform child in shotAct.transform)
                {
                    if (child.GetComponent<ParticleSystem>())
                    {
                        child.GetComponent<ParticleSystem>().Play();
                    }
                }
            }
        }

        public void AimFire()
        {
            m_shootTimer = m_shootSpeedCD;
            GameManager.Instance.SetAmmo(1);
            RaycastHit info;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool hit = Physics.Raycast(ray, out info, m_shootcd, m_layer);
            if (hit)                                                // 射击
            {
                GameObject gameObj = info.collider.gameObject;
                //Debug.Log("Hit");
                if (info.transform.tag.CompareTo("enemy") == 0)
                {
                    m_fx = m_Blood;
                    Enemy enemy = info.transform.GetComponent<Enemy>();
                    enemy.OnDamage(1);
                    //Debug.Log("enemy's life -1");
                }
                else
                {
                    m_fx = m_BulletHole;
                    if (gameObj.tag == "Target")
                    {
                        m_gun.GetComponent<AudioSource>().clip = m_gun.GetComponent<GunProperties>().TargetAudio;
                        m_gun.GetComponent<AudioSource>().Play();
                        gameObj.GetComponent<ConstantForce>().enabled = true;
                    }
                }
                Instantiate(m_fx, info.point, info.transform.rotation);
            }
            if (gameManager.GetComponent<GameManager>().m_minammo == 0)
            {
                m_UseHeadBob = false;
                AmmoCanvas[1].GetComponent<Text>().color = new Color(195, 0, 0);
                StartCoroutine(WaitAndReload(0.05f));
                foreach (Transform child in shotAct.transform)
                {
                    if (child.GetComponent<ParticleSystem>())
                    {
                        child.GetComponent<ParticleSystem>().Stop();
                    }
                }
                m_gun.GetComponent<Animation>().Stop("shotBurst");
            }
            else
            {
                m_UseHeadBob = true;
                shotAct.GetComponent<SimpleShootingScript>().enabled = true;
                foreach (Transform child in shotAct.transform)
                {
                    if (child.GetComponent<ParticleSystem>())
                    {
                        child.GetComponent<ParticleSystem>().Play();
                    }
                }
            }
        }

        public void SquatFire()
        {
            SightBead.GetComponent<Image>().overrideSprite = SightWait;
            m_shootTimer = m_shootSpeedCD;
            GameManager.Instance.SetAmmo(1);
            RaycastHit info;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool hit = Physics.Raycast(ray, out info, m_shootcd, m_layer);
            if (hit)                                                                // 射击
            {
                //Debug.Log("Hit");
                if (info.transform.tag.CompareTo("enemy") == 0)
                {
                    m_fx = m_Blood;
                    Enemy enemy = info.transform.GetComponent<Enemy>();
                    enemy.OnDamage(1);
                    //Debug.Log("enemy's life -1");
                }
                else
                {
                    m_fx = m_BulletHole;
                }
                Instantiate(m_fx, info.point, info.transform.rotation);
            }
            if (!aiming)
            {
                m_gun.GetComponent<Animation>().Play("shotBurst");
            }
            if (gameManager.GetComponent<GameManager>().m_minammo == 0)
            {
                AmmoCanvas[1].GetComponent<Text>().color = new Color(195, 0, 0);
                StartCoroutine(WaitAndReload(0.05f));
                foreach (Transform child in shotAct.transform)
                {
                    if (child.GetComponent<ParticleSystem>())
                    {
                        child.GetComponent<ParticleSystem>().Stop();
                    }
                }
                m_gun.GetComponent<Animation>().Stop("shotBurst");
            }
            else
            {
                shotAct.GetComponent<SimpleShootingScript>().enabled = true;
                foreach (Transform child in shotAct.transform)
                {
                    if (child.GetComponent<ParticleSystem>())
                    {
                        child.GetComponent<ParticleSystem>().Play();
                    }
                }
            }
        }

        public void OnAim()
        {
            m_gun.GetComponent<Animation>()["aimIN"].speed = 3;
            m_gun.GetComponent<Animation>().Play("aimIN");

            aiming = true;
            m_aim = true;
            m_UseFovKick = false;

            SightBead.GetComponent<Image>().enabled = false;
            gameObject.GetComponent<Aim>().enabled = true;
            //Debug.Log(m_gun.GetComponent<Transform>().localPosition.x);

            if (gameManager.GetComponent<GameManager>().m_minammo == 0)
            {

                shotAct.GetComponent<SimpleShootingScript>().enabled = false;
                foreach (Transform child in shotAct.transform)
                {
                    if (child.GetComponent<ParticleSystem>())
                    {
                        child.GetComponent<ParticleSystem>().Stop();
                    }
                }
                m_gun.GetComponent<Animation>().Stop("shotBurst");
            }
            else
            {
                shotAct.GetComponent<SimpleShootingScript>().enabled = true;
                foreach (Transform child in shotAct.transform)
                {
                    if (child.GetComponent<ParticleSystem>())
                    {
                        child.GetComponent<ParticleSystem>().Play();
                    }
                }
            }
        }

        public void OffAim()
        {
            m_gun.GetComponent<Animation>()["aimOUT"].speed = 3;
            m_gun.GetComponent<Animation>().Play("aimOUT");

            aiming = false;
            m_aim = false;
            //m_UseFovKick = true;

            m_gun.GetComponent<Animation>().CrossFade("idle", 1);

            SightBead.GetComponent<Image>().enabled = true;
            gameObject.GetComponent<Aim>().enabled = true;
            //Debug.Log(m_gun.GetComponent<Transform>().localPosition.x);

            if (gameManager.GetComponent<GameManager>().m_minammo == 0)
            {
                shotAct.GetComponent<SimpleShootingScript>().enabled = false;
                foreach (Transform child in shotAct.transform)
                {
                    if (child.GetComponent<ParticleSystem>())
                    {
                        child.GetComponent<ParticleSystem>().Stop();
                    }
                }
                m_gun.GetComponent<Animation>().Stop("shotBurst");
            }
            else
            {
                shotAct.GetComponent<SimpleShootingScript>().enabled = true;
                foreach (Transform child in shotAct.transform)
                {
                    if (child.GetComponent<ParticleSystem>())
                    {
                        child.GetComponent<ParticleSystem>().Play();
                    }
                }
            }
        }

        public void Reload()
        {
            if (GameManager.Instance.m_minammo != GameManager.Instance._ammo && GameManager.Instance.m_maxammo != 0 && (m_gun.GetComponent<Animation>()["draw"].normalizedTime == 0 || m_gun.GetComponent<Animation>()["draw"].normalizedTime == 1))
            {                                                                               // 加子弹（提交数据）
                shotAct.GetComponent<SimpleShootingScript>().enabled = false;
                m_UseHeadBob = false;

                foreach (Transform child in shotAct.transform)
                {
                    if (child.GetComponent<ParticleSystem>())
                    {
                        child.GetComponent<ParticleSystem>().Stop();
                    }
                }

                m_gun.GetComponent<Animation>()["reload"].speed = 1.5f;
                m_gun.GetComponent<Animation>().Play("reload");
                m_gun.GetComponent<Animation>().CrossFade("idle", 7);

                if (m_aim)
                {
                    aiming = false;
                    m_aim = false;
                    gameObject.GetComponent<Aim>().enabled = true;
                    SightBead.GetComponent<Image>().enabled = true;
                }
                if (GameManager.Instance.m_sumammo < GameManager.Instance._ammo)
                {
                    GameManager.Instance.m_minammo = GameManager.Instance.m_sumammo;
                    GameManager.Instance.Txt_minammo.text = GameManager.Instance.m_minammo.ToString();
                    GameManager.Instance.Txt_maxammo.text = "0";
                }
                else
                {
                    GameManager.Instance.m_minammo = GameManager.Instance._ammo;
                    GameManager.Instance.m_maxammo = GameManager.Instance.m_sumammo - GameManager.Instance._ammo;
                    GameManager.Instance.Txt_minammo.text = GameManager.Instance.m_minammo.ToString();
                    GameManager.Instance.Txt_maxammo.text = GameManager.Instance.m_maxammo.ToString();
                }
            }
        }

        public void DrawTools()
        {
            if (drawed)
            {
                m_gun.GetComponent<Animation>()["holster"].speed = 5f;
                m_gun.GetComponent<Animation>().Play("holster");

                drawed = false;
                m_UseHeadBob = false;

                if (m_aim && aiming)
                {
                    aiming = false;
                    m_aim = false;
                    SightBead.GetComponent<Image>().enabled = true;
                    gameObject.GetComponent<Aim>().enabled = true;
                }

                foreach (Transform child in shotAct.transform)
                {
                    if (child.GetComponent<ParticleSystem>())
                    {
                        child.GetComponent<ParticleSystem>().Stop();
                    }
                }
                for (int i = 0; i < AmmoCanvas.Length; i++)
                {
                    AmmoCanvas[i].SetActive(false);
                }

            }
            else
            {
                m_gun.SetActive(false);
                m___gun = m_gun;
                GetComponent<Aim>().m_gun = m__gun;
                m_gun = m__gun;
                m__gun = m___gun;

                __shotAct = shotAct;
                shotAct = shot_Act;
                shot_Act = __shotAct;

                m_gun.SetActive(true);
                m_gun.GetComponent<Animation>()["draw"].speed = 1.5f;
                m_gun.GetComponent<Animation>().Play("draw");
                drawed = true;
                m_UseHeadBob = true;
                for (int i = 0; i < AmmoCanvas.Length; i++)
                {
                    AmmoCanvas[i].SetActive(true);
                }
                m_gun.GetComponent<Animation>().CrossFade("idle", 1.5f);
                m_gun.GetComponent<GunProperties>().GunUpdate();
            }
            shotAct.GetComponent<SimpleShootingScript>().enabled = false;
            if (!Input.GetMouseButton(0))
            {
                foreach (Transform child in shotAct.transform)
                {
                    if (child.GetComponent<ParticleSystem>())
                    {
                        child.GetComponent<ParticleSystem>().Play();
                    }
                }
            }
        }
        */



        public void OnDamage(int damage)
        {
            m_life -= damage;
            GameManager.Instance.SetLife(m_life);
            if (m_life <= 0)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
        private void Control()
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))               // 蹲
            {
                m_UseHeadBob = false;
                m_movSpeed = m_squatSpeed;
                m_ch.height = m_Height - 0.8f - m_ch.skinWidth * 2;
                m_transform.position = new Vector3(m_transform.position.x, (m_transform.position.y - (m_Height - 0.8f) * 0.75f), m_transform.position.z);
            }
            else if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                
                m_movSpeed = ms;
                m_ch.height = m_Height;
                m_transform.position = new Vector3(m_transform.position.x, (m_transform.position.y + (m_Height - 0.8f) * 0.75f), m_transform.position.z);
                //StartCoroutine(WaitAndPrintSquat(0.4f));
                m_UseHeadBob = true;
            }
        }

        private void PlayJumpSound()
        {
            m_AudioSource.clip = m_JumpSound;
            m_AudioSource.Play();
        }

        private void PlayLandingSound()
        {
            m_AudioSource.clip = m_LandSound;
            m_AudioSource.Play();
            m_NextStep = m_StepCycle + .5f;
        }

        private void ProgressStepCycle(float speed)
        {
            if (m_ch.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_StepCycle += (m_ch.velocity.magnitude + (speed * (m_IsWalking ? 1f : m_RunstepLenghten))) *
                             Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            PlayFootStepAudio();
        }

        private void PlayFootStepAudio()
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
        }

        private void UpdateCameraPosition(float speed)
        {

            Vector3 newCameraPosition;
            if (!m_UseHeadBob)
            {
                return;
            }
            if (m_ch.velocity.magnitude > 0 && m_ch.isGrounded && !m_aim)
            {
                m_Camera.transform.localPosition =
                    m_HeadBob.DoHeadBob(m_ch.velocity.magnitude +
                                      (speed * (m_IsWalking ? 1f : m_RunstepLenghten)));
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
            }
            else if (Input.GetMouseButton(0) && m_backpack.GetComponent<Canvas>().enabled == false && Time.timeScale != 0 && !m_aim)
            {
                m_Camera.transform.localPosition =
                    m_HeadBob.DoHeadBob(m_ch.velocity.magnitude +
                                      (speed * (m_IsWalking ? 1f : m_ShotstepLenghten)));
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
            }
            else
            {
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
            }
            if (!m_aim)
            {
                m_Camera.transform.localPosition = newCameraPosition;
            }
        }

        private void GetInput(out float speed)
        {
            // Read input
            float horizontal = CrossPlatformInputManager.GetAxisRaw("Horizontal");
            float vertical = CrossPlatformInputManager.GetAxisRaw("Vertical");

            bool waswalking = m_IsWalking;

#if !MOBILE_INPUT
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
            m_IsWalking = !(Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) && !Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !m_aim);
            
#endif
            // set the desired speed to be walking or running
            speed = m_IsWalking ? m_movSpeed : m_runSpeed;
            m_Input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            if (m_IsWalking != waswalking && m_UseFovKick && m_ch.velocity.sqrMagnitude > 0)
            {
                StopAllCoroutines();
                StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
            }
        }

        private void RotateView()
        {
            m_MouseLook.LookRotation(transform, m_Camera.transform);
        }

        private void OnControllerColliderHit(ControllerColliderHit cchit)
        {
            Rigidbody body = cchit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            body.AddForceAtPosition(m_ch.velocity * 0.1f, cchit.point, ForceMode.Impulse);
        } 

        IEnumerator WaitAndPrintSetHgy(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);

            //等待之后执行的动作  
            GameManager.Instance.SetHgy(hgyValue);

        }

        IEnumerator WaitAndReload(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);

            //等待之后执行的动作  
            shotAct.GetComponent<SimpleShootingScript>().enabled = false;

        }
        IEnumerator WaitAndAmmoOut(float waitTime)
        {
            m_gun.GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(waitTime);

            //等待之后执行的动作  
            

        }
        /*IEnumerator WaitAndPrintSquat(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);

            //等待之后执行的动作  
            m_UseHeadBob = true;
        }*/
    }

}
