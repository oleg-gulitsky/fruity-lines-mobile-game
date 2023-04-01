using System;
using GameLogic;
using OdinSerializer;

namespace Services.Progress
{
  [Serializable]
  public class GameState
  {
    [OdinSerialize] public FruitType[,] Cells { get; set; }

    [OdinSerialize] public int Points { get; set; }

    public GameState(FruitType[,] cells, int points)
    {
      Cells = cells;
      Points = points;
    }
  }
}