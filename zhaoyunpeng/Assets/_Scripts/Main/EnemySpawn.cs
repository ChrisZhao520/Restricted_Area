using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public Transform m_enemy;
    public int m_enemyCount = 0;
    public int m_maxEnemy = 3;
    public float m_timer = 0;
    protected Transform m_transform;

    void Start()
    {
        m_transform = this.transform;
        
    }

    void Update()
    {
        if (m_enemyCount >= m_maxEnemy)
        {
            return;
        }
        m_timer -= Time.deltaTime;
        if (m_timer <= 0)
        {
            m_timer = Random.value * 15.0f;
            if (m_timer < 5)
            {
                m_timer = 5;
            }
            Transform obj = (Transform)Instantiate(m_enemy, m_transform.position,Quaternion.identity);
            Enemy enemy = obj.GetComponent<Enemy>();
            enemy.Init(this);
        }




    }
}
