using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Logic.Board
{
  public static class Tools
  {
    public static List<Cell> GetEmptyCells(FruitType[,] cells)
    {
      List<Cell> emptyCoords = new();

      for (int i = 0; i < cells.GetLength(0); i++)
      {
        for (int j = 0; j < cells.GetLength(1); j++)
        {
          if (cells[i, j] == FruitType.Empty)
            emptyCoords.Add(new Cell(i, j));
        }
      }

      return emptyCoords;
    }
    
    public static FruitType GetRandomFruitType()
    {
      IEnumerable<FruitType> fruitTypes = 
        Enum.GetValues(typeof(FruitType)).Cast<FruitType>().ToArray();
      int min = (int) fruitTypes.Min() + 1;
      int max = (int) fruitTypes.Max() + 1;

      return (FruitType) UnityEngine.Random.Range(min, max);
    }

    public static Cell ConvertVector3ToCellData(Vector3 vector3) => 
      new((int) vector3.x, (int) vector3.y);

    public static Vector3 ConvertCellDataToVector3(Cell cell) => 
      new(cell.X, cell.Y, -1);
    
    public static IEnumerable<Cell> CheckLine(FruitType[,] cells, Cell cell, int dx, int dy)
    {
      HashSet<Cell> line = new();
      
      int gridSize = cells.GetLength(0);
      
      for (int i = 0; i < gridSize; i++)
      {
        int x = cell.X + i * dx;
        int y = cell.Y + i * dy;
        
        if (Pathfinder.IsInBounds(cells, x, y) 
            && (cells[x, y] == cells[cell.X, cell.Y]))
        {
          line.Add(new Cell(x, y));
        }
        else
        {
          break;
        }
      }
      return line;
    }
  }
}