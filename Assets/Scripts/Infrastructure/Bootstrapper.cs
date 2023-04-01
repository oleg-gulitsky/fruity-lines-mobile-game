using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
  public class Bootstrapper : MonoBehaviour
  {
    private IGameStateMachine _gameStateMachine;

    [Inject]
    public void Construct(IGameStateMachine gameStateMachine)
    {
      _gameStateMachine = gameStateMachine;
    }

    private void Start()
    {
      _gameStateMachine.Enter<BootstrapState>();
    }
  }
}