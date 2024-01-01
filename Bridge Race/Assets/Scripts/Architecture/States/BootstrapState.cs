using Architecture.Services.Interfaces;
using Architecture.States.Interfaces;
using IState = Architecture.States.Interfaces.IState;

namespace Architecture.States
{
    public class BootstrapState : IState
    {
        private const string BootSceneName = "Boot";
        
        private readonly IStateMachine _stateMachine;
        private readonly IAudioService _audioService;
        private readonly ICurrencyService _currencyService;
        private readonly ISceneLoader _sceneLoader;
        private readonly ILevelProgressService _levelProgressService;

        public BootstrapState(IStateMachine stateMachine, IAudioService audioService,
            ICurrencyService currencyService, ISceneLoader sceneLoader, 
            ILevelProgressService levelProgressService) 
        {
            _stateMachine = stateMachine;
            _audioService = audioService;
            _currencyService = currencyService;
            _sceneLoader = sceneLoader;
            _levelProgressService = levelProgressService;
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
            _audioService.Initialize();
            _currencyService.Load();
            _levelProgressService.Load();
            
            _stateMachine.Enter<LoadMainMenuState>();
        }
    }
}