using UnityEngine;
using FMODUnity;

public class oneShot : MonoBehaviour
{
	public EventReference sound;
	private BoxCollider boxCollider;

	private void Start()
	{
		boxCollider = GetComponent<BoxCollider>();
		if (!boxCollider.isTrigger) boxCollider.isTrigger = true;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			Destroy(this.gameObject);
			RuntimeManager.PlayOneShot(sound);
		}
	}
}
