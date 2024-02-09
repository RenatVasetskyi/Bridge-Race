using Architecture.Services.Interfaces;
using Game.Character.Interfaces;
using UnityEngine;
using Zenject;

namespace Game.Levels
{
    public class Finish : MonoBehaviour
    {
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
                _isDetected = true;
                
                finishDetectable.DoFinishAnimation(transform);
                
                _levelProgressService.SetNextLevelToPass();
            }
        }
    }
}