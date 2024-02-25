using Architecture.Services.Interfaces;
using Data;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Menu
{
    public class MainMenuWindow : MonoBehaviour
    {
        [SerializeField] private Button _settingsWindowButton;

        private IUIFactory _uiFactory;
        private GameSettings _gameSettings;
        
        [Inject]
        private void Construct(IUIFactory uiFactory, GameSettings gameSettings)
        {
            _uiFactory = uiFactory;
            _gameSettings = gameSettings;
        }

        private void OnEnable()
        {
            _settingsWindowButton.onClick.AddListener(OpenSettingsWindow);
        }

        private void OnDisable()
        {
            _settingsWindowButton.onClick.RemoveListener(OpenSettingsWindow);
        }

        private void OpenSettingsWindow()
        {
            _uiFactory.CreateFullScreenWindow(_gameSettings.Prefabs.SettingsWindow, transform.parent);
        }
    }
}