﻿using System;
using TMPro;
using UnityEngine;

namespace Narration
{
    public class Narrator : MonoBehaviour
    {
        [SerializeField] private BaseSoundManager manager;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private TextAsset storyFile;
        [SerializeField] private float narrationSpeed = 0.005f;
        [SerializeField] private KeyCode continueKey = KeyCode.Return;

        private StoryParser parser;

        private void Awake()
        {
            manager = FindObjectOfType<BaseSoundManager>();

            if (text == null)
                throw new NullReferenceException("Text Mesh is Required");

            if (storyFile == null)
                throw new NullReferenceException("Story File is required");

            parser = new StoryParser(manager, text, storyFile.text)
            {
                TypeSpeed = narrationSpeed,
            };

            StartCoroutine(parser.Reader(continueKey));
        }
    }
}