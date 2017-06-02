using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class FollowMe : MonoBehaviour
{
    Player m_player;
    Transform m_transform;

    public GameObject[] Control;
    public GameObject[] PickProps;
    public float chuxiantimer;                                  // 几秒后出现
    public float timer;    
    public float _timerDis;                                     // 几秒后消失
    public float chixutimer;                                    // 动画持续的时间
    public GameObject Jumpthing;
    public GameObject Squartthing;
    public GameObject Runthing;
    public bool Pickprops;
    public bool FinishPick;

    private int i;
    private int j;
    private float _timer;
    private float alpha1;
    private float alpha2;
    
    // Use this for initialization
    void Start()
    {
        i = 0;
        j = 0;
        alpha1 = 0;
        alpha2 = 1;
        _timer = 0;
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        m_transform = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(m_player.m_transform.position.z);
        //Debug.Log(Squartthing.GetComponent<Transform>().position.z);
        _timer += Time.deltaTime;

        if (_timer >= chuxiantimer)
        {

            WaitAnimation();                                             // 执行动画
            
            if (timer < 1.0f && timer >= 0)
            {
                //Debug.Log(alpha);

                if (Input.GetKey(KeyCode.W) && i == 0)                                      // W
                {
                    timer += Time.deltaTime;
                    //Debug.Log(timer);

                }
                else if (Input.GetKeyUp(KeyCode.W) && i == 0)
                {
                    timer = 0;
                }

                if (Input.GetKey(KeyCode.S) && i == 1)                                      // S
                {
                    timer += Time.deltaTime;
                    //Debug.Log(timer);

                }
                else if (Input.GetKeyUp(KeyCode.S) && i == 1)
                {
                    timer = 0;
                }

                if (Input.GetKey(KeyCode.A) && i == 2)                                      // A
                {
                    timer += Time.deltaTime * 1.25f;
                    //Debug.Log(timer);

                }
                else if (Input.GetKeyUp(KeyCode.A) && i == 2)                               
                {
                    timer = 0;
                }

                if (Input.GetKey(KeyCode.D) && i == 3)                                      // D
                {
                    timer += Time.deltaTime * 1.25f;
                    //Debug.Log(timer);

                }
                else if (Input.GetKeyUp(KeyCode.D) && i == 3)                               
                {
                    timer = 0;
                }

                if (i == 4)                                                                 // Tip1
                {
                    
                    Destroy(GameObject.Find("Dangban"));
                    if (m_player.m_transform.position.z < (Jumpthing.GetComponent<Transform>().position.z - 4) || m_player.m_transform.position.z > (Jumpthing.GetComponent<Transform>().position.z - 3))
                    {
                        //Debug.Log(m_player.m_transform.position.z);
                        //Debug.Log(Jumpthing.GetComponent<Transform>().position.z-4);
                        //Debug.Log(Jumpthing.GetComponent<Transform>().position.z-3);
                        timer = 0;
                    }
                    else
                    {
                        timer = 1;
                    }          
                    //Debug.Log(timer);

                }

                /*if (Input.GetKey(KeyCode.Space) && i == 5)                                  
                {
                    
                    //Debug.Log(timer);

                    if (m_player.m_transform.position.z >= 41.7f && m_player.m_transform.position.z <= 43.7f)
                    {
                        timer = 0;
                        //Debug.Log(i);
                    }
                }*/
                if (m_player.m_transform.position.z > (Jumpthing.GetComponent<Transform>().position.z - 2) && i == 5)                     // Jump
                {
                    timer = 1;
                }

                if (i == 6)                                                                 // Tip2
                {

                    if (m_player.m_transform.position.z < (Squartthing.GetComponent<Transform>().position.z - 2) || m_player.m_transform.position.z > Squartthing.GetComponent<Transform>().position.z)
                    {
                        timer = 0;
                    }
                    else
                    {
                        timer = 1;
                    }
                    //Debug.Log(timer);

                }

                /*if (Input.GetKey(KeyCode.LeftControl) && i == 7)                             
                {

                    //Debug.Log(timer);

                    if (m_player.m_transform.position.z >= 56.5f && m_player.m_transform.position.z <= 57.5f)
                    {
                        timer = 0;
                        //Debug.Log(i);
                    }
                }*/
                if (m_player.m_transform.position.z > (Squartthing.GetComponent<Transform>().position.z + 1) && i == 7)                      // Squat
                {
                    timer = 1;
                }

                if (Input.GetKeyUp(KeyCode.H) && i == 8)                                    // OpenFlashLight
                {
                    timer = 1;
                }


                if (i == 9)                                                                 // Run
                {
                    if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
                    {
                        timer += Time.deltaTime * 0.5f;
                        //Debug.Log(timer)
                    }
                    else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.W))
                    {
                        timer = 0;
                    }
                }
                //Debug.Log(i);
                if (Input.GetKeyUp(KeyCode.H) && i == 10)                                    // CloseFlashLight
                {
                    timer = 1;
                    //Debug.Log(timer);
                }

                if (i == 11)
                {
                    timer += Time.deltaTime * 0.25f;
                }

                if (j == 0 && Pickprops)
                {
                    timer = 0;
                    if (FinishPick)
                    {
                        timer = 1;
                        FinishPick = false;
                    }
                    Pickprops = false;
                }
            }

            else if (timer >= 1.0f && Control[i].GetComponent<Text>().color.a >= 0)
            {
                //Debug.Log("123");
                timer = 0f;
                Control[i].GetComponent<Text>().color = new Color(220, 220, 220, 0);
                Control[i].GetComponent<Text>().enabled = false;

                if (i < Control.Length)
                {
                    i++;
                    _timer = 0;
                }

                if (i == 11)
                {
                    chuxiantimer *= 2;
                }
                if (i == 12)
                {
                    chuxiantimer /= 2;
                }

                if (i == Control.Length)
                {
                    timer = -1;
                }
                //Debug.Log(i);
            }
            else if (timer >= 1.0f && PickProps[j].GetComponent<Text>().color.a >= 0)
            {
                //Debug.Log("123");
                timer = 0f;
                PickProps[j].GetComponent<Text>().color = new Color(220, 220, 220, 0);
                PickProps[j].GetComponent<Text>().enabled = false;

                if (j < PickProps.Length)
                {
                    j++;
                    _timer = 0;
                }
            }
        }  
    }

    public void WaitAnimation()
    {
        if (alpha1 <= 1 && i < Control.Length)
        {
            alpha1 += chixutimer;
            alpha2 = 1;
            Control[i].GetComponent<Text>().color = new Color(220, 220, 220, alpha1);
            Control[i].GetComponent<Text>().enabled = true;
        }
        if (alpha1 > 1 && i < Control.Length)
        {
            
            alpha2 -= chixutimer;
            Control[i].GetComponent<Text>().color = new Color(220, 220, 220, alpha2);

            if (alpha2 < 0)
            {
                alpha1 = 0;
                alpha2 = 1;
            }
        }

        if (alpha1 <= 1 && j < PickProps.Length && Pickprops)
        {
            alpha1 += chixutimer;
            alpha2 = 1;
            PickProps[j].GetComponent<Text>().color = new Color(220, 220, 220, alpha1);
            PickProps[j].GetComponent<Text>().enabled = true;
        }
        if (alpha1 > 1 && j < PickProps.Length && Pickprops)
        {

            alpha2 -= chixutimer;
            PickProps[j].GetComponent<Text>().color = new Color(220, 220, 220, alpha2);

            if (alpha2 < 0)
            {
                alpha1 = 0;
                alpha2 = 1;
            }
        }
        else if (!Pickprops)
        {
            PickProps[j].GetComponent<Text>().enabled = false;
        }
    }
}
