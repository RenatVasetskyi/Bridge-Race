using Architecture.Services.Interfaces;
using Architecture.States.Interfaces;
using Audio;
using Data;
using UnityEngine;

namespace Architecture.States
{
    public class LoadMainMenuState : IState
    {
        private const string MainMenuScene = "MainMenu";
        
        private readonly ISceneLoader _sceneLoader;
        private readonly IAudioService _audioService;
        private readonly IBaseFactory _baseFactory;
        private readonly IUIFactory _uiFactory;
        private readonly GameSettings _gameSettings;
        private readonly IAssetProvider _assetProvider;

        public LoadMainMenuState(ISceneLoader sceneLoader, IAudioService audioService, 
            IBaseFactory baseFactory, IUIFactory uiFactory, GameSettings gameSettings, 
            IAssetProvider assetProvider)
        {
            _sceneLoader = sceneLoader;
            _audioService = audioService;
            _baseFactory = baseFactory;
            _uiFactory = uiFactory;
            _gameSettings = gameSettings;
            _assetProvider = assetProvider;
        }
        
        public void Exit()
        {
            _assetProvider.CleanUp();
            
            _audioService.StopMusic();
        }

        public void Enter()
        {
            _uiFactory.CreateLoadingCurtain();

            _sceneLoader.Load(MainMenuScene, Initialize);
        }

        private async void Initialize()
        {
            Transform parent = (await _baseFactory.CreateAddressableWithObject
                (_gameSettings.Prefabs.BaseParent, Vector3.zero, Quaternion.identity, null)).transform;

            Camera camera = (await _baseFactory.CreateAddressableWithContainer
                (_gameSettings.Prefabs.MainMenuCamera, Vector3.zero, Quaternion.identity, parent)).GetComponent<Camera>();
            
            Canvas mainMenuCanvas = (await _baseFactory.CreateAddressableWithContainer
                (_gameSettings.Prefabs.BaseCanvas, Vector3.zero, Quaternion.identity, parent)).GetComponent<Canvas>();
            mainMenuCanvas.worldCamera = camera;
            
            _uiFactory.CreateFullScreenWindow(_gameSettings.Prefabs.MainMenuWindow, mainMenuCanvas.transform);
            
            _audioService.PlayMusic(MusicType.MainMenu);

            if (_uiFactory.LoadingCurtain != null)
                _uiFactory.LoadingCurtain.Hide();
        }
    }
}