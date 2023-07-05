using Logic.Game;
using Zenject;

namespace UI.Buttons
{
  public class RestartButton : ButtonBase
  {
    private IGame _game;
  
    [Inject]
    public void Construct(IGame game)
    {
      _game = game;
    }
    
    protected override void OnClickHandler()
    {
      _game.StartNewGame();
    }
  }
}