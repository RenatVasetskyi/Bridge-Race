using Architecture.Services.Interfaces;
using Game.Levels.Enums;
using UnityEngine;
using Zenject;

namespace UI.Base
{
    public class LoadLevelButton : StateTransferButton
    {
        [SerializeField] private LevelType _level;

        private ILevelProgressService _levelProgressService;
        
        [Inject]
        public void Construct(ILevelProgressService levelProgressService)
        {
            _levelProgressService = levelProgressService;
        }
        
        protected override void ChangeState()
        {
            _levelProgressService.SetLevelToPass(_level);
            
            base.ChangeState();
        }
    }
}