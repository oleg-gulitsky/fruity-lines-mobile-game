using Infrastructure.SceneLoading;
using Logic.Game;

namespace Infrastructure.StateMachine.States
{
  public class GameplayState : IState
  {
    private readonly ISceneLoader _sceneLoader;
    private readonly IGame _game;

    public GameplayState(ISceneLoader sceneLoader, IGame game)
    {
      _sceneLoader = sceneLoader;
      _game = game;
    }

    public async void Enter()
    {
      await _sceneLoader.LoadSceneAsync(SceneName.Main);
      _game.StartGame();
    }

    public void Exit() {}
  }
}