using System.Linq;
using Architecture.Services.Interfaces;
using Data;
using Game.Levels;
using Game.Levels.Enums;
using UnityEngine.AddressableAssets;

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
                
                CurrentLevelToPass = _gameSettings.Levels.FirstOrDefault(x => x
                    .Type.ToString() == lastPassedLevel)!.Type;
            }
            else 
                CurrentLevelToPass = _gameSettings.Levels.First().Type;
        }

        public Level GetCurrentLevelToPass()
        {
            Level selectedLevel = _gameSettings.Levels.FirstOrDefault(x=> x.Type == CurrentLevelToPass);

            if (selectedLevel != null)
                return selectedLevel;

            return _gameSettings.Levels.First();
        }
    }
}