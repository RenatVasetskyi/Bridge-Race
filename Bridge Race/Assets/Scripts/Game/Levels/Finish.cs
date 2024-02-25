using System;
using Architecture.Services.Interfaces;
using Game.Character.Interfaces;
using Game.Levels.Interfaces;
using UnityEngine;
using Zenject;

namespace Game.Levels
{
    public class Finish : MonoBehaviour, IGameOverReporter
    {
        public event Action OnWin;
        public event Action OnLose;
        
        private ILevelProgressService _levelProgressService;

        private bool _isDetected;
        
        [Inject]
        public void Construct(ILevelProgressService levelProgressService)
        {
            _levelProgressService = levelProgressService;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!_isDetected & other.gameObject.TryGetComponent(out IFinishDetectable finishDetectable))
            {
                SetGameFinished(finishDetectable);
            }
        }

        private void SetGameFinished(IFinishDetectable finishDetectable)
        {
            _isDetected = true;
                
            finishDetectable.DoFinishAnimation(transform);
                
            _levelProgressService.SetNextLevelToPass();
            
            OnWin?.Invoke();
        }
    }
}