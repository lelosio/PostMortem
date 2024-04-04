using UnityEngine;

namespace DialogueSystem
{
	public class PlayerBoxCollision : MonoBehaviour
	{
		[SerializeField] private GameObject dialogue;

		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Player"))
			{
				dialogue.SetActive(true);
				Destroy(this);
			}
		}
	}
}