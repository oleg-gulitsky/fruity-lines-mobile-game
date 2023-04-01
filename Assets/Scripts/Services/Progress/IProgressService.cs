using System.Collections.Generic;
using GameLogic;

namespace Services.Progress
{
  public interface IProgressService
  {
    public void LoadProgress();
    public void UpdateGameProgress(FruitType[,] cells, int points);
    public GameState PopGameState();
    public GameState GetGameState();
    public void ClearGameProgress();
  }
}