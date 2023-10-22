using Narration;
using UnityEngine;

namespace GoodFlower
{
    public class DayNarrator : BaseNarrator
    {
        [SerializeField] private Manager manager;

        protected override void Awake()
        {
            manager = FindObjectOfType<Manager>();
            manager.TransitionState();
            base.Awake();
        }
        protected override string GetStory()
        {
            return manager.GetNarration();
        }
    }
}