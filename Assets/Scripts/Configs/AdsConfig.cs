using System;
using UnityEngine;

namespace Configs
{
  [Serializable]
  public struct AdsUnitsIds
  {
    public string rewardedId;
    public string interstitialId;
    public string bannerId;
    public string appOpenId;
  }
  
  [CreateAssetMenu(fileName = "AdsConfig", menuName = "Configs/Ads")]
  public class AdsConfig : ScriptableObject
  {
    [SerializeField] private string _maxSdkKey;
    [SerializeField, Space(10)] private AdsUnitsIds _androidUnitsIds;
    [SerializeField, Space(10)] private AdsUnitsIds _iOSUnitsIds;
    
    public string MAXSdkKey => _maxSdkKey;
    public AdsUnitsIds AndroidUnitsIds => _androidUnitsIds;
    public AdsUnitsIds IOSUnitsIds => _iOSUnitsIds;
  }
}