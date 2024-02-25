using System;
using Game.Levels;
using UnityEngine.AddressableAssets;

namespace Data
{
    [Serializable]
    public class AddressablePrefabs
    {
        public AssetReferenceGameObject MainMenu;
        public AssetReferenceGameObject GameView;
        public AssetReferenceGameObject BaseParent;
        public AssetReferenceGameObject Player;
        public AssetReferenceGameObject MainMenuCamera;
        public AssetReferenceGameObject UICamera;
        public AssetReferenceGameObject GameCamera;
        public AssetReferenceGameObject LoadingCurtain;
        public AssetReferenceGameObject BridgeTile;
        public AssetReferenceGameObject VictoryWindow;
        public AssetReferenceGameObject LoseWindow;
        public Level[] Levels;
    }
}