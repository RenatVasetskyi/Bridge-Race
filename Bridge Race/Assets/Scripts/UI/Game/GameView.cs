using Architecture.Services.Interfaces;
using Data;
using Game.Levels.Interfaces;
using UI.Game.Tutorial;
using UnityEngine;
using Zenject;

namespace UI.Game
{
    public class GameView : MonoBehaviour
    {
        public Joystick Joystick;

        [SerializeField] private AnimatedHand _animatedHand;
        [SerializeField] private GameObject _tutorial;

        private IBaseFactory _baseFactory;
        private GameSettings _gameSettings;
        
        private IGameOverReporter _gameOverReporter;
        
        [Inject]
        public void Construct(IBaseFactory baseFactory, GameSettings gameSettings)
        {
            _baseFactory = baseFactory;
            _gameSettings = gameSettings;
        }
        
        public void Initialize(IGameOverReporter gameOverReporter)
        {
            _gameOverReporter = gameOverReporter;

            Subscribe();
        }
        
        private void Awake()
        {
            Joystick.OnInputActivated += HideTutorial;
        }

        private void Start()
        {
            ShowTutorial();
        }

        private void OnDestroy()
        {
            UnSubscribe();
        }

        private void HideTutorial()
        {
            Joystick.OnInputActivated -= HideTutorial;
            
            Destroy(_tutorial);
        }

        private void ShowTutorial()
        {
            _tutorial.SetActive(true);
            
            _animatedHand.DoAnimation();
        }

        private void ShowVictoryWindow()
        {
            _baseFactory.CreateGameOverWindow(_gameSettings.Prefabs.VictoryWindow, transform);
        }

        private void ShowLoseWindow()
        {
            _baseFactory.CreateGameOverWindow(_gameSettings.Prefabs.LoseWindow, transform);
        }

        private void Subscribe()
        {
            _gameOverReporter.OnWin += ShowVictoryWindow;
            _gameOverReporter.OnLose += ShowLoseWindow;
        }

        private void UnSubscribe()
        {
            _gameOverReporter.OnWin -= ShowVictoryWindow;
            _gameOverReporter.OnLose -= ShowLoseWindow;
        }
    }
}