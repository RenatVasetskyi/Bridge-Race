using Game.BridgeConstruction;

namespace Game.Character.Interfaces
{
    public interface IBridgeTileCollectable
    {
        void Collect(BridgeTile tile);
        BridgeTile ExtractTile();
        bool HasMaxTiles();
    }
}