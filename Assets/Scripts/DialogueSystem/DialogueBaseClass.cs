using System.Collections;
using UnityEngine;
using TMPro;
using FMODUnity;

namespace DialogueSystem
{
	public class DialogueBaseClass : MonoBehaviour
	{
		public bool finished { get; private set; }
		// private float initialDelay;

		protected IEnumerator WriteText(string input, TMP_Text textHolder, Color textColor, float delay, EventReference sound)
		{
			textHolder.ForceMeshUpdate(true);
			textHolder.color = textColor;
			// initialDelay = delay;

			for (int i = 0; i < input.Length; i++)
			{
				// if (Input.GetMouseButton(0))
				// {
				//     delay = 0;
				// }
				textHolder.text += input[i];
				if (input[i] != ' ') // && delay != 0)
				{
					RuntimeManager.PlayOneShot(sound);
				}
				yield return new WaitForSeconds(delay);
			}

			yield return new WaitUntil(() => Input.GetMouseButton(0));
			// delay = initialDelay;
			finished = true;
		}
	}
}