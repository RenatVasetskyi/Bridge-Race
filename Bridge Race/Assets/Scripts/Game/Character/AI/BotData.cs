using System;

namespace Game.Character.AI
{
    [Serializable]
    public class BotData
    {
        public float Speed;
        public int MaxTilesInHands;
        public float RotationSpeed;
        public double StopThreshold;
    }
}