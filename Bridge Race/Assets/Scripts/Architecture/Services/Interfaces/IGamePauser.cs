namespace Architecture.Services.Interfaces
{
    public interface IGamePauser : IPauseHandler
    {
        public bool IsPaused { get; }
        void Register(IPauseHandler handler);
        void UnRegister(IPauseHandler handler);
        void Clear();
    }
}