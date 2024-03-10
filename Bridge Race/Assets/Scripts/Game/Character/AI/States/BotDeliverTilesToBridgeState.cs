using Game.BridgeConstruction;
using Game.Character.Animations;
using Game.Character.StateMachine.Interfaces;
using UnityEngine;

namespace Game.Character.AI.States
{
    public class BotDeliverTilesToBridgeState : ICharacterState
    {
        private readonly Bot _bot;
        private readonly PlayerAnimator _playerAnimator;
        private readonly ICharacterStateMachine _stateMachine;

        private Bridge _bridgeToMove;

        private bool _isReachedStartBridgePosition;

        public BotDeliverTilesToBridgeState(Bot bot, PlayerAnimator playerAnimator,
            ICharacterStateMachine stateMachine)
        {
            _bot = bot;
            _playerAnimator = playerAnimator;
            _stateMachine = stateMachine;
        }
        
        public void Enter()
        {
            _isReachedStartBridgePosition = false;
            
            _playerAnimator.PlayWalkAnimation(); 
            
            SetRandomBridge();
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

        private void SetRandomBridge()
        {
            _bridgeToMove = _bot.CurrentPlatform.Bridges[Random.Range(0, _bot.CurrentPlatform.Bridges.Length)];
        }

        private void Move()
        {
            if (_bridgeToMove != null)
            {
                // if (!_bridgeToMove.IsStopColliderEnabled)
                // {
                    // _stateMachine.EnterState<BotCollectBridgeTilesState>();
                // }
                
                if (_bot.BridgeTiles.Count <= 0 & _bot.CurrentPlatform.Tiles.Count > 0)
                {
                    _bot.Move(_bridgeToMove.Start.position);

                    if (_bot.IsReachedPosition(_bridgeToMove.Start.position))
                        _stateMachine.EnterState<BotCollectBridgeTilesState>();
                    
                    return;
                }
                
                if(_bot.BridgeTiles.Count <= 0 & _bot.CurrentPlatform.Tiles.Count <= 0)
                {
                    _stateMachine.EnterState<BotIdleState>();
                }

                if (_bot.IsReachedPosition(_bridgeToMove.Start.position) & !_isReachedStartBridgePosition)
                {
                    _isReachedStartBridgePosition = true;
                }
                else if (_isReachedStartBridgePosition & !_bot.IsReachedPosition(_bridgeToMove.End.position))
                {
                    _bot.Move(_bridgeToMove.End.position);
                }
                else
                {
                    _bot.Move(_bridgeToMove.Start.position);
                }
            }
        }
    }
}