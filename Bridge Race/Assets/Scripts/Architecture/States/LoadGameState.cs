using Architecture.Services.Interfaces;
using Architecture.States.Interfaces;
using Audio;
using Data;
using Game.Character;
using Game.Levels;
using UI.Game;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Architecture.States
{
    public class LoadGameState : IState
    {
        private const string GameScene = "Game";
        
        private readonly ISceneLoader _sceneLoader;
        private readonly IGamePauser _gamePauser;
        private readonly IAudioService _audioService;
        private readonly IBaseFactory _baseFactory;
        private readonly IAssetProvider _assetProvider;
        private readonly GameSettings _gameSettings;
        private readonly ILevelProgressService _levelProgressService;

        public LoadGameState(ISceneLoader sceneLoader, IGamePauser gamePauser,
            IAudioService audioService, IBaseFactory baseFactory, 
            IAssetProvider assetProvider, GameSettings gameSettings, 
            ILevelProgressService levelProgressService)
        {
            _sceneLoader = sceneLoader;
            _gamePauser = gamePauser;
            _audioService = audioService;
            _baseFactory = baseFactory;
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
            _sceneLoader.Load(GameScene, Initialize);
        }

        private async void Initialize()
        {
            _gamePauser.Clear();
            _gamePauser.SetPause(false);
            
            Transform parent = (await _baseFactory.CreateAddressableWithObject
                (_gameSettings.BaseParent, Vector3.zero, Quaternion.identity, null)).transform;

            Camera camera = (await _baseFactory.CreateAddressableWithContainer
                (_gameSettings.BaseCamera, Vector3.zero, Quaternion.identity, parent)).GetComponent<Camera>();
            
            GameView gameView = (await _baseFactory.CreateAddressableWithContainer
                (_gameSettings.GameView, Vector3.zero, Quaternion.identity, parent)).GetComponent<GameView>();
            gameView.GetComponent<Canvas>().worldCamera = camera;
            
            Level level = (await _baseFactory.CreateAddressableWithContainer
                (_levelProgressService.GetCurrentLevelToPass().Prefab, Vector3.zero, Quaternion.identity, parent)).GetComponent<Level>();
            
            Player player = (await _baseFactory.CreateAddressableWithContainer
                (_gameSettings.Player, level.PlayerSpawnPoint.position, Quaternion.identity, parent))
                .GetComponent<Player>();
            player.Initialize(gameView.Joystick);

            _audioService.PlayMusic(MusicType.Game);
        }
    }
}