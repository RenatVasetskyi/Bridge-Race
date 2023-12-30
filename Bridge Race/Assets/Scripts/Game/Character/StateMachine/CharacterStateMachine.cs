using System;
using System.Collections.Generic;
using Game.Character.StateMachine.Interfaces;

namespace Game.Character.StateMachine
{
    public class CharacterStateMachine : ICharacterStateMachine
    {
        private readonly Dictionary<Type, ICharacterState> _states = new();
        
        private ICharacterState _lastState;

        public ICharacterState ActiveState { get; private set; }

        public void EnterState<TState>() where TState : class, ICharacterState
        {
            ActiveState?.Exit();
            
            _lastState = ActiveState;

            TState state = GetState<TState>();
            state.Enter();

            ActiveState = state;
        }
        
        public bool CompareStateWithActive<TState>() where TState : class, ICharacterState
        {
            TState state = GetState<TState>();
            
            return state.Equals(ActiveState);
        }

        public bool CompareStateWithLast<TState>() where TState : class, ICharacterState
        {
            TState state = GetState<TState>();
            
            return state.Equals(_lastState);
        }

        public void AddState<TState>(ICharacterState state)
        {
            _states.Add(typeof(TState), state);
        }

        private TState GetState<TState>() where TState : class, ICharacterState
        {
            return _states[typeof(TState)] as TState;
        }
    }
}