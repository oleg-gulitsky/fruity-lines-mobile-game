using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
  public interface IGameLogic
  {
    public void StartGame();
    void SelectFruit(GameObject fruit);
    void SelectCell(GameObject cell);
    void UndoMove();
    void Restart();
    event Action<List<CellData>> AddSomeFruitsToBoard;
    event Action<CellData> AddOneFruitToBoard;
    event Action<List<CellData>> RemoveFruitsFromBoard;
    event Action<string> UpdatePointsUI;
  }
}