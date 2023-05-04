using System;
using System.Collections.Generic;
using GameLogic;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Entities
{
  public class BoardGrid : MonoBehaviour
  {
    public GameObject ApplePrefab;
    public GameObject BananaPrefab;
    public GameObject GrapePrefab;
    public GameObject PearPrefab;
    public GameObject OrangePrefab;

    private IGameLogic _gameLogic;
    private IInstantiator _container;

    [Inject]
    public void Construct(IInstantiator container, GameLogic.GameLogic gameLogic)
    {
      _container = container;
      _gameLogic = gameLogic;
      _gameLogic.AddOneFruitToBoard += AddOneFruitHandler;
      _gameLogic.AddSomeFruitsToBoard += AddSomeFruitsHandler;
      _gameLogic.RemoveFruitsFromBoard += RemoveFruitsHandler;
    }

    private void OnDestroy()
    {
      _gameLogic.AddSomeFruitsToBoard -= AddSomeFruitsHandler;
      _gameLogic.RemoveFruitsFromBoard -= RemoveFruitsHandler;
    }

    private void AddOneFruitHandler(CellData cellData)
    {
      AddFruit(cellData);
    }

    private void AddSomeFruitsHandler(List<CellData> cellsData)
    {
      foreach (CellData cellData in cellsData)
      {
        AddFruit(cellData);
      }
    }

    private void AddFruit(CellData cellData)
    {
      GameObject fruit = _container.InstantiatePrefab(
        GetFruitPrefab(cellData.FruitType), Vector3.zero, Quaternion.identity, transform);
      fruit.transform.localPosition = new Vector3(cellData.X, cellData.Y, -1);
      fruit.transform.localScale = Vector3.one;
    }

    private Object GetFruitPrefab(FruitType fruitType)
    {
      return fruitType switch
      {
        FruitType.Apple => ApplePrefab,
        FruitType.Banana => BananaPrefab,
        FruitType.Grape => GrapePrefab,
        FruitType.Pear => PearPrefab,
        FruitType.Orange => OrangePrefab,
        FruitType.Empty => null,
        _ => null
      };
    }

    private void RemoveFruitsHandler(List<CellData> cells)
    {
      Fruit[] fruits = gameObject.GetComponentsInChildren<Fruit>();
      
      foreach (CellData cell in cells)
      {
        foreach (Fruit fruit in fruits)
        {
          if (Math.Abs(fruit.gameObject.transform.localPosition.x - cell.X) < 0.1 
              && Math.Abs(fruit.gameObject.transform.localPosition.y - cell.Y) < 0.1)
          {
            Destroy(fruit.gameObject);
          }
        }
      }
    }
  }
}