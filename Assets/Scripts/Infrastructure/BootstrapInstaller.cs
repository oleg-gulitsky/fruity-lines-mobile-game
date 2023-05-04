using Infrastructure.StateMachine;
using Services.Ads;
using Services.Progress;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
  public class BootstrapInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      Container.BindInterfacesAndSelfTo<AdsService>().AsSingle();
      Container.BindInterfacesAndSelfTo<ProgressService>().AsSingle();
      Container.BindInterfacesAndSelfTo<GameLogic.GameLogic>().AsSingle();
      Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle();
    }
  }
}