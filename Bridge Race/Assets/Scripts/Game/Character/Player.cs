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
        private const int BridgeTileLimit = 30;
        
        private const float MoveTileDuration = 0.15f;

        private readonly List<BridgeTile> _bridgeTiles = new();
        
        private readonly ICharacterStateMachine _stateMachine = new CharacterStateMachine();

        [Header("Components")]
        
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _bridgeTileHolder;

        [Header("Tile Animation")] 
        
        [SerializeField] private LeanTweenType _tileAnimationEasing;

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
        
        public void Collect(BridgeTile tile)
        {
            if (_bridgeTiles.Count >= BridgeTileLimit)
                return;
            
            tile.transform.SetParent(_bridgeTileHolder);

            ResetRotation(tile.transform);
            
            tile.Position = GetNextTilePosition(tile);
            
            Vector3 localTilePosition = _bridgeTileHolder.InverseTransformPoint(tile.Position);
            
            _bridgeTiles.Add(tile);
            
            AnimateTile(tile, localTilePosition);
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
        
        private Vector3 GetNextTilePosition(BridgeTile tile)
        {
            Vector3 tilePosition;

            if (_bridgeTiles.Count == 0)
            {
                tilePosition = _bridgeTileHolder.position; 
                
                Debug.Log(tilePosition);
            }
            else
            {
                tilePosition = _bridgeTiles.Last().Position + new Vector3
                    (0, tile.MeshRenderer.bounds.extents.y * 2, 0);
            }
            
            return tilePosition;
        }
        
        private void AnimateTile(BridgeTile tile, Vector3 localTilePosition)
        {
            LeanTween.moveLocal(tile.gameObject, new Vector3(tile.MeshRenderer.bounds.extents.x,
                    localTilePosition.y, 0), MoveTileDuration).setEase(_tileAnimationEasing)
                .setOnComplete(() => LeanTween.moveLocalX(tile.gameObject, 0, MoveTileDuration)
                    .setEase(_tileAnimationEasing));
        }
        
        private void ResetRotation(Transform transform)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
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