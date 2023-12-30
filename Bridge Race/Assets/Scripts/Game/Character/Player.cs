using Game.Character.StateMachine;
using Game.Character.StateMachine.Interfaces;
using Game.Character.StateMachine.States;
using Game.Input.Interfaces;
using UnityEngine;

namespace Game.Character
{
    public class Player : MonoBehaviour
    {
        private readonly ICharacterStateMachine _stateMachine = new CharacterStateMachine();
        
        private IInputController _inputController;

        public void Initialize(IInputController inputController)
        {
            StateFactory stateFactory = new StateFactory(_stateMachine);

            _inputController = inputController;

            Subscribe();
            
            EnterIdleState();
        }

        private void Update()
        {
            _stateMachine.ActiveState?.FrameUpdate();
        }

        private void FixedUpdate()
        {
            _stateMachine.ActiveState?.PhysicsUpdate();
        }

        private void OnDestroy()
        {
            UnSubscribe();
        }

        private void Subscribe()
        {
            _inputController.OnInputActivated += EnterMovementState;
            _inputController.OnInputDeactivated += EnterIdleState;
        }

        private void UnSubscribe()
        {
            _inputController.OnInputActivated -= EnterMovementState;
            _inputController.OnInputDeactivated -= EnterIdleState;
        }

        private void EnterMovementState()
        {
            _stateMachine.EnterState<PlayerMovementState>();
        }

        private void EnterIdleState()
        {
            _stateMachine.EnterState<PlayerIdleState>();
        }

        private class StateFactory
        {
            private readonly ICharacterStateMachine _stateMachine;

            public StateFactory(ICharacterStateMachine stateMachine)
            {
                _stateMachine = stateMachine;
                
                CreateStates();
            }

            private void CreateStates()
            {
                CreatePlayerIdleState();
                CreatePlayerMovementState();
            }

            private void CreatePlayerIdleState()
            {
                _stateMachine.AddState<PlayerIdleState>(new PlayerIdleState());
            }

            private void CreatePlayerMovementState()
            {
                _stateMachine.AddState<PlayerMovementState>(new PlayerMovementState());
            }
        }
    }
}