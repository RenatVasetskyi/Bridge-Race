using Game.Character.Interfaces;
using Game.Generation.Interfaces;
using UnityEngine;

namespace Game.BridgeConstruction
{
    public class BridgeTile : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
        [SerializeField] private MeshRenderer _meshRenderer;

        private ITileGenerator _tileGenerator;
        
        private bool _isUsed;
        
        public Vector3 Position { get; set; }
        public MeshRenderer MeshRenderer => _meshRenderer;
        public Material Material => _meshRenderer.material;
        public Vector3 Size => _collider.bounds.extents * 2;

        public void Initialize(ITileGenerator tileGenerator)
        {
            _tileGenerator = tileGenerator;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!_isUsed & other.gameObject.TryGetComponent(out IBridgeTileCollectable player))
            {
                _isUsed = true;
                
                player.Collect(this);
                
                _collider.enabled = false;

                _tileGenerator.RemoveTile(this);
            }
        }
    }
}