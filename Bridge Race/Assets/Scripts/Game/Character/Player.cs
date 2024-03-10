using Data;
using Game.Character.Animations;
using Game.Character.Data;
using Game.Character.StateMachine;
using Game.Character.StateMachine.Interfaces;
using Game.Character.StateMachine.States;
using Game.Input.Interfaces;
using UnityEngine;
using Zenject;

namespace Game.Character
{
    public class Player : BaseCharacter
    {
        private readonly ICharacterStateMachine _stateMachine = new CharacterStateMachine();

        [Header("Components")]
        
        [SerializeField] private Animator _animator;
        
        private IInputController _inputController;
        
        private PlayerAnimator _playerAnimator;
        
        private PlayerData _data;

        [Inject]
        public void Construct(GameSettings gameSettings)
        {
            _data = gameSettings.PlayerData;
        }
        
        public void Initialize(IInputController inputController)
        {
            _inputController = inputController;
            _playerAnimator = new PlayerAnimator(_animator);
            
            StateFactory stateFactory = new StateFactory
                (_stateMachine, _inputController, _rigidbody, _data, _playerAnimator);

            Subscribe();
            
            EnterIdleState();
        }

        public override void DoFinishAnimation(Transform finish)
        {
            base.DoFinishAnimation(finish);
            
            _inputController.Disable();
        }
        
        public override bool HasMaxTiles()
        {
            return _bridgeTiles.Count >= _data.MaxTilesInHands;
        }

        private void Update()
        {
            _stateMachine.ActiveState?.FrameUpdate();
        }

        private void FixedUpdate()
        {
            _stateMachine.ActiveState?.PhysicsUpdate();

            Climb();
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
            private readonly PlayerAnimator _playerAnimator;

            public StateFactory(ICharacterStateMachine stateMachine, IInputController inputController, 
                Rigidbody rigidbody, PlayerData playerData, PlayerAnimator playerAnimator)
            {
                _stateMachine = stateMachine;
                _inputController = inputController;
                _rigidbody = rigidbody;
                _playerData = playerData;
                _playerAnimator = playerAnimator;

                CreateStates();
            }

            private void CreateStates()
            {
                CreatePlayerIdleState();
                CreatePlayerMovementState();
            }

            private void CreatePlayerIdleState()
            {
                _stateMachine.AddState<PlayerIdleState>(new PlayerIdleState(_playerAnimator, _rigidbody));
            }

            private void CreatePlayerMovementState()
            {
                _stateMachine.AddState<PlayerMovementState>(new PlayerMovementState
                    (_inputController, _rigidbody, _playerAnimator, ref _playerData.Speed));
            }
        }
    }
}