using Architecture.Services.Interfaces;
using Architecture.States.Interfaces;
using Audio;

namespace Architecture.States
{
    public class LoadMainMenuState : IState
    {
        private const string MainMenuScene = "MainMenu";
        
        private readonly ISceneLoader _sceneLoader;
        private readonly IAudioService _audioService;

        public LoadMainMenuState(ISceneLoader sceneLoader, IAudioService audioService)
        {
            _sceneLoader = sceneLoader;
            _audioService = audioService;
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
            _audioService.PlayMusic(MusicType.MainMenu);
        }
    }
}