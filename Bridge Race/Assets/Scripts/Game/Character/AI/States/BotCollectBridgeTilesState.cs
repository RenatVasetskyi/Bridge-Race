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
        private const float StopThreshold = 0.05f;
        
        private readonly ICharacterStateMachine _stateMachine;
        private readonly Bot _bot;
        private readonly Rigidbody _rigidbody;
        private readonly BotData _botData;
        private readonly PlayerAnimator _playerAnimator;

        private BridgeTile _closestTile;

        public BotCollectBridgeTilesState(ICharacterStateMachine stateMachine, Bot bot,
            Rigidbody rigidbody, BotData botData, PlayerAnimator playerAnimator)
        {
            _stateMachine = stateMachine;
            _bot = bot;
            _rigidbody = rigidbody;
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
                Vector3 direction = (_closestTile.transform.position - _bot.transform.position).normalized;
            
                _rigidbody.velocity = direction * _botData.Speed;
                
                Rotate(direction);

                if (IsBotNearClosestTile())
                    SetClosestTileOrEnterIdleState();

                if (!_bot.CurrentPlatform.Tiles.Contains(_closestTile))
                    SetClosestTileOrEnterIdleState();

                if (_bot.BridgeTiles.Count >= _botData.MaxTilesInHands)
                    _stateMachine.EnterState<BotDeliverTilesToBridgeState>();
            }
        }

        private void Rotate(Vector3 direction)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            Quaternion rotation = Quaternion.Lerp(_bot.transform.rotation,
                targetRotation, _botData.RotationSpeed * Time.deltaTime);

            _bot.transform.rotation = rotation;
        }

        private bool IsBotNearClosestTile()
        {
            return Vector3.Distance(new Vector3(_rigidbody.position.x, 0, _rigidbody.position.z),
                new Vector3(_closestTile.transform.position.x, 0, _closestTile.transform.position.z)) <= StopThreshold;
        }
    }
}