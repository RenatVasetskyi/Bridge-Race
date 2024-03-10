using System;
using System.Collections.ObjectModel;
using Game.BridgeConstruction;
using UnityEngine;

namespace Game.Levels
{
    public class Platform : MonoBehaviour
    {
        public Bridge[] Bridges;
        [NonSerialized] public ObservableCollection<BridgeTile> Tiles = new();
    }
}