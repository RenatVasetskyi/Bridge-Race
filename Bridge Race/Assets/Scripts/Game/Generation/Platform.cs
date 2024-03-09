using System;
using System.Collections.ObjectModel;
using Game.BridgeConstruction;
using UnityEngine;

namespace Game.Generation
{
    public class Platform : MonoBehaviour
    {
        [NonSerialized] public ObservableCollection<BridgeTile> Tiles = new();
    }
}