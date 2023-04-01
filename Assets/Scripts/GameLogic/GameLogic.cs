using System;
using System.Collections;
using System.Collections.Generic;
using Infrastructure;
using Services.Progress;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using static GameLogic.FruitType;

namespace GameLogic
{
  public class GameLogic : IGameLogic
  {
    public event Action<List<CellData>> AddSomeFruitsToBoard;
    public event Action<CellData> AddOneFruitToBoard;
    public event Action<List<CellData>> RemoveFruitsFromBoard;
    public event Action<string> UpdatePointsUI;

    private const int GridSize = 9;
    private const int TurnAddedFruitsAmount = 3;

    private FruitType[,] _cells;
    private int _points;
    private GameObject _selectedFruit;
    private bool _isFruitMoving;

    private AsyncProcessor _asyncProcessor;
    private IProgressService _progressService;

    [Inject]
    public void Constructor(AsyncProcessor asyncProcessor, IProgressService progressService)
    {
      _asyncProcessor = asyncProcessor;
      _progressService = progressService;
    }

    private void SetPoints(int newPoints)
    {
      _points = newPoints;
      UpdatePointsUI?.Invoke(_points.ToString());
    }
    
    public void SelectFruit(GameObject fruit)
    {
      if (_isFruitMoving)
        return;

      _selectedFruit = fruit;
    }

    public void SelectCell(GameObject cell)
    {
      if (_selectedFruit == null || _isFruitMoving)
        return;
      CellData cellData = Tools.ConvertVector3ToCellData(cell.transform.localPosition);

      TryMakeMove(cellData);
    }

    private void TryAddFruits()
    {
      List<CellData> emptyCells = GetEmptyCells();

      if (emptyCells.Count == 0) return;

      int addedFruitsAmount = Mathf.Min(TurnAddedFruitsAmount, emptyCells.Count);
      
      for (int i = 0; i < addedFruitsAmount; i++)
      {
        FruitType fruitType = Tools.GetRandomFruitType();

        int index = UnityEngine.Random.Range(0, emptyCells.Count);

        CellData cellData = emptyCells[index];
        cellData.FruitType = fruitType;
        
        _cells[cellData.X, cellData.Y] = fruitType;
        
        emptyCells.RemoveAt(index);

        AddOneFruitToBoard?.Invoke(cellData);
        TryDestroyLines(cellData);
      }
    }

    private List<CellData> GetEmptyCells()
    {
      List<CellData> emptyCoords = new();

      for (int i = 0; i < GridSize; i++)
      {
        for (int j = 0; j < GridSize; j++)
        {
          if (_cells[i, j] == Empty)
            emptyCoords.Add(new CellData(i, j));
        }
      }
      
      return emptyCoords;
    }

    private void TryMakeMove(CellData targetCell)
    {
      CellData currentFruitCell = Tools.ConvertVector3ToCellData(_selectedFruit.transform.localPosition);
      
      List<CellData> path = Pathfinder.GetPath(_cells, currentFruitCell, targetCell);

      if (path.Count <= 0)
        return;

      _isFruitMoving = true;
      _asyncProcessor.StartCoroutine(Move(path));

      _cells[targetCell.X, targetCell.Y] = _cells[currentFruitCell.X, currentFruitCell.Y];
      _cells[currentFruitCell.X, currentFruitCell.Y] = Empty;
    }

    private IEnumerator Move(List<CellData> path)
    {
      foreach (CellData cellData in path)
      {
        _selectedFruit.transform.localPosition = Tools.ConvertCellDataToVector3(cellData);
        yield return new WaitForSeconds(0.15f);
      }

      UpdateGameState(path[^1]);
    }

    private void UpdateGameState(CellData newSelectedFruitPosition)
    {
      TryDestroyLines(newSelectedFruitPosition);
      TryAddFruits();
      UpdateProgress();

      _selectedFruit = null;
      _isFruitMoving = false;
    }

    private void UpdateProgress()
    {
      _progressService.UpdateGameProgress(_cells, _points);
    }

    public void UndoMove()
    {
      if (_isFruitMoving)
        return;

      ClearBoard();
      
      _progressService.PopGameState();
      GameState previousGameState = _progressService.GetGameState();
      
      List<CellData> addFruitCoords = new();

      for (int i = 0; i < GridSize; i++)
      {
        for (int j = 0; j < GridSize; j++)
        {
          if(previousGameState.Cells[i, j] != Empty)
            addFruitCoords.Add(new CellData(i, j, previousGameState.Cells[i, j]));
        }
      }

      if (addFruitCoords.Count > 0)
        AddSomeFruitsToBoard?.Invoke(addFruitCoords);
      
      _cells = (FruitType[,]) previousGameState.Cells.Clone();
      
      SetPoints(previousGameState.Points);
    }

    public void Restart()
    {
      _progressService.ClearGameProgress();
      SetPoints(0);
      ClearBoard();
      StartNewGame();
    }

    private void StartNewGame()
    {
      _cells = new FruitType[GridSize, GridSize];
      TryAddFruits();
      UpdateProgress();
    }

    public void StartGame()
    {
      GameState currentGameState = _progressService.GetGameState();
      if (currentGameState.IsUnityNull())
      {
        StartNewGame();
      }
      else
      {
        ResumeGame(currentGameState);
      }
    }

    private void ResumeGame(GameState currentGameState)
    {
      _cells = currentGameState.Cells;
      SetPoints(currentGameState.Points);

      List<CellData> addedFruitCoord = new();
      
      for (int i = 0; i < GridSize; i++)
      {
        for (int j = 0; j < GridSize; j++)
        {
          if (_cells[i, j] == Empty)
            continue;

          addedFruitCoord.Add(new CellData(i, j, _cells[i, j]));
        }
      }
      
      AddSomeFruitsToBoard?.Invoke(addedFruitCoord);
    }

    private void ClearBoard()
    {
      _cells = new FruitType[GridSize, GridSize];
      
      List<CellData> removeFruitCoords = new();
      
      for (int i = 0; i < GridSize; i++)
      {
        for (int j = 0; j < GridSize; j++)
        {
          removeFruitCoords.Add(new CellData(i, j));
        }
      }
      
      if(removeFruitCoords.Count > 0)
        RemoveFruitsFromBoard?.Invoke(removeFruitCoords);
    }
    
    private void TryDestroyLines(CellData cellData)
    {
      HashSet<CellData> destroyed = new();
      HashSet<CellData> line = new();

      List<Tuple<Vector2Int, Vector2Int>> tuples = new()
      {
        new Tuple<Vector2Int, Vector2Int>(new Vector2Int(1, 0), new Vector2Int(-1, 0)),
        new Tuple<Vector2Int, Vector2Int>(new Vector2Int(0, 1), new Vector2Int(0, -1)),
        new Tuple<Vector2Int, Vector2Int>(new Vector2Int(1, 1), new Vector2Int(-1, -1)),
        new Tuple<Vector2Int, Vector2Int>(new Vector2Int(1, -1), new Vector2Int(-1, 1))
      };

      foreach (Tuple<Vector2Int, Vector2Int> t in tuples)
      {
        line.UnionWith(Tools.CheckLine(_cells, cellData, t.Item1.x, t.Item1.y));
        line.UnionWith(Tools.CheckLine(_cells, cellData, t.Item2.x, t.Item2.y));

        if (line.Count >= 5)
          destroyed.UnionWith(line);

        line.Clear();
      }

      if (destroyed.Count <= 0)
        return;
      
      List<CellData> removeFruitCoords = new();

      foreach (CellData fruitPosition in destroyed)
      {
        _cells[fruitPosition.X, fruitPosition.Y] = Empty;
        
        removeFruitCoords.Add(fruitPosition);
      }

      RemoveFruitsFromBoard?.Invoke(removeFruitCoords);
      
      SetPoints(_points + destroyed.Count);
    }
  }
}