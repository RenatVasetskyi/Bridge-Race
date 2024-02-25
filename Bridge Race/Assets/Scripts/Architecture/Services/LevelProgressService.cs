using System.Linq;
using Architecture.Services.Interfaces;
using Data;
using Game.Levels;
using Game.Levels.Enums;

namespace Architecture.Services
{
    public class LevelProgressService : ILevelProgressService
    {
        private const string CurrentLevelToPassSaveId = "CurrentLevelToPass";
        
        private readonly GameSettings _gameSettings;
        private readonly ISaveService _saveService;

        public LevelType CurrentLevelToPass { get; private set; }

        public LevelProgressService(GameSettings gameSettings, ISaveService saveService)
        {
            _gameSettings = gameSettings;
            _saveService = saveService;
        }
        
        public void SetLevelToPass(LevelType level)
        {
            CurrentLevelToPass = level;
            
            _saveService.SaveString(CurrentLevelToPassSaveId, level.ToString());
        }
        
        public void Load()
        {
            if (_saveService.HasKey(CurrentLevelToPassSaveId))
            {
                string lastPassedLevel = _saveService.LoadString(CurrentLevelToPassSaveId);
                
                CurrentLevelToPass = _gameSettings.Prefabs.Levels.FirstOrDefault(x => x
                    .Type.ToString() == lastPassedLevel)!.Type;
            }
            else 
                CurrentLevelToPass = _gameSettings.Prefabs.Levels.First().Type;
        }

        public Level GetCurrentLevelToPass()
        {
            Level selectedLevel = _gameSettings.Prefabs.Levels.FirstOrDefault(x=> x.Type == CurrentLevelToPass);

            if (selectedLevel != null)
                return selectedLevel;

            return _gameSettings.Prefabs.Levels.First();
        }

        public void SetNextLevelToPass()
        {
            int currentLevelToPass = (int)GetCurrentLevelToPass().Type;

            if (currentLevelToPass < _gameSettings.Prefabs.Levels.Length - 1 & 
                currentLevelToPass == (int)CurrentLevelToPass)
            {
                CurrentLevelToPass = _gameSettings.Prefabs.Levels[currentLevelToPass + 1].Type;
                
                Save();
            }
        }

        private void Save()
        {
            _saveService.SaveString(CurrentLevelToPassSaveId, CurrentLevelToPass.ToString());
        }
    }
}