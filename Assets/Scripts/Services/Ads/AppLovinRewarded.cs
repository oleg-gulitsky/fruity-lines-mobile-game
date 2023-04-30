using Cysharp.Threading.Tasks;

namespace Services.Ads
{
  public class AppLovinRewarded
  {
    private UniTaskCompletionSource<bool> _showCompletion;
    
    private bool _isRewarded;
    private readonly string _rewardedId;
    
    public AppLovinRewarded(string rewardedId)
    {
      _rewardedId = rewardedId;
    }
    
    public void InitializeRewardedAds()
    {
      MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnFailedToDisplayListener;
      MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnHiddenListener;
      MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnReceivedRewardListener;
      
      LoadRewardedAd();
    }

    private void LoadRewardedAd()
    {
      MaxSdk.LoadRewardedAd(_rewardedId);
    }
    
    public UniTask<bool> TryShowRewarded()
    {
      _showCompletion = new UniTaskCompletionSource<bool>();

      if (!MaxSdk.IsRewardedAdReady(_rewardedId))
      {
        return UniTask.FromResult(false);
      }

      MaxSdk.ShowRewardedAd(_rewardedId);

      return _showCompletion.Task;
    }

    private void OnFailedToDisplayListener(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo _)
    {
      _showCompletion?.TrySetResult(false);
      LoadRewardedAd();
    }
    
    private void OnHiddenListener(string adUnitId, MaxSdkBase.AdInfo _)
    {
      _showCompletion?.TrySetResult(_isRewarded);
      _isRewarded = false;
      LoadRewardedAd();
    }
    
    private void OnReceivedRewardListener(string adUnitId, MaxSdkBase.Reward reward, MaxSdkBase.AdInfo _)
    {  
      _isRewarded = true;
    }
  }
}