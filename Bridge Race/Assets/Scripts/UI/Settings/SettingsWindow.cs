using Architecture.Services.Interfaces;
using Data;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Settings
{
    public class SettingsWindow : MonoBehaviour
    {
        [SerializeField] private Button _editNameButton;
        [SerializeField] private Button _closeButton;

        private IUIFactory _uiFactory;
        private GameSettings _gameSettings;
        
        [Inject]
        public void Construct(IUIFactory uiFactory, GameSettings gameSettings)
        {
            _uiFactory = uiFactory;
            _gameSettings = gameSettings;
        }

        private void OnEnable()
        {
            _editNameButton.onClick.AddListener(OpenEditNameWindow);
            _closeButton.onClick.AddListener(Destroy);
        }

        private void OnDisable()
        {
            _editNameButton.onClick.RemoveListener(OpenEditNameWindow);
            _closeButton.onClick.RemoveListener(Destroy);
        }

        private void OpenEditNameWindow()
        {
            _uiFactory.CreateFullScreenWindow(_gameSettings.Prefabs.EditNameWindow, transform.parent);  
        }

        private void Destroy()
        {
            Destroy(gameObject);
        }
    }
}