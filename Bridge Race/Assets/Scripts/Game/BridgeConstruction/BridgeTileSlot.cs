using System;
using Game.Character.Interfaces;
using UnityEngine;

namespace Game.BridgeConstruction
{
    public class BridgeTileSlot : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;

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
                    
                _meshRenderer.material = tile.Material;

                _meshRenderer.enabled = true;
                    
                Destroy(tile.gameObject);
            }
        }
    }
}