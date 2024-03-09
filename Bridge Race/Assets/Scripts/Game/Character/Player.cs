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
    public class Player : BaseCharacter, IBridgeTileCollectable, IFinishDetectable
    {
        private const int BridgeTileLimit = 30;
        
        private const int FinishAnimationDuration = 1;
        
        private const float MoveTileDuration = 0.15f;
 
        private readonly List<BridgeTile> _bridgeTiles = new();
        
        private readonly ICharacterStateMachine _stateMachine = new CharacterStateMachine();

        [Header("Components")]
        
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
            if (HasMaxTiles())
                return;
            
            tile.transform.SetParent(_bridgeTileHolder);

            ResetRotation(tile.transform);
            
            tile.Position = GetNextTilePosition(tile);
            
            _bridgeTiles.Add(tile);
            
            AnimateTile(tile);
        }

        private bool HasMaxTiles()
        {
            return _bridgeTiles.Count >= BridgeTileLimit;
        }

        public BridgeTile ExtractTile()
        {
            if (_bridgeTiles.Count > 0)
            {
                BridgeTile tile = GetLastTileFromList();

                return tile;
            }

            return null;
        }

        public void DoFinishAnimation(Transform finish)
        {
            _rigidbody.isKinematic = true;
            
            _bridgeTileHolder.gameObject.SetActive(false);

            _inputController.Disable();

            LeanTween.scale(gameObject, Vector3.zero, FinishAnimationDuration);
            LeanTween.move(gameObject, finish.position, FinishAnimationDuration);
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
        
        private BridgeTile GetLastTileFromList()
        {
            BridgeTile tile = _bridgeTiles.Last();

            _bridgeTiles.Remove(tile);
            
            return tile;
        }
        
        private Vector3 GetNextTilePosition(BridgeTile tile)
        {
            Vector3 tilePosition;

            if (_bridgeTiles.Count == 0)
            {
                tilePosition = _bridgeTileHolder.localPosition; 
            }
            else
            {
                tilePosition = _bridgeTiles.Last().Position + new Vector3
                    (0, tile.MeshRenderer.bounds.extents.y / 2, 0);
            }
            
            return tilePosition;
        }
        
        private void AnimateTile(BridgeTile tile)
        {
            LeanTween.moveLocal(tile.gameObject, new Vector3(tile.MeshRenderer.bounds.extents.x,
                    tile.Position.y, 0), MoveTileDuration).setEase(_tileAnimationEasing)
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
                    (_inputController, _rigidbody, _playerAnimator, ref _playerData.Speed));
            }
        }
    }
}