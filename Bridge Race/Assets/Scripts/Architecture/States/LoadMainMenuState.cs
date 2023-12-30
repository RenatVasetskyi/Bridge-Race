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

        public LoadMainMenuState(ISceneLoader sceneLoader, IAudioService audioService, 
            IBaseFactory baseFactory)
        {
            _sceneLoader = sceneLoader;
            _audioService = audioService;
            _baseFactory = baseFactory;
        }
        
        public void Exit()
        {
            _audioService.StopMusic();
        }

        public void Enter()
        {
            _sceneLoader.Load(MainMenuScene, Initialize);
        }

        private void Initialize()
        {
            Transform parent = _baseFactory.CreateBaseWithObject<Transform>(AssetPath.BaseParent);
            
            Camera camera = _baseFactory.CreateBaseWithContainer<Camera>(AssetPath.BaseCamera, parent);
            
            Canvas mainMenu = _baseFactory.CreateBaseWithContainer<Canvas>(AssetPath.MainMenu, parent);
            mainMenu.worldCamera = camera;
            
            _audioService.PlayMusic(MusicType.MainMenu);
        }
    }
}