using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameLogic
{
  public static class Tools
  {
    public static FruitType GetRandomFruitType()
    {
      IEnumerable<FruitType> fruitTypes = 
        Enum.GetValues(typeof(FruitType)).Cast<FruitType>().ToArray();
      int min = (int) fruitTypes.Min() + 1;
      int max = (int) fruitTypes.Max() + 1;

      return (FruitType) UnityEngine.Random.Range(min, max);
    }

    public static CellData ConvertVector3ToCellData(Vector3 vector3) => 
      new((int) vector3.x, (int) vector3.y);

    public static Vector3 ConvertCellDataToVector3(CellData cellData) => 
      new(cellData.X, cellData.Y, -1);
    
    public static IEnumerable<CellData> CheckLine(FruitType[,] cells, CellData cellData, int dx, int dy)
    {
      HashSet<CellData> line = new();
      
      int gridSize = cells.GetLength(0);
      
      for (int i = 0; i < gridSize; i++)
      {
        int x = cellData.X + i * dx;
        int y = cellData.Y + i * dy;
        
        if (Pathfinder.IsInBounds(cells, x, y) 
            && (cells[x, y] == cells[cellData.X, cellData.Y]))
        {
          line.Add(new CellData(x, y));
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