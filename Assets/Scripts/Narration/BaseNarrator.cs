using System;
using TMPro;
using UnityEngine;

namespace Narration
{
    public abstract class BaseNarrator : MonoBehaviour
    {
        [SerializeField] protected BaseSoundManager soundManager;
        [SerializeField] protected TextMeshProUGUI text;
        [SerializeField] protected float narrationSpeed = 0.005f;
        [SerializeField] protected float narrationAudioSpeed = 0.01f;
        [SerializeField] protected string newLineSound = "typewriter_ding";
        [SerializeField] private KeyCode continueKey = KeyCode.Return;

        private StoryParser parser;

        protected virtual void Awake()
        {
            soundManager = FindObjectOfType<BaseSoundManager>();

            if (text == null)
                throw new NullReferenceException("Text Mesh is Required");

            parser = new StoryParser(soundManager, text, GetStory())
            {
                TypeSpeed = narrationSpeed,
                TypeAudioSpeed = narrationAudioSpeed,
                CurrentNewlineSound = newLineSound,
                CurrentCharacter = ""
            };
            StartCoroutine(parser.Reader(continueKey));
        }

        protected abstract string GetStory();
    }
}