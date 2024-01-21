using Game.Character.Interfaces;
using UnityEngine;

namespace Game.BridgeConstruction
{
    public class BridgeTile : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
        [SerializeField] private MeshRenderer _meshRenderer;

        private bool _isUsed;
        
        public Vector3 Position { get; set; }
        public MeshRenderer MeshRenderer => _meshRenderer;
        public Material Material => _meshRenderer.material;

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