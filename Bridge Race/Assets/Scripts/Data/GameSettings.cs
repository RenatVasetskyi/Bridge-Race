using Game.Character.AI;
using Game.Character.Data;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Game Settings", menuName = "Create Settings Holder/Game Settings")]
    public class GameSettings : ScriptableObject
    {
        public AddressablePrefabs Prefabs;

        [Header("Player")]
        
        public PlayerData PlayerData;

        public Sprite DefaultUserPhoto;
        
        [Header("Bot")]
        public BotData BotData;
    }
}
