using System.Collections.Generic;
using System.Linq;
using Data;
using Game.BridgeConstruction;
using Game.Character.Animations;
using Game.Character.Data;
using Game.Character.Interfaces;
using Game.Character.StateMachine;
using Game.Character.StateMachine.Interfaces;
using Game.Character.StateMachine.States;
using Game.Input.Interfaces;
using UnityEngine;
using Zenject;

namespace Game.Character
{
    public class Player : MonoBehaviour, IBridgeTileCollectable
    {
        private readonly List<BridgeTile> _bridgeTiles = new();
        
        private readonly ICharacterStateMachine _stateMachine = new CharacterStateMachine();

        [Header("Components")]
        
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _bridgeTileHolder; 

        private GameSettings _gameSettings;
        
        private IInputController _inputController;
        
        private PlayerAnimator _playerAnimator;

        [Inject]
        public void Construct(GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
        }
        
        public void Initialize(IInputController inputController)
        {
            _inputController = inputController;
            _playerAnimator = new PlayerAnimator(_animator);
            
            StateFactory stateFactory = new StateFactory
                (_stateMachine, _inputController, _rigidbody, _gameSettings.PlayerData, _playerAnimator);

            Subscribe();
            
            EnterIdleState();
        }
        
        public void Collect(Collider tile, BridgeTile tileComponent)
        {
            if (_bridgeTiles.Count == 0)
            {
                tileComponent.transform.position = _bridgeTileHolder.position;   
            }
            else
            {
                tileComponent.transform.position = _bridgeTiles.Last().transform
                    .position + new Vector3(0, tile.bounds.extents.y * 2, 0);
            }
            
            tile.transform.SetParent(_bridgeTileHolder);
            
            tile.transform.rotation = new Quaternion(0, 0, 0, 0);

            _bridgeTiles.Add(tileComponent);
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
                    (_inputController, _rigidbody, ref _playerData.Speed, _playerAnimator));
            }
        }
    }
}