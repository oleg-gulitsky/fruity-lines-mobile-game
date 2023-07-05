using Logic.Game;
using Zenject;

namespace UI.Buttons
{
  public class UndoButton : ButtonBase
  {
    private IGame _game;

    [Inject]
    private void Constructor(IGame game)
    {
      _game = game;
    }

    protected override async void OnClickHandler()
    {
      await _game.TryUndoMove();
    }
  }
}