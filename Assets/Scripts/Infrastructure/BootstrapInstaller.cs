using Infrastructure.SceneLoading;
using Infrastructure.StateMachine;
using Logic.Board;
using Logic.Game;
using Logic.Points;
using Services.Ads;
using Services.Progress;
using Zenject;

namespace Infrastructure
{
  public class BootstrapInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      Container.BindInterfacesAndSelfTo<SceneLoader>().AsSingle();
      Container.BindInterfacesAndSelfTo<AdsService>().AsSingle();
      Container.BindInterfacesAndSelfTo<ProgressService>().AsSingle();
      Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle();
      Container.BindInterfacesAndSelfTo<Board>().AsSingle();
      Container.BindInterfacesAndSelfTo<Points>().AsSingle();
      Container.BindInterfacesAndSelfTo<Game>().AsSingle();
    }
  }
}