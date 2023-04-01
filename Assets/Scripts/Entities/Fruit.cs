using GameLogic;
using UnityEngine;
using Zenject;

namespace Entities
{
  public class Fruit : MonoBehaviour
  {
    private IGameLogic _gameLogic;
  
    [Inject]
    public void Construct(IGameLogic gameLogic)
    {
      _gameLogic = gameLogic;
    }
  
    private void OnMouseDown()
    {
      _gameLogic.SelectFruit(gameObject);
    }
  }
}