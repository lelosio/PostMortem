using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
	//reference
	public Gun gun;
	public Transform player;
	public GameObject uzi;
	public GameObject pistol;



	private void Start()
	{
		gun = GetComponent<Gun>();
	}

	private void Update()
	{
		gun.enabled = true;
		Vector3 distanceToPlayer = player.position - transform.position;
		if (Input.GetKeyDown(KeyCode.E))
		{
			pistol.SetActive(false);
			uzi.SetActive(true);

		}


		if (Input.GetKeyDown(KeyCode.Q))
		{
			pistol.SetActive(true);
			uzi.SetActive(false);
		}


	}
}

//private void PickUp()
//{
//    spriteTurn.enabled = false;
//    DroppedWeaponsHolder.SetParent(player);
//    canvas.SetActive(true);
//    equipped = true;
//    slotFull = true;

//    transform.SetParent(gunHolder);
//    transform.localPosition = Vector3.zero;
//    transform.localRotation = Quaternion.Euler(Vector3.zero);
//    transform.localScale = Vector3.one;

//    rb.isKinematic = true;
//    coll.isTrigger = true;
//    gun.enabled = true;

//}

//private void Drop()
//{
//    canvas.SetActive(false);
//    equipped = false;
//    slotFull = false;  

//    Vector3 dropPosition = DroppedWeaponsHolder.transform.position;
//    dropPosition += Vector3.up * 0.5f; 
//    transform.position = dropPosition;
//    rb.isKinematic = false;
//    coll.isTrigger = false;
//    rb.velocity = player.GetComponent<Rigidbody2D>().velocity;
//    gun.enabled = false;
//}
