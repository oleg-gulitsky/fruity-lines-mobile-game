using System;
using System.Collections.Generic;
using Infrastructure.StateMachine.States;
using Zenject;

namespace Infrastructure.StateMachine

{
  public class GameStateMachine : IGameStateMachine
  {
    private readonly Dictionary<Type, IExitableState> _states;
    private IExitableState _activeState;
    private readonly IInstantiator _container;
    
    public GameStateMachine(IInstantiator container)
    {
      _container = container;
    }

    public void Enter<TState>() where TState : class, IState
    {
      IState state = ChangeState<TState>();
      state.Enter();
    }

    public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
    {
      TState state = ChangeState<TState>();
      state.Enter(payload);
    }

    private TState ChangeState<TState>() where TState : class, IExitableState
    {
      _activeState?.Exit();
      
      TState state = Create<TState>();
      _activeState = state;
      
      return state;
    }

    private TState GetState<TState>() where TState : class, IExitableState =>
      _states[typeof(TState)] as TState;
    
    private TState Create<TState>() where TState : class, IExitableState =>
      _container.Instantiate<TState>();
  }
}