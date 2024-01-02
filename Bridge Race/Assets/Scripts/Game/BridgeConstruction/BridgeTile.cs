using Game.Character.Interfaces;
using UnityEngine;

namespace Game.BridgeConstruction
{
    public class BridgeTile : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
        [SerializeField] private MeshRenderer _meshRenderer;

        private bool _isUsed;
        
        public Collider Collider => _collider;
        public MeshRenderer MeshRenderer => _meshRenderer;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!_isUsed & other.gameObject.TryGetComponent(out IBridgeTileCollectable player))
            {
                _isUsed = true;
                
                player.Collect(this);
                
                _collider.enabled = false;
            }
        }
    }
}