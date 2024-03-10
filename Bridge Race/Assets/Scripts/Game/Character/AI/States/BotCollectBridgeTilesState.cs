using System.Collections.Generic;
using System.Linq;
using Game.BridgeConstruction;
using Game.Character.Animations;
using Game.Character.StateMachine.Interfaces;
using UnityEngine;

namespace Game.Character.AI.States
{
    public class BotCollectBridgeTilesState : ICharacterState
    {
        private readonly ICharacterStateMachine _stateMachine;
        private readonly Bot _bot;
        private readonly BotData _botData;
        private readonly PlayerAnimator _playerAnimator;

        private BridgeTile _closestTile;

        public BotCollectBridgeTilesState(ICharacterStateMachine stateMachine, Bot bot, 
            BotData botData, PlayerAnimator playerAnimator)
        {
            _stateMachine = stateMachine;
            _bot = bot;
            _botData = botData;
            _playerAnimator = playerAnimator;
        }
        
        public void Enter()
        {
            SetClosestTileOrEnterIdleState();
            
            _playerAnimator.PlayWalkAnimation();   
        }

        public void Exit()
        {
        }

        public void FrameUpdate()
        {
        }

        public void PhysicsUpdate()
        {
            MoveToClosestTile();
        }
        
        private void SetClosestTileOrEnterIdleState()
        {
            _closestTile = GetClosestTile();

            if (_closestTile == null)
                _stateMachine.EnterState<BotIdleState>();
        }
        
        private BridgeTile GetClosestTile()
        {
            List<BridgeTile> tilesOnPlatform = _bot.CurrentPlatform.Tiles.ToList();

            if (tilesOnPlatform.Count == 0)
                return null;
            
            BridgeTile closestTile = tilesOnPlatform.First();
            
            float closestDistance = Vector3.Distance
                (_bot.transform.position, closestTile.transform.position);
            
            foreach (BridgeTile bridgeTile in tilesOnPlatform)
            {
                float distanceToCurrentTile = Vector3.Distance(_bot.transform.position, bridgeTile.transform.position);

                if (distanceToCurrentTile < closestDistance)
                {
                    closestDistance = distanceToCurrentTile;

                    closestTile = bridgeTile;
                }
            }

            return closestTile;
        }

        private void MoveToClosestTile()
        {
            if (_closestTile != null)
            {
                _bot.Move(_closestTile.transform.position);

                if (_bot.IsReachedPosition(_closestTile.transform.position))
                    SetClosestTileOrEnterIdleState();

                if (!_bot.CurrentPlatform.Tiles.Contains(_closestTile))
                    SetClosestTileOrEnterIdleState();

                if (_bot.BridgeTiles.Count >= _botData.MaxTilesInHands)
                    _stateMachine.EnterState<BotDeliverTilesToBridgeState>();
            }
        }
    }
}