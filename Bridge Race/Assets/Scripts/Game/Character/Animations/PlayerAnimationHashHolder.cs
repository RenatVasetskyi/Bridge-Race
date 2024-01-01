using UnityEngine;

namespace Game.Character.Animations
{
    public class PlayerAnimationHashHolder
    {
        public int Idle { get; }
        public int Walk { get; }

        public PlayerAnimationHashHolder(string idle, string walk)
        {
            Idle = Animator.StringToHash(idle);
            Walk = Animator.StringToHash(walk);
        }
    }
}