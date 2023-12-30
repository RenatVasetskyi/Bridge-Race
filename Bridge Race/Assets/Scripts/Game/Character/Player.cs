using Data;
using Game.Character.Data;
using Game.Character.StateMachine;
using Game.Character.StateMachine.Interfaces;
using Game.Character.StateMachine.States;
using Game.Input.Interfaces;
using UnityEngine;
using Zenject;

namespace Game.Character
{
    public class Player : MonoBehaviour
    {
        private readonly ICharacterStateMachine _stateMachine = new CharacterStateMachine();

        [SerializeField] private Rigidbody _rigidbody;

        private GameSettings _gameSettings;
        
        private IInputController _inputController;

        [Inject]
        public void Construct(GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
        }
        
        public void Initialize(IInputController inputController)
        {
            _inputController = inputController;
            
            StateFactory stateFactory = new StateFactory
                (_stateMachine, _inputController, _rigidbody, _gameSettings.PlayerData);

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
            private readonly IInputController _inputController;
            private readonly Rigidbody _rigidbody;
            private readonly PlayerData _playerData;

            public StateFactory(ICharacterStateMachine stateMachine, IInputController inputController, 
                Rigidbody rigidbody, PlayerData playerData)
            {
                _stateMachine = stateMachine;
                _inputController = inputController;
                _rigidbody = rigidbody;
                _playerData = playerData;

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
                _stateMachine.AddState<PlayerMovementState>(new PlayerMovementState
                    (_inputController, _rigidbody, ref _playerData.Speed));
            }
        }
    }
}