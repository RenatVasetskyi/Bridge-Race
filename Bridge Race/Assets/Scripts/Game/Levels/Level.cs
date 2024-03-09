using Game.Generation;
using Game.Levels.Enums;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Levels
{
    public class Level : MonoBehaviour
    {
        public LevelType Type;
        public AssetReferenceGameObject Prefab;
        public Transform PlayerSpawnPoint;
        public Transform BotSpawnPoint;
        public Finish Finish;
        public Platform[] Platforms;
    }
}