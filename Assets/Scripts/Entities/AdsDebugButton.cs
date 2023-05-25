using Services.Ads;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Entities
{
  public class AdsDebugButton : MonoBehaviour
  {
    public Button Button;
    private IAdsService _adsService;

    [Inject]
    private void Constructor(IAdsService adsService)
    {
      _adsService = adsService;
    }
    
    private void Awake()
    {
      Button.onClick.AddListener(OnClickHandler);
    }

    private void OnClickHandler()
    {
      _adsService.ShowDebugMenu();
    }
  }
}