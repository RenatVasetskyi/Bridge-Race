using Game.Levels;
using Game.Levels.Enums;

namespace Architecture.Services.Interfaces
{
    public interface ILevelProgressService
    {
        LevelType CurrentLevelToPass { get; }
        void SetLevelToPass(LevelType level);
        void Load();
        Level GetCurrentLevelToPass();
        void SetNextLevelToPass();
    }
}