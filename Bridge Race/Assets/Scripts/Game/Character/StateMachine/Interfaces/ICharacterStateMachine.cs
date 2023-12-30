namespace Game.Character.StateMachine.Interfaces
{
    public interface ICharacterStateMachine
    {
        public ICharacterState ActiveState { get; }
        void EnterState<TState>() where TState : class, ICharacterState;
        void AddState<TState>(ICharacterState state);
        bool CompareStateWithActive<TState>() where TState : class, ICharacterState;
        bool CompareStateWithLast<TState>() where TState : class, ICharacterState;
    }
}