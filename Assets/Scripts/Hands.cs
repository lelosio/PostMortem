using UnityEditor.Animations;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class Hands : MonoBehaviour
{
    [Header("Sway Settings")]
    public float smooth;
    public float swayAmount;
    private float xMousePos;
    private float yMousePos;
    public Transform anchor;

    public GameObject bulletHolePrefab;
    public GameObject bulletHoleContainer;
    public float destroyDelay;

    public AnimatorController pistolAnim;
    public AnimatorController shotgunAnim;
    public Animator hand;
    public Gun[] Inventory = new Gun[4];
    public Gun equippedGun;

    public Transform muzzleFlash;
    public Animator muzzleFlashAnimator;
    public Light muzzleFlashLight;
    public Light muzzleFlashFlash;
    public ParticleSystem gunImpact;

    int selectedGun;
    private float flashIntensity;
    private float lightIntensity;

    // OSKAR
    //==============
    public TextMeshProUGUI ammoUI;
    bool reloading;                                                          // UNUSED BOOLS?
    bool readyToShoot;

    public struct Gun
    {
        public string name;
        public int ammo;
        public int magSize;
        public float spread;
        public AnimatorController ac;
        public bool acquired;
        public int bulletsLeft;
        public int bulletsShot;
        public float shootingCooldown;
        public float reloadTime;

        public Gun(string Name, int Ammo, int MagSize, float Spread, float ReloadTime, float ShootingCooldown, AnimatorController AC, bool Acquired)
        {
            name = Name;
            ammo = Ammo;
            magSize = MagSize;
            spread = Spread;
            reloadTime = ReloadTime;
            shootingCooldown = ShootingCooldown;

            ac = AC;
            acquired = Acquired;
        
            bulletsShot = 0;
            bulletsLeft = magSize;
        }
    }
    
    void Start()
    {
        Gun pistol = new Gun("Pistol", 64, 12, 1.2f, 0.5f, 1f, pistolAnim, true);
        Gun shotgun = new Gun("Shotgun", 32, 6, 1.2f, 0.5f, 1f, shotgunAnim, true);
        Gun submachineGun = new Gun("Pistol", 200, 12, 1.2f, 0.5f, 1f, null, false);
        Gun machineGun = new Gun("Pistol", 200, 12, 1.2f, 0.5f, 1f, null, false);

        Inventory[0] = pistol;
        Inventory[1] = shotgun;
        Inventory[2] = submachineGun;
        Inventory[3] = machineGun;

        // for (int i = 0; i < Inventory.Length; i++)
        // {
        //     Inventory[i].bulletsLeft = Inventory[i].magSize;
        // }
        readyToShoot = true;
    }

    void Update()
    {
        // ammoUI.SetText(equippedGun.bulletsLeft + "/" + equippedGun.ammo);

        equippedGun = Inventory[selectedGun];
        selectedGun += (int)Input.mouseScrollDelta.y;
        if(Input.GetKeyDown(KeyCode.Alpha1)) selectedGun = 0;
        if(Input.GetKeyDown(KeyCode.Alpha2)) selectedGun = 1;
        if(Input.GetKeyDown(KeyCode.Alpha3)) selectedGun = 2;
        if(Input.GetKeyDown(KeyCode.Alpha4)) selectedGun = 3;
        selectedGun = Mathf.Clamp(selectedGun, 0, Inventory.Length - 1);
        int originalSelectedGun = selectedGun;
        while (!Inventory[selectedGun].acquired)
        {
            selectedGun++;
            if (selectedGun >= Inventory.Length) selectedGun = 0;
            if (selectedGun == originalSelectedGun) break;
        }
        hand.runtimeAnimatorController = equippedGun.ac;

        flashIntensity = muzzleFlashFlash.intensity;
        lightIntensity = muzzleFlashLight.intensity;

        if (Input.GetKeyDown(KeyCode.R) && equippedGun.bulletsLeft < equippedGun.magSize && !reloading) Reload();
        if (Input.GetMouseButtonDown(0) && equippedGun.bulletsLeft > 0 && readyToShoot)
        {
            Shoot();
        }

        muzzleFlashLight.intensity = Mathf.Lerp(lightIntensity,0f, Time.deltaTime * 10f);
        muzzleFlashFlash.intensity = Mathf.Lerp(flashIntensity,0f, Time.deltaTime * 15f);

        ammoUI.text = equippedGun.bulletsLeft.ToString();

        #region  Camera Sway
        xMousePos = Input.GetAxisRaw("Mouse X");
        yMousePos = Input.GetAxisRaw("Mouse Y");
        float offsetX = xMousePos * swayAmount;
        float offsetY = yMousePos * swayAmount;
        Vector3 targetPosition = new Vector3(-offsetX, -offsetY, 0f);

        anchor.localPosition = Vector3.Lerp(anchor.localPosition, targetPosition, smooth * Time.deltaTime);
        
        // targetPosition += new Vector3(1000f,1000f, 10f);    SUPPOSED TO BE IN SHOOT METHOD
        #endregion
    }

    void Shoot()
    {
        readyToShoot = false;

        muzzleFlashAnimator.Play("flashOn");
        hand.Play("gunShoot");
        CameraShaker.Invoke();
        
        flashIntensity = 0.3f;
        lightIntensity = 300f;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            gunImpact.transform.position = hit.point;
            gunImpact.transform.forward = hit.normal;
            gunImpact.Emit(1);

            SpawnBulletHole(hit, ray);
        }

        equippedGun.bulletsLeft--;
        equippedGun.bulletsShot--;

        Invoke("ResetShot", equippedGun.shootingCooldown);

        if(equippedGun.bulletsShot > 0 && equippedGun.bulletsLeft > 0)
        Invoke("Shoot", equippedGun.shootingCooldown);
    }

    void SpawnBulletHole(RaycastHit hit, Ray ray)
    {
        float positionMultiplier = 0.5f;
        float spawnX = hit.point.x - ray.direction.x * positionMultiplier;
        float spawnY = hit.point.y - ray.direction.y * positionMultiplier;
        float spawnZ = hit.point.z - ray.direction.z * positionMultiplier;
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, spawnZ);

        GameObject spawnedObject = Instantiate(bulletHolePrefab, spawnPosition, Quaternion.identity);
        Quaternion targetRotation = Quaternion.LookRotation(ray.direction);

        spawnedObject.transform.rotation = targetRotation;
        spawnedObject.transform.SetParent(bulletHoleContainer.transform);
        spawnedObject.transform.Rotate(Vector3.forward, UnityEngine.Random.Range(0f, 360f));
        Destroy(spawnedObject, destroyDelay);
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", equippedGun.reloadTime);
    }
    
    private void ReloadFinished()
    {
        equippedGun.bulletsLeft = equippedGun.magSize;
        reloading = false;
    }

}