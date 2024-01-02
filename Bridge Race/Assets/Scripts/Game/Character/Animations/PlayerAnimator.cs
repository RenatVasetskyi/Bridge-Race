using UnityEngine;

namespace Game.Character.Animations
{
    public class PlayerAnimator
    {
        private readonly Animator _animator;
        private readonly PlayerAnimationHashHolder _playerAnimationHashHolder;

        public PlayerAnimator(Animator animator)
        {
            _animator = animator;
            _playerAnimationHashHolder = new(PlayerAnimationName.Idle, PlayerAnimationName.Walk);
        }

        public void PlayIdleAnimation()
        {
            _animator.Play(_playerAnimationHashHolder.Idle);
        }

        public void PlayWalkAnimation()
        {
            _animator.Play(_playerAnimationHashHolder.Walk);
        }

        public void SetSpeed(float speed)
        {
            _animator.speed = speed;
        }
    }
}