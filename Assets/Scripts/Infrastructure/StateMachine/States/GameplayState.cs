using GameLogic;
using Infrastructure.SceneLoading;

namespace Infrastructure.StateMachine.States
{
  public class GameplayState : IState
  {
    private readonly ISceneLoader _sceneLoader;
    private readonly IGameLogic _gameLogic;

    public GameplayState(ISceneLoader sceneLoader, IGameLogic gameLogic)
    {
      _sceneLoader = sceneLoader;
      _gameLogic = gameLogic;
    }

    public async void Enter()
    {
      await _sceneLoader.LoadSceneAsync(SceneName.Main);
      _gameLogic.StartGame();
    }

    public void Exit() {}
  }
}