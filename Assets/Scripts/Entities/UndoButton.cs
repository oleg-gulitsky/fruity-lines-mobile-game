using GameLogic;
using Services.Ads;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Entities
{
  public class UndoButton : MonoBehaviour
  {
    public Button Button;
    private IGameLogic _gameLogic;
    private IAdsService _adsService;

    [Inject]
    private void Constructor(IGameLogic gameLogic, IAdsService adsService)
    {
      _gameLogic = gameLogic;
      _adsService = adsService;
    }
    
    private void Awake()
    {
      Button.onClick.AddListener(OnClickHandler);
    }

    private async void OnClickHandler()
    {
      bool shown = await _adsService.TryShowRewarded();

      if (shown)
      {
        _gameLogic.UndoMove(); 
      }
    }
  }
}