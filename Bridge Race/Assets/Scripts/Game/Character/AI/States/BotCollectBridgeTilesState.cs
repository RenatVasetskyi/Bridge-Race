using System.Collections.Generic;
using System.Linq;
using Game.BridgeConstruction;
using Game.Character.StateMachine.Interfaces;
using UnityEngine;

namespace Game.Character.AI.States
{
    public class BotCollectBridgeTilesState : ICharacterState
    {
        private readonly ICharacterStateMachine _stateMachine;
        private readonly Bot _bot;
        private readonly Rigidbody _rigidbody;

        public BotCollectBridgeTilesState(ICharacterStateMachine stateMachine, Bot bot,
            Rigidbody rigidbody)
        {
            _stateMachine = stateMachine;
            _bot = bot;
            _rigidbody = rigidbody;
        }
        
        public void Enter()
        {
            BridgeTile closestTile = GetClosestTile();

            if (closestTile != null)
            {
                
            }
            else
            {
                _stateMachine.EnterState<BotIdleState>();
            }
        }

        public void Exit()
        {
        }

        public void FrameUpdate()
        {
        }

        public void PhysicsUpdate()
        {
        }
        
        private BridgeTile GetClosestTile()
        {
            List<BridgeTile> tilesOnPlatform = _bot.CurrentPlatform.Tiles.ToList();

            if (tilesOnPlatform.Count == 0)
                return null;
            
            BridgeTile closestTile = tilesOnPlatform.First();
            
            float closestDistance = Vector3.Distance
                (_bot.transform.position, closestTile.transform.position);
            
            foreach (BridgeTile bridgeTile in _bot.CurrentPlatform.Tiles.ToList())
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

        private void MoveToClosestTile(Transform tile)
        {
        }
    }
}