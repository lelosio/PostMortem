using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

namespace DialogueSystem
{
    public class DialogueBaseClass : MonoBehaviour
    {
        public bool finished { get; private set; }

        protected IEnumerator WriteText(string input, TMP_Text textHolder, Color textColor, float delay, EventReference sound, float delayBetweenLines)
        {
            textHolder.ForceMeshUpdate(true);
            textHolder.color = textColor;

            for (int i = 0; i < input.Length; i++)
            {
                textHolder.text += input[i];
                if (input[i].ToString() != " ")
                {
                    FMODUnity.RuntimeManager.PlayOneShot(sound);
                }
                yield return new WaitForSeconds(delay);
            }

            //yield return new WaitForSeconds(delayBetweenLines);
            yield return new WaitUntil(() => Input.GetMouseButton(0));
            finished = true;
        }
    }
}