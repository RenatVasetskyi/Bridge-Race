using Game.Character.Animations;
using Game.Character.StateMachine.Interfaces;
using Game.Input.Interfaces;
using UnityEngine;

namespace Game.Character.StateMachine.States
{
    public class PlayerMovementState : ICharacterState
    {
        private const int RotationSpeed = 400;
        
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
            Rotate();
        }

        public void PhysicsUpdate()
        {
            Move();
        }

        private void Move()
        { 
            Vector3 direction = new Vector3(_inputController.CurrentDirection.x, 0f, _inputController.CurrentDirection.y);
            
            _rigidbody.velocity = direction * _speed;
        }

        private void Rotate()
        {
            float angle = Mathf.Atan2(_inputController.CurrentDirection.x, _inputController.CurrentDirection.y) * Mathf.Rad2Deg;
            
            Quaternion newRotation = Quaternion.Euler(0f, angle, 0f);
            
            _rigidbody.transform.rotation = Quaternion.RotateTowards(_rigidbody.transform.rotation,
                newRotation, RotationSpeed * Time.deltaTime);
        }
    }
}