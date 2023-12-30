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

        public PlayerMovementState(IInputController inputController, Rigidbody rigidbody, 
            ref float speed)
        {
            _inputController = inputController;
            _rigidbody = rigidbody;
            _speed = speed;
        }
        
        public void Enter()
        {
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