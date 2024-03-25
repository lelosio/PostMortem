using System.Collections;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueHolder : MonoBehaviour
    {
        public GameObject UI;
        public GameObject hands;
        public PlayerMovement movement;
        public Camera playerCam;
        public Vector3 cutscenePos;
        public Vector3 cutsceneRot;

        private void OnEnable()
        {
            LockPlayer();
            StartCoroutine(DialogueSequence());
        }

        private void LockPlayer()
        {
            hands.SetActive(false);
            UI.SetActive(false);
            movement.camAnim.SetBool("isWalking", false);
            movement.enabled = false;
            playerCam.transform.position = cutscenePos;
            playerCam.transform.rotation = Quaternion.Euler(cutsceneRot);
        }

        private void UnlockPlayer()
        {
            hands.SetActive(true);
            UI.SetActive(true);
            movement.enabled = true;
        }

        private IEnumerator DialogueSequence()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Deactivate();
                transform.GetChild(i).gameObject.SetActive(true);
                yield return new WaitUntil(() => transform.GetChild(i).GetComponent<DialogueLine>().finished);
            }

            gameObject.SetActive(false);
            UnlockPlayer();
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