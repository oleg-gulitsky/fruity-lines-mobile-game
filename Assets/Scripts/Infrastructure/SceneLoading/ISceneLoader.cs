using Cysharp.Threading.Tasks;

namespace Infrastructure.SceneLoading
{
  public interface ISceneLoader
  {
    public UniTask LoadSceneAsync(string sceneName);
  }
}