using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.FirstPerson;

public class Enemy : MonoBehaviour {

    Transform m_transform;
    Animator m_ani;
    Player m_player;
    NavMeshAgent m_agent;
    public int m_life;
    public float m_movSpeed;
    public float m_rotSpeed;
    public int m_attacklife;

    private float m_timer = 2;

    protected EnemySpawn m_spawn;
    
    void Start()
    {
        m_transform = this.transform;
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        m_ani = this.GetComponent<Animator>();
        m_agent = GetComponent<NavMeshAgent>();
        m_agent.speed = m_movSpeed;
        m_agent.SetDestination(m_player.m_transform.position);
    }
    
    void Update()
    {
        if (m_player.m_life <= 0)
        {
            return;
        }
        AnimatorStateInfo stateInfo = m_ani.GetCurrentAnimatorStateInfo(0);
        //Debug.Log(m_life);
        // 如果处于待机状态
        if (stateInfo.nameHash == Animator.StringToHash("Base Layer.idle") && !m_ani.IsInTransition(0))
        {
            m_ani.SetBool("idle", false);
            m_timer -= Time.deltaTime;

            if (m_timer > 0)
            {
                return;
            }
            if (Vector3.Distance(m_transform.position, m_player.m_transform.position) < (2.0f + m_player.m_ch.skinWidth)) 
            {
                m_ani.SetBool("attack", true);
            }
            else
            {
                m_timer = 1;
                m_agent.SetDestination(m_player.m_transform.position);
                m_ani.SetBool("walk", true);
            }

        }
        // 如果处于走路状态
        if (stateInfo.nameHash == Animator.StringToHash("Base Layer.walk") && !m_ani.IsInTransition(0))
        {
            m_ani.SetBool("walk", false);
            m_timer -= Time.deltaTime;
            if (m_timer < 0)
            {
                //Debug.Log(m_agent.speed);
                m_agent.Resume();
                m_agent.SetDestination(m_player.m_transform.position);
                m_timer = 1;
            }
            if (Vector3.Distance(m_transform.position, m_player.m_transform.position) <= 30.0f)
            {
                m_agent.speed += 4;
                m_agent.Stop();
                m_ani.SetBool("run", true);
            }
        }
        // 如果处于奔跑状态
        if (stateInfo.nameHash == Animator.StringToHash("Base Layer.run") && !m_ani.IsInTransition(0))
        {
            
            m_ani.SetBool("run", false);
            m_timer -= Time.deltaTime;
            if (m_timer < 0)
            {
                //Debug.Log(m_agent.speed);
                m_agent.Resume();
                m_agent.SetDestination(m_player.m_transform.position);
                m_timer = 1;
            }
            if (Vector3.Distance(m_transform.position, m_player.m_transform.position) > 30.0f)
            {
                m_agent.speed -= 4;
                m_agent.Stop();
                m_ani.SetBool("walk", true);
            }
            if (Vector3.Distance(m_transform.position, m_player.m_transform.position) <= (2.0f + m_player.m_ch.skinWidth))
            {
                m_agent.Stop();
                m_ani.SetBool("attack", true);
            }
        }
        // 如果处于攻击状态
        if (stateInfo.nameHash == Animator.StringToHash("Base Layer.attack") && !m_ani.IsInTransition(0))
        {
            RotateTo();
            m_ani.SetBool("attack", false);
            if (stateInfo.normalizedTime >= 1.0f)
            {
                m_ani.SetBool("run", true);
                m_timer = 0;
                m_player.OnDamage(m_attacklife);
            }
        }
        // 如果处于死亡状态
        if (stateInfo.nameHash == Animator.StringToHash("Base Layer.death") && !m_ani.IsInTransition(0))
        {
            if (stateInfo.normalizedTime >= 1.0f)
            {
                m_spawn.m_enemyCount--;
                GameManager.Instance.SetDestroyEnemy(1);
                Destroy(this.gameObject);
            }
        }
    }

    void RotateTo()
    {
        Vector3 targetdir = m_player.m_transform.position - m_transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetdir, m_rotSpeed * Time.deltaTime, 0.0f);
        m_transform.rotation = Quaternion.LookRotation(newDir);
    }

    public void OnDamage(int damage)
    {
        m_life -= damage;
        if (m_life <= 0)
        {
            m_ani.SetBool("death", true);
        }
    }

    public void Init(EnemySpawn spawn)
    {
        m_spawn = spawn;
        m_spawn.m_enemyCount++;
    }



}
