using Cysharp.Threading.Tasks;

namespace Services.Ads
{
  public interface IAdsService
  {
    UniTask<bool> Initialize();
    UniTask<bool> TryShowRewarded();
    void ShowBanner();
    void HideBanner();
    void ShowDebugMenu();
  }
}