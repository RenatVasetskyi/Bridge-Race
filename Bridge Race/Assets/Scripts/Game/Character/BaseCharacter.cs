using System.Collections.Generic;
using System.Linq;
using Game.BridgeConstruction;
using Game.Character.Interfaces;
using UnityEngine;

namespace Game.Character
{
    public abstract class BaseCharacter : MonoBehaviour, IBridgeTileCollectable, IFinishDetectable
    {
        private const float FinishAnimationDuration = 1f;
        
        private const float MoveTileDuration = 0.15f;
        
        private const float StepRayDistance = 3f;
        
        protected readonly List<BridgeTile> _bridgeTiles = new();

        [Header("Components")]        
        
        [SerializeField] protected Rigidbody _rigidbody;
        [SerializeField] protected Transform _bridgeTileHolder;
        [SerializeField] private Transform _climbRaycastOrigin;

        [Header("Tile Animation")] 
        
        [SerializeField] private LeanTweenType _tileAnimationEasing;
        
        [Header("Layers")]
        
        [SerializeField] private LayerMask _stepLayer;
        [SerializeField] private LayerMask _groundLayer;
        
        public abstract bool HasMaxTiles();
        
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
        
        public BridgeTile ExtractTile()
        {
            if (_bridgeTiles.Count > 0)
            {
                BridgeTile tile = GetLastTileFromList();

                return tile;
            }

            return null;
        }
        
        public virtual void DoFinishAnimation(Transform finish)
        {
            _rigidbody.isKinematic = true;
            
            _bridgeTileHolder.gameObject.SetActive(false);

            LeanTween.scale(gameObject, Vector3.zero, FinishAnimationDuration);
            LeanTween.move(gameObject, finish.position, FinishAnimationDuration);
        }
        
        protected void Climb()
        {
            RaycastHit hit;

            if (Physics.Raycast(_climbRaycastOrigin.position, transform.TransformDirection(-Vector3.up),
                    out hit, StepRayDistance, _stepLayer | _groundLayer))
            {
                Vector3 targetVector = new Vector3(_rigidbody.position.x, hit.point.y, _rigidbody.position.z);   

                _rigidbody.position = targetVector;
                
                _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
            }
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
    }
}