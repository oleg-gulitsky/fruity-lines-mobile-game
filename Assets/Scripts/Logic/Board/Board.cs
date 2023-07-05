using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Logic.Points;
using Services.Progress;
using UI.Board;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;
using static Logic.Board.FruitType;
using Object = UnityEngine.Object;

namespace Logic.Board
{
  public class Board : IBoard
  {
    private const int GridSize = 9;
    private const int TurnAddedFruitsAmount = 3;

    private FruitType[,] _cells;
    private List<Fruit> _fruits = new();

    private GameObject _boardView;

    private IPoints _points;
    private IProgressService _progressService;
    private IInstantiator _instantiator;

    [Inject]
    public void Constructor(IInstantiator instantiator, IPoints points, IProgressService progressService)
    {
      _instantiator = instantiator;
      _points = points;
      _progressService = progressService;
    }

    public void SetBoardView(GameObject boardView)
    {
      _boardView = boardView;
    }

    public bool TryMakeMove(GameObject selectedFruit, GameObject cell, Action endMoveCallback)
    {
      Cell currentFruitCell = Tools.ConvertVector3ToCellData(selectedFruit.transform.localPosition);
      Cell targetCell = Tools.ConvertVector3ToCellData(cell.transform.localPosition);

      List<Cell> path = Pathfinder.GetPath(_cells, currentFruitCell, targetCell);

      if (path.Count <= 0)
        return false;

      Move(selectedFruit, path, endMoveCallback);

      _cells[targetCell.X, targetCell.Y] = _cells[currentFruitCell.X, currentFruitCell.Y];
      _cells[currentFruitCell.X, currentFruitCell.Y] = Empty;

      return true;
    }

    public async Task SetCells(FruitType[,] cells)
    {
      ClearBoard();

      _cells = cells;

      for (int i = 0; i < GridSize; i++)
      {
        for (int j = 0; j < GridSize; j++)
        {
          if (cells[i, j] == Empty)
            continue;

          await AddFruitOnBoard(new Cell(i, j, cells[i, j]));
        }
      }
    }

    public async Task ResetBoard()
    {
      ClearBoard();
      await TryAddFruits();
      UpdateGameProgress();
    }

    private async Task TryAddFruits()
    {
      List<Cell> emptyCells = Tools.GetEmptyCells(_cells);

      if (emptyCells.Count == 0)
        return;

      int addedFruitsAmount = Mathf.Min(TurnAddedFruitsAmount, emptyCells.Count);

      for (int i = 0; i < addedFruitsAmount; i++)
      {
        FruitType fruitType = Tools.GetRandomFruitType();

        int index = UnityEngine.Random.Range(0, emptyCells.Count);

        Cell cell = emptyCells[index];
        cell.FruitType = fruitType;

        _cells[cell.X, cell.Y] = fruitType;

        emptyCells.RemoveAt(index);

        await AddFruitOnBoard(cell);
        CheckLinesAndGetPoints(cell);
        await UniTask.Delay(100);
      }
    }

    private async void Move(GameObject selectedFruit, List<Cell> path, Action endMoveCallback)
    {
      foreach (Cell cellData in path)
      {
        selectedFruit.transform.localPosition = Tools.ConvertCellDataToVector3(cellData);
        await UniTask.Delay(150);
      }

      CheckLinesAndGetPoints(path[^1]);
      await TryAddFruits();

      UpdateGameProgress();

      CheckIsGameOver();
      
      endMoveCallback();
    }

    private void ClearBoard()
    {
      _cells = new FruitType[GridSize, GridSize];

      foreach (Fruit fruit in _fruits)
      {
        Object.Destroy(fruit.gameObject);
      }
      
      _fruits = new List<Fruit>();
    }

    private void CheckLinesAndGetPoints(Cell cell)
    {
      HashSet<Cell> destroyed = new();
      HashSet<Cell> line = new();

      List<Tuple<Vector2Int, Vector2Int>> tuples = new()
      {
        new Tuple<Vector2Int, Vector2Int>(new Vector2Int(1, 0), new Vector2Int(-1, 0)),
        new Tuple<Vector2Int, Vector2Int>(new Vector2Int(0, 1), new Vector2Int(0, -1)),
        new Tuple<Vector2Int, Vector2Int>(new Vector2Int(1, 1), new Vector2Int(-1, -1)),
        new Tuple<Vector2Int, Vector2Int>(new Vector2Int(1, -1), new Vector2Int(-1, 1))
      };

      foreach (Tuple<Vector2Int, Vector2Int> t in tuples)
      {
        line.UnionWith(Tools.CheckLine(_cells, cell, t.Item1.x, t.Item1.y));
        line.UnionWith(Tools.CheckLine(_cells, cell, t.Item2.x, t.Item2.y));

        if (line.Count >= 5)
          destroyed.UnionWith(line);

        line.Clear();
      }

      if (destroyed.Count <= 0)
        return;
      
      foreach (Cell fruitPosition in destroyed)
      {
        _cells[fruitPosition.X, fruitPosition.Y] = Empty;
        RemoveFruitFromBoard(fruitPosition);
      }
      
      _points.AddPoints(destroyed.Count);
    }

    private async Task AddFruitOnBoard(Cell cell)
    {
      if (_boardView == null)
        return;

      GameObject fruitPrefab = await GetFruitPrefab(cell.FruitType);
      
      Fruit fruit = _instantiator.InstantiatePrefab(
        fruitPrefab, Vector3.zero, Quaternion.identity, _boardView.transform
      ).GetComponent<Fruit>();
      
      Transform fruitTransform = fruit.transform;
      fruitTransform.localPosition = new Vector3(cell.X, cell.Y, -1);
      fruitTransform.localScale = Vector3.one;
      
      _fruits.Add(fruit);
    }

    private void RemoveFruitFromBoard(Cell cell)
    {
      Fruit removeFruit = null;

      foreach (Fruit fruit in _fruits)
      {
        if (Math.Abs(fruit.gameObject.transform.localPosition.x - cell.X) < 0.1
            && Math.Abs(fruit.gameObject.transform.localPosition.y - cell.Y) < 0.1)
        {
          removeFruit = fruit;
          Object.Destroy(fruit.gameObject);
        }
      }

      if (removeFruit)
        _fruits.Remove(removeFruit);
    }

    private static async Task<GameObject> GetFruitPrefab(FruitType fruitType)
    {
      return fruitType switch
      {
        Apple => await Addressables.LoadAssetAsync<GameObject>("Apple"),
        Banana => await Addressables.LoadAssetAsync<GameObject>("Banana"),
        Grape => await Addressables.LoadAssetAsync<GameObject>("Grape"),
        Pear => await Addressables.LoadAssetAsync<GameObject>("Pear"),
        Orange => await Addressables.LoadAssetAsync<GameObject>("Orange"),
        Empty => null,
        _ => null
      };
    }

    private void UpdateGameProgress()
    {
      _progressService.Push(new GameProgress((FruitType[,]) _cells.Clone(), _points.GetPoints()));
    }
    
    private void CheckIsGameOver()
    {
      List<Cell> emptyCells = Tools.GetEmptyCells(_cells);

      if (emptyCells.Count <= 0)
      {
        Debug.Log("Game Over");
      }
    }
  }
}