using Game.Character.Animations;
using Game.Character.StateMachine.Interfaces;
using Game.Input.Interfaces;
using UnityEngine;

namespace Game.Character.StateMachine.States
{
    public class PlayerMovementState : ICharacterState
    {
        private const float AnimationSpeedMultiplayer = 0.13f;
        
        private const int RotationSpeed = 800;
        
        private readonly IInputController _inputController;
        private readonly Rigidbody _rigidbody;
        private readonly PlayerAnimator _playerAnimator;
        private readonly float _speed;

        public PlayerMovementState(IInputController inputController, Rigidbody rigidbody, 
            PlayerAnimator playerAnimator, ref float speed)
        {
            _inputController = inputController;
            _rigidbody = rigidbody;
            _playerAnimator = playerAnimator;
            _speed = speed;
        }
        
        public void Enter()
        {
            _playerAnimator.SetSpeed(_speed * AnimationSpeedMultiplayer);
            
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
            Vector3 direction = new Vector3(_inputController.CurrentDirection.x,
                0f, _inputController.CurrentDirection.y).normalized;
            
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