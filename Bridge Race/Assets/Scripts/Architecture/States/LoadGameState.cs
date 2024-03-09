using System.Linq;
using Architecture.Services.Interfaces;
using Architecture.States.Interfaces;
using Audio;
using Data;
using Game.Camera;
using Game.Character;
using Game.Character.AI;
using Game.Levels;
using UI.Game;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Architecture.States
{
    public class LoadGameState : IState
    {
        private const string GameScene = "Game";
        
        private readonly ISceneLoader _sceneLoader;
        private readonly IGamePauser _gamePauser;
        private readonly IAudioService _audioService;
        private readonly IBaseFactory _baseFactory;
        private readonly IUIFactory _uiFactory;
        private readonly IAssetProvider _assetProvider;
        private readonly GameSettings _gameSettings;
        private readonly ILevelProgressService _levelProgressService;

        public LoadGameState(ISceneLoader sceneLoader, IGamePauser gamePauser,
            IAudioService audioService, IBaseFactory baseFactory,
            IUIFactory uiFactory, IAssetProvider assetProvider,
            GameSettings gameSettings, ILevelProgressService levelProgressService)
        {
            _sceneLoader = sceneLoader;
            _gamePauser = gamePauser;
            _audioService = audioService;
            _baseFactory = baseFactory;
            _uiFactory = uiFactory;
            _assetProvider = assetProvider;
            _gameSettings = gameSettings;
            _levelProgressService = levelProgressService;
        }
        
        public void Exit()
        {
            _assetProvider.CleanUp();
            
            _audioService.StopMusic();
        }

        public void Enter()
        {
            _uiFactory.CreateLoadingCurtain();
            
            _sceneLoader.Load(GameScene, Initialize);
        }

        private async void Initialize()
        {
            _gamePauser.Clear();
            _gamePauser.SetPause(false);
            
            Transform parent = (await _baseFactory.CreateAddressableWithObject
                (_gameSettings.Prefabs.BaseParent, Vector3.zero, Quaternion.identity, null)).transform;

            Camera gameCamera = (await _baseFactory.CreateAddressableWithContainer
                (_gameSettings.Prefabs.GameCamera, Vector3.zero, Quaternion.identity, parent)).GetComponent<Camera>();
            
            Camera uiCamera = (await _baseFactory.CreateAddressableWithContainer
                (_gameSettings.Prefabs.UICamera, Vector3.zero, Quaternion.identity, parent)).GetComponent<Camera>();
            
            UniversalAdditionalCameraData cameraData = gameCamera.GetComponent<UniversalAdditionalCameraData>();
            cameraData.cameraStack.Add(uiCamera);
            
            GameView gameView = (await _baseFactory.CreateAddressableWithContainer
                (_gameSettings.Prefabs.GameView, Vector3.zero, Quaternion.identity, parent)).GetComponent<GameView>();
            gameView.GetComponent<Canvas>().worldCamera = uiCamera;
            
            Level level = (await _baseFactory.CreateAddressableWithContainer
                (_levelProgressService.GetCurrentLevelToPass().Prefab,
                    Vector3.zero, Quaternion.identity, parent)).GetComponent<Level>();
            
            gameView.Initialize(level.Finish);
            
            Player player = (await _baseFactory.CreateAddressableWithContainer
                (_gameSettings.Prefabs.Player, level.PlayerSpawnPoint.position, Quaternion.identity, parent))
                .GetComponent<Player>();
            player.Initialize(gameView.Joystick);
            
            Bot bot = (await _baseFactory.CreateAddressableWithContainer
                    (_gameSettings.Prefabs.Bot, level.BotSpawnPoint.position, Quaternion.identity, parent))
                .GetComponent<Bot>();
            bot.Initialize(level.Platforms.First());
            
            CameraFollowTarget cameraFollowTarget = gameCamera.GetComponent<CameraFollowTarget>();
            cameraFollowTarget.Initialize(player.transform);

            _audioService.PlayMusic(MusicType.Game);

            if (_uiFactory.LoadingCurtain != null)
                _uiFactory.LoadingCurtain.Hide();
        }
    }
}