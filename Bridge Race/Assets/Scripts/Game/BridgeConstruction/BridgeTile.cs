using Game.Character.Interfaces;
using UnityEngine;

namespace Game.BridgeConstruction
{
    public class BridgeTile : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out IBridgeTileCollectable player))
                player.Collect(_collider, this);
        }
    }
}