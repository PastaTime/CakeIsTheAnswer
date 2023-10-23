using Narration;
using UnityEngine;

namespace GoodFlower
{
    public class FileNarrator : BaseNarrator
    {
        [SerializeField] private TextAsset file;
        
        protected override string GetStory()
        {
            return file.text;
        }
    }
}