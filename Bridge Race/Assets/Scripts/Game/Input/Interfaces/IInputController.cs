using System;
using UnityEngine;

namespace Game.Input.Interfaces
{
    public interface IInputController
    {
        event Action OnInputActivated;
        event Action OnInputDeactivated;
        Vector2 CurrentDirection { get; }
        void Disable();
    }
}