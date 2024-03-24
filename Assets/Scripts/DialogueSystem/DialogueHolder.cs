using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueHolder : MonoBehaviour
    {

        private void Awake()
        {
                StartCoroutine(dialogueSequence());
        }

        public void OnTriggerEnter(Collider collision) {
            if (collision.gameObject.name == "Dialogue")
            {
                Debug.Log("Working");
            }
        }

        private IEnumerator dialogueSequence()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Deactivate();
                transform.GetChild(i).gameObject.SetActive(true);
                yield return new WaitUntil(() => transform.GetChild(i).GetComponent<DialogueLine>().finished);
            }
            gameObject.SetActive(false);
        }

        private void Deactivate()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}