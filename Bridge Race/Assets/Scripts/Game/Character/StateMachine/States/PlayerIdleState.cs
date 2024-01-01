using Game.Character.Animations;
using Game.Character.StateMachine.Interfaces;

namespace Game.Character.StateMachine.States
{
    public class PlayerIdleState : ICharacterState
    {
        private readonly PlayerAnimator _playerAnimator;

        public PlayerIdleState(PlayerAnimator playerAnimator)
        {
            _playerAnimator = playerAnimator;
        }
        
        public void Enter()
        {
            _playerAnimator.PlayIdleAnimation();
        }

        public void Exit()
        {
        }

        public void FrameUpdate()
        {
        }

        public void PhysicsUpdate()
        {
        }
    }
}