namespace Architecture.States.Interfaces
{
    public interface IState : IExitableState
    {
        void Enter();
    }
}