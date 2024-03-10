using System.Collections.Generic;
using System.Collections.Specialized;
using Data;
using Game.BridgeConstruction;
using Game.Character.AI.States;
using Game.Character.Animations;
using Game.Character.StateMachine;
using Game.Character.StateMachine.Interfaces;
using Game.Generation;
using UnityEngine;
using Zenject;

namespace Game.Character.AI
{
    public class Bot : BaseCharacter
    {
        private readonly ICharacterStateMachine _stateMachine = new CharacterStateMachine();

        [SerializeField] private Animator _animator;
        
        private BotData _data;
        private PlayerAnimator _playerAnimator;
        
        public Platform CurrentPlatform { get; private set; }
        public List<BridgeTile> BridgeTiles => _bridgeTiles; 

        [Inject]
        public void Construct(GameSettings gameSettings)
        {
            _data = gameSettings.BotData;
        }
        
        public void Initialize(Platform startPlatform)
        {
            ChangeCurrentPlatform(startPlatform);

            _playerAnimator = new PlayerAnimator(_animator);
            
            StateFactory stateFactory = new StateFactory
                (_stateMachine, this, _rigidbody, _data, _playerAnimator);
            
            _stateMachine.EnterState<BotCollectBridgeTilesState>();
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
            if (CurrentPlatform != null)
                CurrentPlatform.Tiles.CollectionChanged -= IfCurrentStateIsIdleTryToEnterCollectBridgeTileState;
        }
        
        protected override bool HasMaxTiles()
        {
            return _bridgeTiles.Count >= _data.MaxTilesInHands;
        }

        private void ChangeCurrentPlatform(Platform platform)
        {
            if (CurrentPlatform != null)
            {
                CurrentPlatform.Tiles.CollectionChanged -= IfCurrentStateIsIdleTryToEnterCollectBridgeTileState;
            }
            
            CurrentPlatform = platform;

            CurrentPlatform.Tiles.CollectionChanged += IfCurrentStateIsIdleTryToEnterCollectBridgeTileState;
        }

        private void IfCurrentStateIsIdleTryToEnterCollectBridgeTileState
            (object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_stateMachine.CompareStateWithActive<BotIdleState>())
            {
                if (CurrentPlatform.Tiles.Count > 0)
                {
                    _stateMachine.EnterState<BotCollectBridgeTilesState>();
                }
            }
        }

        private class StateFactory
        {
            private readonly ICharacterStateMachine _stateMachine;
            private readonly Bot _bot;
            private readonly Rigidbody _rigidbody;
            private readonly BotData _botData;
            private readonly PlayerAnimator _animator;

            public StateFactory(ICharacterStateMachine stateMachine, Bot bot,
                Rigidbody rigidbody, BotData botData, PlayerAnimator animator) 
            {
                _stateMachine = stateMachine;
                _bot = bot;
                _rigidbody = rigidbody;
                _botData = botData;
                _animator = animator;

                CreateStates();
            }

            private void CreateStates()
            {
                CreateIdleState();
                CreateCollectBridgeTilesState();
                CreateDeliverTileToBridgeState();
            }

            private void CreateIdleState()
            {
                _stateMachine.AddState<BotIdleState>
                    (new BotIdleState(_animator, _rigidbody));
            }

            private void CreateCollectBridgeTilesState()
            {
                _stateMachine.AddState<BotCollectBridgeTilesState>(new 
                    BotCollectBridgeTilesState(_stateMachine, _bot, _rigidbody, _botData, _animator));
            }

            private void CreateDeliverTileToBridgeState()
            {
                _stateMachine.AddState<BotDeliverTilesToBridgeState>
                    (new BotDeliverTilesToBridgeState(_animator));   
            }
        }
    }
}