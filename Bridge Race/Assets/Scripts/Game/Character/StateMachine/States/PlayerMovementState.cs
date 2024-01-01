using Game.Character.Animations;
using Game.Character.StateMachine.Interfaces;
using Game.Input.Interfaces;
using UnityEngine;

namespace Game.Character.StateMachine.States
{
    public class PlayerMovementState : ICharacterState
    {
        private readonly IInputController _inputController;
        private readonly Rigidbody _rigidbody;
        private readonly float _speed;
        private readonly PlayerAnimator _playerAnimator;

        public PlayerMovementState(IInputController inputController, Rigidbody rigidbody, 
            ref float speed, PlayerAnimator playerAnimator)
        {
            _inputController = inputController;
            _rigidbody = rigidbody;
            _speed = speed;
            _playerAnimator = playerAnimator;
        }
        
        public void Enter()
        {
            _playerAnimator.PlayWalkAnimation();
        }

        public void Exit()
        {
        }

        public void FrameUpdate()
        {
        }

        public void PhysicsUpdate()
        {
            Move();
        }

        private void Move()
        { 
            _rigidbody.velocity = new Vector3(_inputController.CurrentDirection.x, 
                _rigidbody.velocity.y, _inputController.CurrentDirection.y) * _speed;   
        }
    }
}