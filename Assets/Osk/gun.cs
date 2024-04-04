using TMPro;
using UnityEngine;

public class Gun : MonoBehaviour
{
	[Header("Sway Settings")]
	[SerializeField] private float smooth;
	[SerializeField] private float swayAmount;

	public Transform anchor;
	public Animator gunAnim;
	public Camera fpscam;
	public Enemy enemy;
	private int bulletsLeft, bulletsShot;
	public int magazineSize;
	public TextMeshProUGUI text;
	private float timeBetweenShots, timeBetweenShooting;
	// bool shooting, readyToShoot;
	bool reloading;                                                          // UNUSED BOOLS?
	private float reloadTime = 1f;

	public void Start()
	{
		enemy = GetComponent<Enemy>();                                                               // GET TAG NOT GET COMPONENT TO SELECT ALL ENEMIES.
	}
	private void Awake()
	{
		bulletsLeft = magazineSize;
	}
	void Update()
	{
		text.SetText(bulletsLeft + "/" + magazineSize);
		if (Input.GetButtonDown("Fire1") && bulletsLeft > 0)                                 // if enough bullets shoot
		{
			Shoot();                                                                                 // shoot
		}
		if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();       // checking if can reload
	}

	void Shoot()
	{
		gunAnim.Play("gunShoot");
		RaycastHit hit;
		if (Physics.Raycast(fpscam.transform.position, transform.forward, out hit))
		{
			if (hit.collider.CompareTag("Enemy"))
			{
				Enemy enemy = hit.collider.GetComponent<Enemy>();
				if (enemy != null)
				{
					enemy.TakeDamage(1);
				}
			}
		}

		bulletsShot--;
		if (bulletsShot > 0 && bulletsLeft > 0) Invoke("Shoot", timeBetweenShots);
		bulletsLeft--;
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
