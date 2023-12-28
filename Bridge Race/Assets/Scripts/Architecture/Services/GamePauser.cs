using System.Collections.Generic;
using System.Linq;
using Architecture.Services.Interfaces;

namespace Architecture.Services
{
    public class GamePauser : IGamePauser
    {
        private readonly List<IPauseHandler> _handlers = new();

        public bool IsPaused { get; private set; }

        public void Register(IPauseHandler handler)
        {
            _handlers.Add(handler);
        }

        public void UnRegister(IPauseHandler handler)
        {
            _handlers.Remove(handler);
        }
        
        public void SetPause(bool isPaused)
        {
            IsPaused = isPaused;
            
            foreach (IPauseHandler handler in _handlers.ToList())
                handler.SetPause(isPaused);
        }
        
        public void Clear()
        {
            _handlers.Clear();
        }
    }
}