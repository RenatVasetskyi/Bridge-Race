using Game.BridgeConstruction;
using UnityEngine;

namespace Game.Character.Interfaces
{
    public interface IBridgeTileCollectable
    {
        void Collect(Collider tile, BridgeTile tileComponent);
    }
}