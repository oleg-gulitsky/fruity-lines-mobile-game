using GameLogic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Entities
{
  public class UndoButton : MonoBehaviour
  {
    public Button Button;
    private IGameLogic _gameLogic;

    [Inject]
    private void Constructor(IGameLogic gameLogic)
    {
      _gameLogic = gameLogic;
    }
    
    private void Awake()
    {
      Button.onClick.AddListener(OnClickHandler);
    }

    private void OnClickHandler()
    {
      _gameLogic.UndoMove();
    }
  }
}