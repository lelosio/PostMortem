using UnityEditor.Animations;
using UnityEngine;
using TMPro;
using FMODUnity;
using Microsoft.Unity.VisualStudio.Editor;

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
    public Animator crosshair;
    public Animator hand;
    public Gun[] Inventory = new Gun[4];
    public Gun equippedGun;

    public Transform muzzleFlash;
    public Animator muzzleFlashAnimator;
    public Light muzzleFlashLight;
    public Light muzzleFlashFlash;
    public ParticleSystem gunImpact;

    public EventReference gunShot;
    public EventReference emptyClip;

    int selectedGun;
    private float flashIntensity;
    private float lightIntensity;

    public TextMeshProUGUI ammoUI;
    bool reloading;
    bool readyToShoot;
    public UIManager uim;

    public class Gun
    {
        public string name;
        public int ammo;
        public int magSize;
        public float spread;
        public AnimatorController ac;
        public bool acquired;
        public int bulletsLeft;
        public int pellets;
        public float shootingCooldown;
        public float reloadTime;

        public Gun(string Name, int Ammo, int MagSize, float Spread, float ReloadTime, float ShootingCooldown, AnimatorController AC, bool Acquired, int Pellets = 1)
        {
            name = Name;
            ammo = Ammo;
            magSize = MagSize;
            spread = Spread;
            reloadTime = ReloadTime;
            shootingCooldown = ShootingCooldown;

            ac = AC;
            acquired = Acquired;
        
            bulletsLeft = magSize;

            pellets = Pellets;
        }
    }
    
    void Start()
    {
        Gun pistol = new Gun("Pistol", 24, 12, 0f, 0.25f, 0.2f, pistolAnim, true);
        Gun shotgun = new Gun("Shotgun", 10, 3, 0.1f, 0.5f, 0.5f, shotgunAnim, true, 6);
        Gun submachineGun = new Gun("Pistol", 300, 12, 1.2f, 0.5f, 1f, null, false);
        Gun machineGun = new Gun("Pistol", 400, 12, 1.2f, 0.5f, 1f, null, false);

        Inventory[0] = pistol;
        Inventory[1] = shotgun;
        Inventory[2] = submachineGun;
        Inventory[3] = machineGun;

        readyToShoot = true;
    }

    void Update()
    {
        #region  Camera Sway
        xMousePos = Input.GetAxisRaw("Mouse X");
        yMousePos = Input.GetAxisRaw("Mouse Y");
        float offsetX = xMousePos * swayAmount;
        float offsetY = yMousePos * swayAmount;
        Vector3 targetPosition = new Vector3(-offsetX, -offsetY, 0f);

        anchor.localPosition = Vector3.Lerp(anchor.localPosition, targetPosition, smooth * Time.deltaTime);
        
        // targetPosition += new Vector3(1000f,1000f, 10f);    SUPPOSED TO BE IN SHOOT METHOD
        #endregion

        if (equippedGun != Inventory[selectedGun])
        {
            // set opacity value to X
        }
            // lerp opacity, value x, time
            // maybe add some invoke function if the selected gun changed to start lerping after that so there's a delay

        equippedGun = Inventory[selectedGun];
        if (Input.GetKeyDown(KeyCode.Alpha1) && Inventory[0].acquired) selectedGun = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2) && Inventory[1].acquired) selectedGun = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3) && Inventory[2].acquired) selectedGun = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4) && Inventory[3].acquired) selectedGun = 3;
        int nextSelectedGun = selectedGun + (int)Input.mouseScrollDelta.y;
        if (nextSelectedGun < 0) {
            nextSelectedGun = Inventory.Length - 1;
        } else if (nextSelectedGun >= Inventory.Length) {
            nextSelectedGun = 0;
        }

        while (!Inventory[nextSelectedGun].acquired) {
            nextSelectedGun += (int)Input.mouseScrollDelta.y;
            if (nextSelectedGun < 0) {
                nextSelectedGun = Inventory.Length - 1;
            } else if (nextSelectedGun >= Inventory.Length) {
                nextSelectedGun = 0;
            }
        }
    
        for(int i = 0; i < selectedGun; i++ )
        {
            if (Inventory[i].acquired)
            {
                uim.slot[i].sprite = uim.slotSprite[1];
            }
            else
            {
                uim.slot[i].sprite = uim.slotSprite[0];
            }
        }

        selectedGun = nextSelectedGun;


        hand.runtimeAnimatorController = equippedGun.ac;

        flashIntensity = muzzleFlashFlash.intensity;
        lightIntensity = muzzleFlashLight.intensity;

        if (Input.GetKeyDown(KeyCode.R) && equippedGun.bulletsLeft < equippedGun.magSize && !reloading) Reload();
        if (Input.GetMouseButtonDown(0) && readyToShoot && !reloading)
        {
            if (equippedGun.bulletsLeft > 0)
            {
                Shoot();
            }
            else
            {
                readyToShoot = false;
                RuntimeManager.PlayOneShot(emptyClip);
                Invoke("ResetShot", equippedGun.shootingCooldown);
            }
        }

        muzzleFlashLight.intensity = Mathf.Lerp(lightIntensity,0f, Time.deltaTime * 10f);
        muzzleFlashFlash.intensity = Mathf.Lerp(flashIntensity,0f, Time.deltaTime * 15f);

        uim.ammo.text = $"{equippedGun.bulletsLeft}<color=#d9d9d9><size=60%>/{equippedGun.ammo}<br>{equippedGun.name}";
    }

    void Shoot()
    {
        readyToShoot = false;

        muzzleFlashAnimator.Play("flashOn");
        hand.Play("gunShoot");
        crosshair.Play("crosshairShot");
        CameraShaker.Invoke();
        
        flashIntensity = 0.3f;
        lightIntensity = 300f;

        RuntimeManager.PlayOneShot(gunShot);
        
        for (int i = 0; i < equippedGun.pellets; i++)
        {
            float y = Random.Range(-equippedGun.spread, equippedGun.spread);
            float x = Random.Range(-equippedGun.spread, equippedGun.spread);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction + new Vector3(x, y, 0), out hit))
            {
                gunImpact.transform.position = hit.point;
                gunImpact.transform.forward = hit.normal;
                gunImpact.Emit(1);

                SpawnBulletHole(hit, ray);
            }
        }

        equippedGun.bulletsLeft--;

        Invoke("ResetShot", equippedGun.shootingCooldown);
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
        spawnedObject.transform.Rotate(Vector3.forward, Random.Range(0f, 360f));
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
        if (Inventory[selectedGun].ammo >= Inventory[selectedGun].magSize - Inventory[selectedGun].bulletsLeft)
        {
            Inventory[selectedGun].ammo -= Inventory[selectedGun].magSize - Inventory[selectedGun].bulletsLeft;
            Inventory[selectedGun].bulletsLeft = Inventory[selectedGun].magSize;
        }
        else
        {
            Inventory[selectedGun].bulletsLeft += Inventory[selectedGun].ammo;
            Inventory[selectedGun].ammo = 0;
        }
        reloading = false;
    }

}