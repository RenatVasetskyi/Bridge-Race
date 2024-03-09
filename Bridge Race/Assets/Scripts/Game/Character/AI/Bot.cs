using Game.Character.AI.States;
using Game.Character.StateMachine;
using Game.Character.StateMachine.Interfaces;
using Game.Generation;
using UnityEngine;

namespace Game.Character.AI
{
    public class Bot : BaseCharacter
    {
        private readonly ICharacterStateMachine _stateMachine = new CharacterStateMachine();

        public Platform CurrentPlatform { get; private set; }

        public void Initialize(Platform startPlatform)
        {
            CurrentPlatform = startPlatform;
        }
        
        private void Awake()
        {
            StateFactory stateFactory = new StateFactory(_stateMachine, this, _rigidbody);
            
            _stateMachine.EnterState<BotCollectBridgeTilesState>();
        }

        private void Update()
        {
            _stateMachine.ActiveState.FrameUpdate();
        }

        private void FixedUpdate()
        {
            _stateMachine.ActiveState.PhysicsUpdate();
            
            Climb();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
        }
        
        private class StateFactory
        {
            private readonly ICharacterStateMachine _stateMachine;
            private readonly Bot _bot;
            private readonly Rigidbody _rigidbody;

            public StateFactory(ICharacterStateMachine stateMachine, Bot bot,
                Rigidbody rigidbody) 
            {
                _stateMachine = stateMachine;
                _bot = bot;
                _rigidbody = rigidbody;

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
                    BotCollectBridgeTilesState(_stateMachine, _bot, _rigidbody));
            }

            private void CreateDeliverTileToBridgeState()
            {
                _stateMachine.AddState<BotDeliverTilesToBridgeState>(new BotDeliverTilesToBridgeState());   
            }
        }
    }
}