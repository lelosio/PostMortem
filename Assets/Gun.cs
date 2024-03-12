using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public LayerMask raycastLayerMask;
    public float range = 20f;
    public float verticalRange = 20f;

    public float damage = 1f;

    public float fireRate;

    private float nextTimeToFire;
    private BoxCollider gunTrigger;

    public EnemyManager enemyManager;
    void Start()
    {
        gunTrigger = GetComponent<BoxCollider>();
        gunTrigger.size = new Vector3(1, verticalRange, range);
        gunTrigger.center = new Vector3(0, 0, range * 0.5f);
    }


    void Update()
    {
        if(Input.GetMouseButtonDown(0)&& Time.time > nextTimeToFire)
        {
            Fire();
            // Debug.Log("Fire!");
        }
        
    }
    public void Fire()
    {
        //damage enemies
        foreach (var enemy in enemyManager.enemiesInTrigger)
        {
            var dir = enemy.transform.position - transform.position;
            RaycastHit hit;
            if(Physics.Raycast(transform.position, dir, out hit, range * 1.5f, raycastLayerMask))
            {
                if (hit.transform == enemy.transform)
                {
                    enemy.TakeDamage(damage);
                    // Debug.DrawRay(transform.position,dir,Color.green);
                    // Debug.Break();
                    // Debug.Log("Enemy Hit!");
                }
            }
        }

        //firerate
        nextTimeToFire = Time.time + fireRate;
    }
    
    private void OnTriggerEnter(Collider other) {
        //add potential enemy to shoot
        Enemy enemy = other.transform.GetComponent<Enemy>();

        if (enemy)
        {
            enemyManager.AddEnemy(enemy);
        }
    }

    private void OnTriggerExit(Collider other) {
        //remove enemy when shoot
        Enemy enemy = other.transform.GetComponent<Enemy>();

        if (enemy)
        {
            enemyManager.RemoveEnemy(enemy);
        }
    }
}
