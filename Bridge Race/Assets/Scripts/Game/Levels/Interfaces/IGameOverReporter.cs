using System;

namespace Game.Levels.Interfaces
{
    public interface IGameOverReporter
    {
        event Action OnWin;
        event Action OnLose;
    }
}