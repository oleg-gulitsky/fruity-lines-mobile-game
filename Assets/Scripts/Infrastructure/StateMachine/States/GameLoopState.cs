using GameLogic;

namespace Infrastructure.StateMachine.States
{
  public class GameLoopState : IState
  {
    private readonly IGameLogic _gameLogic;
    
    public GameLoopState(IGameLogic gameLogic)
    {
      _gameLogic = gameLogic;
    }

    public void Enter()
    {
      _gameLogic.StartGame();
    }

    public void Exit() {}
  }
}