using Architecture.Services.Interfaces;
using Architecture.States.Interfaces;
using UnityEngine.Device;
using IState = Architecture.States.Interfaces.IState;

namespace Architecture.States
{
    public class BootstrapState : IState
    {
        private const string BootSceneName = "Boot";

        private const int TargetFrameRate = 120;
        
        private readonly IStateMachine _stateMachine;
        private readonly IAudioService _audioService;
        private readonly ICurrencyService _currencyService;
        private readonly ISceneLoader _sceneLoader;
        private readonly ILevelProgressService _levelProgressService;
        private readonly IUserDataStorage _userDataStorage;

        public BootstrapState(IStateMachine stateMachine, IAudioService audioService,
            ICurrencyService currencyService, ISceneLoader sceneLoader, 
            ILevelProgressService levelProgressService, IUserDataStorage userDataStorage) 
        {
            _stateMachine = stateMachine;
            _audioService = audioService;
            _currencyService = currencyService;
            _sceneLoader = sceneLoader;
            _levelProgressService = levelProgressService;
            _userDataStorage = userDataStorage;
        }

        public void Exit()
        {
        }

        public void Enter()
        {
            _sceneLoader.Load(BootSceneName, Initialize);
        }

        private void Initialize()
        {
            Application.targetFrameRate = TargetFrameRate; 
            
            _audioService.Initialize();
            _currencyService.Load();
            _levelProgressService.Load();
            _userDataStorage.Load();
            
            _stateMachine.Enter<LoadMainMenuState>();
        }
    }
}