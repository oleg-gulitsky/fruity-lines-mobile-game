using System.Collections.Generic;
using UnityEngine;
using static Logic.Board.FruitType;

namespace Logic.Board
{
  public static class Pathfinder
  {
    private static readonly Vector2Int[] Dxdy =
    {
      Vector2Int.down,
      Vector2Int.left,
      Vector2Int.right,
      Vector2Int.up,
    };
    
    public static List<Cell> GetPath(FruitType[,] cells, Cell currentPosition, Cell targetPosition)
    {
      
      int[,] wave = GetWave(cells, currentPosition, targetPosition);
      List<Cell> path = new();

      if (wave[targetPosition.X, targetPosition.Y] < 1)
        return path;

      Cell current = targetPosition;

      path.Add(current);

      while (!current.Equals(currentPosition))
      {
        bool stop = true;

        foreach (Vector2Int t in Dxdy)
        {
          int x = (current.X + t.x);
          int y = (current.Y + t.y);

          if (IsInBounds(cells, x, y) && (wave[x, y] == wave[current.X, current.Y] - 1))
          {
            current = new Cell(x, y);
            path.Insert(0, current);
            stop = false;
          }
        }

        if (stop)
          break;
      }

      if (path.Count <= 0)
        return path;

      if (path[0].X != currentPosition.X && path[0].Y != currentPosition.Y)
        path.Clear();

      return path;
    }

    private static int[,] GetWave(FruitType[,] cells, Cell from, Cell to)
    {
      int rowsAmount = cells.GetLength(0);
      int columnAmount = cells.GetLength(1);
      
      int[,] wave = new int[rowsAmount, columnAmount];

      for (int i = 0; i < rowsAmount; i++)
      {
        for (int j = 0; j < columnAmount; j++)
        {
          wave[i, j] = cells[i, j] == Empty ? 0 : -1;
        }
      }

      int d = 1;

      wave[from.X, from.Y] = d;

      while (true)
      {
        bool stop = true;

        for (int i = 0; i < rowsAmount; i++)
        {
          for (int j = 0; j < columnAmount; j++)
          {
            if (wave[i, j] != d)
              continue;
            foreach (Vector2Int t in Dxdy)
            {
              int x = i + t.x;
              int y = j + t.y;

              if (IsInBounds(cells, x, y) && wave[x, y] == 0)
              {
                wave[x, y] = d + 1;

                stop = false;
              }
            }
          }
        }

        d++;

        if (wave[to.X, to.Y] != 0)
          break;

        if (stop)
          break;
      }
      
      return wave;
    }

    public static bool IsInBounds(FruitType[,] cells, int x, int y)
    {
      int rowsAmount = cells.GetLength(0);
      int columnAmount = cells.GetLength(1);
      
      return (x >= 0) && (x < rowsAmount) && (y >= 0) && (y < columnAmount); 
    }
  }
}