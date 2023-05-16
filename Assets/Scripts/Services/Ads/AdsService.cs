using Configs;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Services.Ads
{
  public class AdsService : IAdsService
  {
    private string _sdkKey;
    private AdsUnitsIds _adsUnitsIds;
    private AppLovinRewarded _rewarded;
    private AppLovinBanner _banner;

    [Inject]
    public void Constructor(AdsConfig adsConfig)
    {
      _sdkKey = adsConfig.MAXSdkKey;

#if UNITY_IOS
      _adsUnitsIds = adsConfig.IOSUnitsIds;
#elif UNITY_ANDROID
      _adsUnitsIds = adsConfig.AndroidUnitsIds;
#endif

      _rewarded = new AppLovinRewarded(_adsUnitsIds.rewardedId);
      _banner = new AppLovinBanner(_adsUnitsIds.bannerId);
    }

    public UniTask<bool> Initialize()
    {
      UniTaskCompletionSource<bool> init = new();

      MaxSdkCallbacks.OnSdkInitializedEvent += (_) =>
      {
        init.TrySetResult(true);
      };

      MaxSdk.SetSdkKey(_sdkKey);
      MaxSdk.InitializeSdk();
      InitializeAds();

      return init.Task;
    }

    private void InitializeAds()
    {
      _rewarded.InitializeRewardedAds();
      _banner.InitializeBannerAds();
      ShowBanner();
    }

    public UniTask<bool> TryShowRewarded() =>
      _rewarded.TryShowRewarded();

    public void ShowBanner() =>
      _banner.ShowBanner();

    public void HideBanner() => 
      _banner.HideBanner();

    public void ShowDebugMenu() =>
      MaxSdk.ShowMediationDebugger();
  }
}