using System;

namespace Game.Input.Interfaces
{
    public interface IInputController
    {
        event Action OnInputActivated;
        event Action OnInputDeactivated;
    }
}