using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Data
{
    [CreateAssetMenu(fileName = "Game Settings", menuName = "Create Settings Holder/Game Settings")]
    public class GameSettings : ScriptableObject
    {
        public AssetReferenceGameObject MainMenu;
        public AssetReferenceGameObject GameView;
        public AssetReferenceGameObject BaseParent;
        public AssetReferenceGameObject Player;
        public AssetReferenceGameObject BaseCamera;
    }
}
