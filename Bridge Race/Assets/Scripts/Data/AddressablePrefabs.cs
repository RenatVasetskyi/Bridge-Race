using System;
using Game.Levels;
using UnityEngine.AddressableAssets;

namespace Data
{
    [Serializable]
    public class AddressablePrefabs
    {
        public Level[] Levels;
        public AssetReferenceGameObject BaseCanvas;
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
        public AssetReferenceGameObject EditNameWindow;
        public AssetReferenceGameObject SettingsWindow;
        public AssetReferenceGameObject MainMenuWindow;
        public AssetReferenceGameObject Bot;
    }
}