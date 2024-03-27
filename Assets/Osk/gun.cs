using TMPro;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Sway Settings")]
    [SerializeField] private float smooth;
    [SerializeField] private float swayAmount;

    public Transform anchor;
    private float xMousePos;
    private float yMousePos;
    public Animator gunAnim;
    public Camera fpscam;
    public Enemy enemy;
    private int bulletsLeft, bulletsShot;
    public int magazineSize, bulletsPerTap;
    public TextMeshProUGUI text;
    private float timeBetweenShots, timeBetweenShooting;
    bool shooting, readyToShoot, reloading;
    private float reloadTime = 1f;

    public void Start()
    {
        enemy = GetComponent<Enemy>();
    }
    private void Awake()
    {
        bulletsLeft = magazineSize;
    }
    void Update()
    {
        text.SetText(bulletsLeft + "/" + magazineSize);
        HandleSway();
        HandleInput();
    }

    void HandleSway()
    {
        xMousePos = Input.GetAxisRaw("Mouse X");
        yMousePos = Input.GetAxisRaw("Mouse Y");
        float offsetX = xMousePos * swayAmount;
        float offsetY = yMousePos * swayAmount;
        Vector3 targetPosition = new Vector3(-offsetX, -offsetY, 0f);

        anchor.localPosition = Vector3.Lerp(anchor.localPosition, targetPosition, smooth * Time.deltaTime);
    }

    void HandleInput()
    {
        if (Input.GetButtonDown("Fire1") && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();
    }

    void Shoot()
    {
        gunAnim.Play("gunShoot");

        RaycastHit hit;
        
        if (Physics.Raycast(fpscam.transform.position, transform.forward, out hit))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                // Assuming enemy has health script attached
                 Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(1); // Adjust damage as needed
                }
            }
            Debug.Log(hit.ToString());
        }
        else
        {
            Debug.Log("nothing Detected");
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
    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

}
