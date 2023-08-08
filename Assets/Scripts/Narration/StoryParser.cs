using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

namespace Narration
{
    public class StoryParser
    {
        private readonly BaseSoundManager manager;
        private readonly TextMeshProUGUI textMesh;
        private readonly string story;

        private int startIndex = 0;
        private int endIndex = 0;

        private const string CommandRegex = "{.*}";
        private const string PressToContinue = "\n\n<b>Press {0} to Continue</b>";


        private const string PlayCommand = "PLAY";
        private const string CharacterCommand = "CHARACTER";
        private const string EffectCommand = "EFFECT";

        public StoryParser(BaseSoundManager manager, TextMeshProUGUI textMesh, string story)
        {
            this.manager = manager;
            this.textMesh = textMesh;
            this.story = story;
            startIndex = 0;
            endIndex = 0;
        }

        public string CurrentCharacter { get; set; } = "";
        public string CurrentNewlineSound { get; set; } = "";

        public float TypeSpeed { get; set; } = 0.01f;


        public IEnumerator Reader(KeyCode continueKey)
        {
            var keyName = continueKey.ToString();
            while (endIndex < story.Length)
            {
                textMesh.text = ReadNext();
                manager.PlaySoundEffect(CurrentCharacter);

                if (textMesh.isTextOverflowing && story[endIndex] == '\n')
                {
                    textMesh.text += string.Format(PressToContinue, keyName);
                    yield return WaitForKeyPress(continueKey);
                    manager.PlaySoundEffect(CurrentNewlineSound);
                    startIndex = endIndex;
                }
                yield return new WaitForSeconds(TypeSpeed);
            }
        }

        private string ReadNext()
        {
            switch (story[endIndex++])
            {
                case '<':
                    ReadUntil('>');
                    break;
                case '{':
                    ProcessNarrationCommand();
                    break;
            }

            return Regex.Replace(story[startIndex..endIndex], CommandRegex, "");
        }

        private void ReadUntil(char character)
        {
            while (endIndex < story.Length && story[endIndex - 1] != character)
                endIndex++;
        }

        private void ProcessNarrationCommand()
        {
            var start = endIndex;
            ReadUntil('}');

            var command = story[start..(endIndex - 1)];
            var parameters = command.Split(':');
            var directive = parameters[0].ToUpper();
            var argument = parameters[1].Trim();
            switch (directive)
            {
                case PlayCommand:
                    manager.PlayBackgroundMusic(argument);
                    break;

                case CharacterCommand:
                    CurrentCharacter = argument;
                    break;

                case EffectCommand:
                    manager.PlaySoundEffect(argument);
                    break;
            }
        }

        private IEnumerator WaitForKeyPress(KeyCode key)
        {
            while (!Input.GetKeyDown(key))
            {
                yield return null;
            }
        }
    }
}