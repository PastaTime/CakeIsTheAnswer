using System;
using Narration;
using UnityEngine;

namespace GoodFlower
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(SoundBank))]
    public class SoundManager : BaseSoundManager
    {
        [SerializeField] private AudioSource source;
        [SerializeField] private SoundBank soundBank;


        private string CurrentlyPlayingClip => source.clip != null ? source.clip.name : "";

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            source = GetComponent<AudioSource>();
            soundBank = GetComponent<SoundBank>();
            source.loop = true;
        }


        public override void PlayBackgroundMusic(string songName)
        {
            if (songName == "" || CurrentlyPlayingClip.Equals(songName))
                return;

            source.clip = soundBank.GetSoundByName(songName);
            source.Play();
        }

        public override void PlaySoundEffect(string songName)
        {
            if (songName == "")
                return;

            source.PlayOneShot(soundBank.GetSoundByName(songName));
        }
    }
}