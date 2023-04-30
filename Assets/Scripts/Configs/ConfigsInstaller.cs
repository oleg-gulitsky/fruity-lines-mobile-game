using UnityEngine;
using Zenject;

namespace Configs
{
  [CreateAssetMenu(fileName = "ConfigsInstaller", menuName = "Configs/Installer")]
  public class ConfigsInstaller : ScriptableObjectInstaller<ConfigsInstaller>
  {
    [SerializeField] private AdsConfig _adsConfig;

    public override void InstallBindings()
    {
      Container.BindInstances(
        _adsConfig
      );
    }
  }
}