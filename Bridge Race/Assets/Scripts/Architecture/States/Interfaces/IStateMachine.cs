using System;
using System.Collections.Generic;

namespace Architecture.States.Interfaces
{
    public interface IStateMachine
    {
        Dictionary<Type, IExitableState> States { get; set; }
        void Enter<TState>() where TState : class, IState;
    }
}