using System;

namespace GameLogic
{
  [Serializable]
  public class CellData
  {
    public FruitType FruitType;
    public readonly int X;
    public readonly int Y;

    public CellData(int x, int y, FruitType fruitType = FruitType.Empty)
    {
      X = x;
      Y = y;
      FruitType = fruitType;
    }
    
    public override bool Equals(object obj)
    {
      return Equals(obj as CellData);
    }

    public bool Equals(CellData other)
    {
      return other != null &&
        X == other.X &&
        Y == other.Y;
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(X, Y);
    }
  }
}