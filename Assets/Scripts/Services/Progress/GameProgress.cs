using System;
using Logic.Board;
using OdinSerializer;

namespace Services.Progress
{
  [Serializable]
  public class GameProgress
  {
    [OdinSerialize] public FruitType[,] Cells { get; set; }

    [OdinSerialize] public int Points { get; set; }

    public GameProgress(FruitType[,] cells, int points)
    {
      Cells = cells;
      Points = points;
    }
  }
}