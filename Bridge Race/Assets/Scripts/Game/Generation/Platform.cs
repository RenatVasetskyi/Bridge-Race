using System;
using System.Collections.Generic;
using Game.BridgeConstruction;
using UnityEngine;

namespace Game.Generation
{
    public class Platform : MonoBehaviour
    {
        [NonSerialized] public List<BridgeTile> Tiles = new();
    }
}