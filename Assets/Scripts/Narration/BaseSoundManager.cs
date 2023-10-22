using UnityEngine;

namespace Narration
{
    public abstract class BaseSoundManager : MonoBehaviour
    {
        public abstract void PlayBackgroundMusic(string songName);

        public abstract void PlaySoundEffect(string songName);
    }
}