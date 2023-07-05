using Services.Ads;
using Zenject;

namespace UI.Buttons
{
  public class AdsDebugButton : ButtonBase
  {
    private IAdsService _adsService;

    [Inject]
    private void Constructor(IAdsService adsService)
    {
      _adsService = adsService;
    }

    protected override void OnClickHandler()
    {
      _adsService.ShowDebugMenu();
    }
  }
}