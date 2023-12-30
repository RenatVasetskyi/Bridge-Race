using Game.Input.Interfaces;
using UnityEngine;

namespace Game.Character
{
    public class Player : MonoBehaviour
    {
        private IInputController _inputController;
        
        public void Initialize(IInputController inputController)
        {
            _inputController = inputController;
            
            Subscribe();
        }

        private void OnDestroy()
        {
            UnSubscribe();
        }

        private void Subscribe()
        {
            _inputController.OnInputActivated += EnterMovementState;
            _inputController.OnInputDeactivated += EnterIdleState;
        }

        private void UnSubscribe()
        {
            _inputController.OnInputActivated -= EnterMovementState;
            _inputController.OnInputDeactivated -= EnterIdleState;   
        }

        private void EnterMovementState()
        {
            
        }

        private void EnterIdleState()
        {
            
        }
    }
}