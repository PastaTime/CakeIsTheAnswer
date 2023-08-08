using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace GoodFlower
{
    public interface ISound
    {
        public string Name { get; }

        public AudioClip File { get; }
    }

    [Serializable]
    public class Sound : ISound
    {
        [SerializeField] private AudioClip file;

        public string Name => file.name;

        public AudioClip File => file;
    }

    [Serializable]
    public class SoundArray : ISound
    {
        [SerializeField] private List<AudioClip> files;

        private static Random _rnd = new();

        public string Name
        {
            get
            {
                var firstName = files[0].name;
                return firstName[..firstName.LastIndexOf('_')];
            }
        }

        public AudioClip File => files[_rnd.Next(files.Count)];
    }

    public class SoundBank : MonoBehaviour
    {
        [SerializeField] private List<Sound> sounds = new();
        [SerializeField] private List<SoundArray> soundArrays = new();

        private IEnumerable<ISound> Sounds => sounds.ToList<ISound>().Concat(soundArrays.ToList<ISound>());


        public AudioClip GetSoundByName(string soundName)
        {
            var clip = Sounds.FirstOrDefault(s => s.Name == soundName)?.File;

            if (clip is null)
                throw new FileNotFoundException($"'{soundName}' is not registered in the Sound Bank.");

            return clip;
        }
    }
}