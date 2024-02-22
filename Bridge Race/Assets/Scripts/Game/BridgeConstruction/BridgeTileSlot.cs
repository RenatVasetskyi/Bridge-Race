using Game.Character.Interfaces;
using UnityEngine;

namespace Game.BridgeConstruction
{
    public class BridgeTileSlot : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        
        [SerializeField] private Bridge _bridge;

        private bool _isEmpty = true;

        private void Awake()
        {
            _meshRenderer.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out IBridgeTileCollectable player) & _isEmpty)
                SetTile(player);
        }

        private void SetTile(IBridgeTileCollectable player)
        {
            BridgeTile tile = player.ExtractTile();

            if (tile != null)
            {
                _isEmpty = false;
                    
                EnableMeshRenderer(tile.Material);

                _bridge.MoveColliderToNextTileOrDisable();
                    
                Destroy(tile.gameObject);
            }
        }

        private void EnableMeshRenderer(Material materialToSet)
        {
            _meshRenderer.material = materialToSet;

            _meshRenderer.enabled = true;
        }
    }
}