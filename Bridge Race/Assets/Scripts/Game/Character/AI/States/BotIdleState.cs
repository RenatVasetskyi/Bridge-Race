using Game.Character.Animations;
using Game.Character.StateMachine.Interfaces;
using UnityEngine;

namespace Game.Character.AI.States
{
    public class BotIdleState : ICharacterState
    {
        private readonly PlayerAnimator _playerAnimator;
        private readonly Rigidbody _rigidbody;

        public BotIdleState(PlayerAnimator playerAnimator, Rigidbody rigidbody)
        {
            _playerAnimator = playerAnimator;
            _rigidbody = rigidbody;
        }
        
        public void Enter()
        {
            StopRigidbody();
            
            _playerAnimator.PlayIdleAnimation();
        }

        public void Exit()
        {
        }

        public void FrameUpdate()
        {
        }

        public void PhysicsUpdate()
        {
        }
        
        private void StopRigidbody()
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }
    }
}