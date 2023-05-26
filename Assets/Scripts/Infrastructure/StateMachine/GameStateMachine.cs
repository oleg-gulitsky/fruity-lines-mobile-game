using System;
using System.Collections.Generic;
using GameLogic;
using Infrastructure.SceneLoading;
using Infrastructure.StateMachine.States;
using Services.Ads;
using Services.Progress;

namespace Infrastructure.StateMachine

{
  public class GameStateMachine : IGameStateMachine
  {
    private readonly Dictionary<Type, IExitableState> _states;
    private IExitableState _activeState;
    
    public GameStateMachine(
      ISceneLoader sceneLoader,
      IProgressService progressService,
      IAdsService adsService,
      IGameLogic gameLogic)
    {
      _states = new Dictionary<Type, IExitableState>()
      {
        [typeof(BootstrapState)] = new BootstrapState(this, progressService, adsService),
        [typeof(GameplayState)] = new GameplayState(sceneLoader, gameLogic)
      };
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
      
      TState state = GetState<TState>();
      _activeState = state;
      
      return state;
    }

    private TState GetState<TState>() where TState : class, IExitableState =>
      _states[typeof(TState)] as TState;
  }
}