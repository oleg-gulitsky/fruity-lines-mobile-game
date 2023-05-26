using Cysharp.Threading.Tasks;
using Zenject;

namespace Infrastructure.SceneLoading
{
  public class SceneLoader : ISceneLoader
  {
    private readonly ZenjectSceneLoader _zenjectLoader;

    public SceneLoader(ZenjectSceneLoader zenjectLoader)
    {
      _zenjectLoader = zenjectLoader;
    }

    public async UniTask LoadSceneAsync(string sceneName) =>
      await _zenjectLoader.LoadSceneAsync(sceneName);
  }
}