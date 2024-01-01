using UI.Game.Tutorial;
using UnityEngine;

namespace UI.Game
{
    public class GameView : MonoBehaviour
    {
        public Joystick Joystick;

        [SerializeField] private AnimatedHand _animatedHand;
        [SerializeField] private GameObject _tutorial;

        private void Awake()
        {
            Joystick.OnInputActivated += HideTutorial;
        }

        private void Start()
        {
            ShowTutorial();
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
    }
}