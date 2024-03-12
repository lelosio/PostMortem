using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float enemyHealth = 2f;
    //public float damage = 0.5f;
    public EnemyManager enemyManager;
    void Start()
    {
        
    }

  
    void Update()
    {
        if (enemyHealth <= 0)
        {
            enemyManager.RemoveEnemy(this);
            Destroy(gameObject);    
        }
    }

    public void TakeDamage(float damage)
    {
        enemyHealth -= damage;
    }
}
