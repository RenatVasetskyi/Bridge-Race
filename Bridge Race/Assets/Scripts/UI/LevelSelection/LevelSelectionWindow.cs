using Architecture.Services.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.LevelSelection
{
    public class LevelSelectionWindow : MonoBehaviour
    {
        [SerializeField] private Button[] _levelSelectors;

        private ILevelProgressService _levelProgressService;
        
        [Inject]
        public void Construct(ILevelProgressService levelProgressService)
        {
            _levelProgressService = levelProgressService;
        }

        private void OnEnable()
        {
            UpdateSelectors();
        }

        private void UpdateSelectors()
        {
            for (int i = 0; i <= (int)_levelProgressService.CurrentLevelToPass; i++)
                _levelSelectors[i].interactable = true;
            
            for (int i = (int)_levelProgressService.CurrentLevelToPass + 1; i < _levelSelectors.Length; i++)
                _levelSelectors[i].interactable = false;
        }
    }
}