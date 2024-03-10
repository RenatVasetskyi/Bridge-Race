using Game.Character.Animations;
using Game.Character.StateMachine.Interfaces;

namespace Game.Character.AI.States
{
    public class BotDeliverTilesToBridgeState : ICharacterState
    {
        private readonly PlayerAnimator _playerAnimator;

        public BotDeliverTilesToBridgeState(PlayerAnimator playerAnimator)
        {
            _playerAnimator = playerAnimator;
        }
        
        public void Enter()
        {
            _playerAnimator.PlayWalkAnimation();   
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