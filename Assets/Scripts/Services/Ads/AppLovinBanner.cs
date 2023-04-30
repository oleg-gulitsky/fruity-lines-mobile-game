using UnityEngine;

namespace Services.Ads
{
  public class AppLovinBanner
  {
    private static readonly Color BannerBackgroundColor = new(0, 0, 0, 0);

    private readonly string _bannerId;

    public AppLovinBanner(string bannerId)
    {
      _bannerId = bannerId;
    }

    public void InitializeBannerAds()
    {
      MaxSdk.CreateBanner(_bannerId, MaxSdkBase.BannerPosition.BottomCenter);
      MaxSdk.SetBannerBackgroundColor(_bannerId, BannerBackgroundColor);
      MaxSdk.SetBannerExtraParameter(_bannerId, "adaptive_banner", "true");
    }

    public void ShowBanner() =>
      MaxSdk.ShowBanner(_bannerId);
    
    public void HideBanner() =>
      MaxSdk.HideBanner(_bannerId);
  }
}