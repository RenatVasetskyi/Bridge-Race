using Game.Character.Data;
using Game.Levels;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Data
{
    [CreateAssetMenu(fileName = "Game Settings", menuName = "Create Settings Holder/Game Settings")]
    public class GameSettings : ScriptableObject
    {
        [Header("Prefabs")]
        
        public AssetReferenceGameObject MainMenu;
        public AssetReferenceGameObject GameView;
        public AssetReferenceGameObject BaseParent;
        public AssetReferenceGameObject Player;
        public AssetReferenceGameObject MainMenuCamera;
        public AssetReferenceGameObject UICamera;
        public AssetReferenceGameObject GameCamera;
        public AssetReferenceGameObject LoadingCurtain;
        public AssetReferenceGameObject BridgeTile;
        
        public Level[] Levels;

        [Header("Player")]
        
        public PlayerData PlayerData;
    }
}
