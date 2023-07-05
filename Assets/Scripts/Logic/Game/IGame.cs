using System.Threading.Tasks;
using UnityEngine;

namespace Logic.Game
{
  public interface IGame
  {
    void SelectFruit(GameObject fruit);
    void SelectCell(GameObject cell);
    Task<bool> TryUndoMove();
    void StartGame();
    void StartNewGame();
  }
}