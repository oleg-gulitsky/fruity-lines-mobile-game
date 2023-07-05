using Logic.Board;

namespace Services.Progress
{
  public interface IProgressService
  {
    public void Load();
    public void Push(GameProgress gameProgress);
    public GameProgress Pop();
    public GameProgress Get();
    public void Clear();
  }
}