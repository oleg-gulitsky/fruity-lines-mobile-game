using Services.Ads;
using Services.Progress;

namespace Infrastructure.StateMachine.States
{
  public class BootstrapState : IState
  {
    private readonly GameStateMachine _stateMachine;
    private readonly IProgressService _progressService;
    private readonly IAdsService _adsService;

    public BootstrapState(GameStateMachine stateMachine, IProgressService progressService, IAdsService adsService)
    {
      _progressService = progressService;
      _adsService = adsService;
      _stateMachine = stateMachine;
    }

    public void Enter()
    {
      _adsService.Initialize();
      _progressService.LoadProgress();
      _stateMachine.Enter<GameLoopState>();
    }

    public void Exit() { }
  }
}