using UnityEngine;
using TMPro;
using System.Security.Cryptography;

public class GunSystem : MonoBehaviour
{
    // Gun stats
    public int damage;
    public float timeBetweenShooting, spread, range;
    private float timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    private int bulletsLeft, bulletsShot;
    public float ImpactForce = 10f;

    // Bools 
    bool shooting, readyToShoot, reloading;

    // Reference
    public Camera fpsCam;
    public Transform attackPoint;
    public ParticleSystem muzzleFlash;
    public GameObject hitEffect;
    public TextMeshProUGUI text;

    
    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    public void Update()
    {
        text.SetText(bulletsLeft + "/" + 0);
        MyInput();
    }


    private void MyInput()
    {
        if (allowButtonHold)
            shooting = Input.GetMouseButton(0);
        else
            shooting = Input.GetMouseButtonDown(0); // Changed GetMouseButton to GetMouseButtonDown

            
        //Shoot
        if (readyToShoot && shooting && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        Vector3 direction = fpsCam.transform.forward;

        //rotacja rozrzutu
        direction += fpsCam.transform.right * x;
        direction += fpsCam.transform.up * y;

        muzzleFlash.Play();

        // Draw the raycast
        Debug.DrawRay(fpsCam.transform.position, direction * range, Color.red, 0.1f);

        // Raycast
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, direction, out hit, range))
        {
            Debug.Log(hit.transform.name);
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            PlayerMovement player = hit.transform.GetComponent<PlayerMovement>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * ImpactForce);
            }
            GameObject ImpactGo = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(ImpactGo, 2f);

            // Pause the game
            //Time.timeScale = 0f;
        }

        bulletsShot--; 

        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
        else
            Invoke("ResetShot", timeBetweenShooting);

        bulletsLeft--;
    }






    private void ResetShot()
    {
        readyToShoot = true;
    }
}