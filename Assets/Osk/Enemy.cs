using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    
    public float enemyHealth = 5f;
    public SpriteRenderer enemyRenderer; 
    public EnemyManager enemyManager;
    public Transform playersTransform;
    private NavMeshAgent enemyNavMeshAgent;
    public Material aggroMat;
    public GameObject bullet;


    //Dane
    public float awarenessRadius = 8f;
    public bool isAggro;
    public float damage = 10f;
    public float bulletSpeed;
  
    private void Start()
    {
        enemyRenderer = GetComponent<SpriteRenderer>();
        playersTransform = FindObjectOfType<PlayerMovement>().transform;
        enemyNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void Update()
    {
        MoveEnemy();
        if (enemyHealth <= 0)
        {
            enemyManager.RemoveEnemy(this);
            Destroy(gameObject);
        }
        if (isAggro)
        {
            enemyNavMeshAgent.SetDestination(playersTransform.position);
        }
    }

    public void TakeDamage(float damage)
    {
        enemyHealth -= damage;
    }
    public void MoveEnemy()
    {
        var dist = Vector3.Distance(transform.position, playersTransform.position);
        if (dist < awarenessRadius)
        {
            isAggro = true;
        }
    }
    public void Shoot()
    {
        if (isAggro) 
        {
            Instantiate(bullet, playersTransform.position, Quaternion.identity);
        }
    }
}