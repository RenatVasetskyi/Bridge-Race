using Architecture.Services.Interfaces;
using Architecture.States.Interfaces;
using Audio;
using Data;
using UI.Game;
using UnityEngine;

namespace Architecture.States
{
    public class LoadGameState : IState
    {
        private const string GameScene = "Game";
        
        private readonly ISceneLoader _sceneLoader;
        private readonly IGamePauser _gamePauser;
        private readonly IAudioService _audioService;
        private readonly IBaseFactory _baseFactory;

        public LoadGameState(ISceneLoader sceneLoader, IGamePauser gamePauser,
            IAudioService audioService, IBaseFactory baseFactory)
        {
            _sceneLoader = sceneLoader;
            _gamePauser = gamePauser;
            _audioService = audioService;
            _baseFactory = baseFactory;
        }
        
        public void Exit()
        {
            _audioService.StopMusic();
        }

        public void Enter()
        {
            _sceneLoader.Load(GameScene, Initialize);
        }

        private void Initialize()
        {
            _gamePauser.Clear();
            _gamePauser.SetPause(false);
            
            Transform parent = _baseFactory.CreateBaseWithObject<Transform>(AssetPath.BaseParent);
            
            Camera camera = _baseFactory.CreateBaseWithContainer<Camera>(AssetPath.BaseCamera, parent);
            
            GameView gameView = _baseFactory.CreateBaseWithContainer<GameView>(AssetPath.GameView, parent);
            gameView.GetComponent<Canvas>().worldCamera = camera;
            
            _audioService.PlayMusic(MusicType.Game);
        }
    }
}