using Infrastructure.StateMachine.States;

namespace Infrastructure.StateMachine
{
  public interface IGameStateMachine
  {
    void Enter<TState>() where TState : class, IState;
    void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>;
  }
}