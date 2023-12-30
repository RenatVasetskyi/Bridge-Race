using System.Threading.Tasks;
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
            _sceneLoader.Load(MainMenuScene, Initialize);
        }

        private async void Initialize()
        {
            Transform parent = _baseFactory.CreateBaseWithObject<Transform>(AssetPath.BaseParent);
            
            Camera camera = _baseFactory.CreateBaseWithContainer<Camera>(AssetPath.BaseCamera, parent);
            
            GameObject mainMenu = await _baseFactory.CreateAddressableWithContainer
                (_gameSettings.MainMenu, Vector3.zero, Quaternion.identity, parent);
            mainMenu.GetComponent<Canvas>().worldCamera = camera;
            
            _audioService.PlayMusic(MusicType.MainMenu);
        }
    }
}