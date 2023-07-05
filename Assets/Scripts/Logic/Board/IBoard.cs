using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Logic.Board
{
  public interface IBoard
  {
    void SetBoardView(GameObject gridView);
    bool TryMakeMove(GameObject selectedFruit, GameObject targetCell, Action endMoveCallback);
    Task ResetBoard();
    Task SetCells(FruitType[,] cells);
  }
}