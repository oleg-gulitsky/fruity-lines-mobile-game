using Logic.Game;
using UnityEngine;
using Zenject;

namespace UI.Board
{
  public class Fruit : MonoBehaviour
  {
    private IGame _game;
  
    [Inject]
    public void Construct(IGame game)
    {
      _game = game;
    }
  
    private void OnMouseDown()
    {
      _game.SelectFruit(gameObject);
    }
  }
}