using UnityEngine;
using FMODUnity;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public SpriteRenderer enemyRenderer;
    public Transform player;
    private NavMeshAgent enemy;

    public float shootInterval = 1f;
    private float lastShootTime;
    public float raycastSpreadAngle = 10f;

    public float enemyHealth = 5f;
    public float damage = 10f;

    public float timeBetweenShots = 1f;
    private float lastShotTime;
    public float maxDistanceToPlayer = 10f;
    public ParticleSystem muzzleFlash;

    private PlayerMovement playerScript;

    private void Start()
    {
        enemyRenderer = GetComponent<SpriteRenderer>();
        player = FindFirstObjectByType<PlayerMovement>().transform;
        playerScript = FindFirstObjectByType<PlayerMovement>();
        enemy = GetComponent<NavMeshAgent>();

        lastShootTime = Time.time - shootInterval;
        lastShotTime = Time.time;
    }

    private void Update()
    {
        enemy.SetDestination(player.position);
        if (enemyHealth <= 0)
        {
            Destroy(gameObject);
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= maxDistanceToPlayer && Time.time - lastShootTime >= shootInterval && Time.time - lastShotTime >= timeBetweenShots)
        {
            ShootAtPlayer();
            lastShootTime = Time.time;
        }
    }

    private void ShootAtPlayer()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, (player.position - transform.position).normalized, out hit))
        {
            if (hit.collider.CompareTag("Player"))
            {
                playerScript.TakeDamage(damage);
                lastShotTime = Time.time; 
            }
        }
        muzzleFlash.Play();
    }

    public void TakeDamage(float damage)
    {
        enemyHealth -= damage;
    }
}
