using Game.Character.Data;
using Game.Levels;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Data
{
    [CreateAssetMenu(fileName = "Game Settings", menuName = "Create Settings Holder/Game Settings")]
    public class GameSettings : ScriptableObject
    {
        public AddressablePrefabs Prefabs;

        [Header("Player")]
        
        public PlayerData PlayerData;

        public Sprite DefaultUserPhoto;
    }
}
