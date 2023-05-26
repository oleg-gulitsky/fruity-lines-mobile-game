using GameLogic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Entities
{
  public class Points : MonoBehaviour
  {
    [SerializeField] private Text text;

    private IGameLogic _gameLogic;

    [Inject]
    private void Constructor(IGameLogic gameLogic)
    {
      _gameLogic = gameLogic;
    }

    private void Awake()
    {
      _gameLogic.UpdatePointsUI += UpdateHandler;
    }
    
    private void OnDestroy()
    {
      _gameLogic.UpdatePointsUI -= UpdateHandler;
    }

    private void UpdateHandler(string newValue)
    {
      text.text = newValue;
    }
  }
}