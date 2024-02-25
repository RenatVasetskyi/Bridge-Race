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
        private readonly GameSettings _gameSettings;
        private readonly IAssetProvider _assetProvider;

        public LoadMainMenuState(ISceneLoader sceneLoader, IAudioService audioService, 
            IBaseFactory baseFactory, GameSettings gameSettings, 
            IAssetProvider assetProvider)
        {
            _sceneLoader = sceneLoader;
            _audioService = audioService;
            _baseFactory = baseFactory;
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
            _baseFactory.CreateLoadingCurtain(_gameSettings.Prefabs.LoadingCurtain);

            _sceneLoader.Load(MainMenuScene, Initialize);
        }

        private async void Initialize()
        {
            Transform parent = (await _baseFactory.CreateAddressableWithObject
                (_gameSettings.Prefabs.BaseParent, Vector3.zero, Quaternion.identity, null)).transform;

            Camera camera = (await _baseFactory.CreateAddressableWithContainer
                (_gameSettings.Prefabs.MainMenuCamera, Vector3.zero, Quaternion.identity, parent)).GetComponent<Camera>();
            
            Canvas mainMenu = (await _baseFactory.CreateAddressableWithContainer
                (_gameSettings.Prefabs.MainMenu, Vector3.zero, Quaternion.identity, parent)).GetComponent<Canvas>();
            mainMenu.worldCamera = camera;
            
            _audioService.PlayMusic(MusicType.MainMenu);

            if (_baseFactory.LoadingCurtain != null)
                _baseFactory.LoadingCurtain.Hide();
        }
    }
}