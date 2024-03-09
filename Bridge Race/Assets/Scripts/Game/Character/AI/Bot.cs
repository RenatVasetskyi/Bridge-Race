using System.Collections.Specialized;
using Data;
using Game.Character.AI.States;
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

        private BotData _data;
        
        public Platform CurrentPlatform { get; private set; }

        [Inject]
        public void Construct(GameSettings gameSettings)
        {
            _data = gameSettings.BotData;
        }
        
        public void Initialize(Platform startPlatform)
        {
            ChangeCurrentPlatform(startPlatform);
            
            StateFactory stateFactory = new StateFactory
                (_stateMachine, this, _rigidbody, _data);
            
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

            public StateFactory(ICharacterStateMachine stateMachine, Bot bot,
                Rigidbody rigidbody, BotData botData) 
            {
                _stateMachine = stateMachine;
                _bot = bot;
                _rigidbody = rigidbody;
                _botData = botData;

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
                _stateMachine.AddState<BotIdleState>(new BotIdleState());
            }

            private void CreateCollectBridgeTilesState()
            {
                _stateMachine.AddState<BotCollectBridgeTilesState>(new 
                    BotCollectBridgeTilesState(_stateMachine, _bot, _rigidbody, _botData));
            }

            private void CreateDeliverTileToBridgeState()
            {
                _stateMachine.AddState<BotDeliverTilesToBridgeState>(new BotDeliverTilesToBridgeState());   
            }
        }
    }
}