using Services.Progress;

namespace Infrastructure.StateMachine.States
{
  public class BootstrapState : IState
  {
    private readonly GameStateMachine _stateMachine;
    private readonly IProgressService _progressService;

    public BootstrapState(GameStateMachine stateMachine, IProgressService progressService)
    {
      _progressService = progressService;
      _stateMachine = stateMachine;
    }

    public void Enter()
    {
      _progressService.LoadProgress();
      _stateMachine.Enter<GameLoopState>();
    }

    public void Exit() { }
  }
}