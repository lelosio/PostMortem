using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FMODUnity;

namespace DialogueSystem
{
    public class DialogueLine : DialogueBaseClass
    {
        private TextMeshProUGUI textHolder;

        [Header ("Text Options")]
        [SerializeField] private string input;
        [SerializeField] private Color textColor;

        [Header("Time parameters")]
        [SerializeField] private float delay;

        [Header("Sound")]
        [SerializeField] private EventReference sound;

        [Header("Character Image")]
        [SerializeField] private Sprite characterSprite;
        [SerializeField] private Image imageHolder;

        private void Awake()
        {
            textHolder = GetComponent<TextMeshProUGUI>();
            textHolder.text = "";

            imageHolder.sprite = characterSprite;
            imageHolder.preserveAspect = true;
        }

        private void Start()
        {
            StartCoroutine(WriteText(input, textHolder, textColor, delay, sound));
        }
    }
}