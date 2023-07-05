using System;

namespace Logic.Board
{
  [Serializable]
  public class Cell
  {
    public FruitType FruitType;
    public readonly int X;
    public readonly int Y;

    public Cell(int x, int y, FruitType fruitType = FruitType.Empty)
    {
      X = x;
      Y = y;
      FruitType = fruitType;
    }
    
    public override bool Equals(object obj)
    {
      return Equals(obj as Cell);
    }

    public bool Equals(Cell other)
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